namespace Minary.Domain.Input
{
  using Minary.Form;
  using Minary.LogConsole.Main;
  using System;
  using System.Collections.Concurrent;
  using System.IO;
  using System.IO.Pipes;
  using System.Text.RegularExpressions;
  using System.Threading;
  using System.Windows.Forms;


  public class InputHandler
  {

    #region MEMBERS

    private static bool stopThreads = false;

    private MinaryMain minaryMain;
    private ConcurrentDictionary<string, DataTypes.MinaryExtension> tabPageCatalog;
    private Thread[] inputWorkerThreads = new Thread[Config.PipeInstances];
    private Thread processInputDataThread;
    private NamedPipeServerStream[] pipeStream = new NamedPipeServerStream[Config.PipeInstances];
    private StreamReader[] streamReader = new StreamReader[Config.PipeInstances];
    private ConcurrentQueue<string> inputDataQueue;

    #endregion


    #region PROPERTIES

    public bool IsBeepOn { get; set; }

    #endregion


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="InputHandler"/> class.
    ///
    /// </summary>
    /// <param name="minaryMain"></param>
    public InputHandler(MinaryMain minaryMain)
    {
      this.minaryMain = minaryMain;
      this.IsBeepOn = false;
      this.tabPageCatalog = this.minaryMain.PluginHandler.TabPagesCatalog;
      this.inputDataQueue = new ConcurrentQueue<string>();
      this.processInputDataThread = new Thread(this.DataProcessingThread);
      this.processInputDataThread.Start();
    }


    /// <summary>
    ///
    /// </summary>
    public void StartInputThread()
    {
      stopThreads = false;

      try
      {
        // We have several concurrently running NamedPipes reading
        // input data. Start them all.
        for (int i = 0; i < Config.PipeInstances; i++)
        {
          try
          {
            if (this.pipeStream[i] != null)
            {
              this.pipeStream[i].Close();
              this.pipeStream[i] = null;
            }
          }
          catch (Exception ex)
          {
            LogCons.Inst.Write("Can't start named pipe - " + ex.StackTrace + "\n" + ex.ToString());
            MessageBox.Show("Can't start named pipe : " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }

          this.inputWorkerThreads[i] = new Thread(new ParameterizedThreadStart(this.DataInputThread));
          this.inputWorkerThreads[i].Start(i);
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write("An error occurred while starting the sniffer : " + ex.StackTrace + "\n" + ex.ToString());
        MessageBox.Show("An error occurred while starting the sniffer : " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }



    /// <summary>
    ///
    /// </summary>
    public void StopInputThreads()
    {
      NamedPipeClientStream namedPipeClient = null;
      StreamWriter streamWriter = null;

      stopThreads = true;

      if (this.pipeStream == null)
      {
        return;
      }

      for (int i = 0; i < Config.PipeInstances; i++)
      {
        try
        {
          ClosePipeStream(this.pipeStream[i], this.streamReader[i]);

          namedPipeClient = new NamedPipeClientStream(".", Config.PipeName, PipeDirection.InOut);
          streamWriter = new StreamWriter(namedPipeClient);

          namedPipeClient.Connect(500);
          streamWriter.AutoFlush = true;
          streamWriter.WriteLine("QUIT\r\n");
          streamWriter.Close();

          namedPipeClient.Close();
          namedPipeClient = null;
          /*
             if (mInputWorkerThread != null)
             {
                 System.Threading.Thread.Sleep(500);
               try { mInputWorkerThread[i].Abort(); } catch { }
                 mInputWorkerThread.Join();
               try { mInputWorkerThread[i].Interrupt();  } catch { }
             }
          */
        }
        catch (TimeoutException tex)
        {
          LogCons.Inst.Write(tex.StackTrace + "\n" + tex.ToString());
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write("An error occurred while starting the sniffer : " + ex.StackTrace + "\n" + ex.ToString());
        }
        finally
        {
          if (streamWriter != null)
          {
            try
            {
              streamWriter.Close();
            }
            catch
            {
            }
          }


          if (namedPipeClient != null)
          {
            try
            {
              namedPipeClient.Close();
            }
            catch
            {
            }

            namedPipeClient = null;
          }
        }
      }


      for (int i = 0; i < Config.PipeInstances; i++)
      {
        if (this.pipeStream == null || this.pipeStream[i] == null)
        {
          continue;
        }

        try
        {
          this.pipeStream[i].Disconnect();
          this.pipeStream[i].Close();
          this.pipeStream[i].Dispose();
        }
        catch
        {
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    public void DataProcessingThread()
    {
      string tmpRecord;
      bool processIsStopped = false;

      while (processIsStopped == false)
      {
        // Process all records in queue
        while (!this.inputDataQueue.IsEmpty)
        {
          if (this.inputDataQueue.TryDequeue(out tmpRecord) == false)
          {
            continue;
          }

          try
          {
            if (tmpRecord.StartsWith("QUIT"))
            {
              LogCons.Inst.Write("Minary.DataInput.InputModule.DataProcessingThread(): Received QUIT signal");
              processIsStopped = true;
              break;
            }
            else
            {
              this.UpdateMainTB(tmpRecord);
            }
          }
          catch (Exception ex)
          {
            LogCons.Inst.Write("Minary.DataInput.InputModule.DataProcessingThread(): The following exception occurred: {0}", ex.Message);
          }

          // If activated in the GUI generate a short beep
          // to signalize that a data packet was processed
          if (this.IsBeepOn)
          {
            System.Threading.Tasks.Task.Run(() => { Console.Beep(800, 90); });
          }
        }

        Thread.Sleep(300);
      }

      LogCons.Inst.Write("Minary.DataInput.InputModule.DataProcessingThread(): Exiting thread");
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="newData"></param>
    public void DataInputThread(object data)
    {
      int threadNo = (int)data;
      string tmpLine = string.Empty;

      while (stopThreads == false)
      {
        try
        {
          ClosePipeStream(this.pipeStream[threadNo], this.streamReader[threadNo]);

          this.pipeStream[threadNo] = new NamedPipeServerStream(Config.PipeName, PipeDirection.InOut, Config.PipeInstances);
          this.pipeStream[threadNo].WaitForConnection();
          this.streamReader[threadNo] = new StreamReader(this.pipeStream[threadNo]);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(ex.StackTrace);

          try
          {
            ClosePipeStream(this.pipeStream[threadNo], this.streamReader[threadNo]);
          }
          catch
          {
          }

          this.pipeStream = null;
          this.streamReader = null;
          stopThreads = true;
          break;
        }

        try
        {
          while (this.pipeStream[threadNo] != null &&
                 this.streamReader[threadNo] != null &&
                 this.pipeStream[threadNo].IsConnected)
          {
            try
            {
              if ((tmpLine = this.streamReader[threadNo].ReadLine()) != null && tmpLine.Length > 0)
              {
                if (tmpLine.StartsWith("QUIT"))
                {
                  ClosePipeStream(this.pipeStream[threadNo], this.streamReader[threadNo]);
                  this.pipeStream = null;
                  this.streamReader = null;
                  stopThreads = true;
                  break;
                }

                this.inputDataQueue.Enqueue(tmpLine);
              }
              else
              {
                break;
              }
            }
            catch (ObjectDisposedException odex)
            {
              LogCons.Inst.Write(odex.StackTrace + "\n" + odex.ToString());
              break;
            }
            catch (Exception ex)
            {
              LogCons.Inst.Write(ex.StackTrace + "\n" + ex.ToString());
              break;
            }
          }
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(ex.StackTrace + "\n" + ex.ToString());
        }
      }
    }


    private delegate void UpdateMainTBDelegate(string newData);
    public void UpdateMainTB(string newData)
    {
      string[] splitter;
      int port;

      try
      {
        // TCP||aa:bb:cc:dd:ee:ff||192.168.0.123||51984||74.125.79.136||80||GET ...
        splitter = Regex.Split(newData, @"\|\|");

        if (splitter == null || splitter.Length < 7)
        {
          throw new Exception("Data packet has the wrong format");
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write("InputModules.UpdateMainTB(EXCEPTION): {0}", ex.Message);
        return;
      }

      if (!int.TryParse(splitter[5], out port))
      {
        throw new Exception("Port format is invalid");
      }

      // We got TCP data.
      if (splitter[0] == "TCP")
      {
        foreach (string tmpKey in this.minaryMain.PluginHandler.TabPagesCatalog.Keys)
        {
          if (this.minaryMain.PluginHandler.IsPluginActive(tmpKey) &&
              this.minaryMain.PluginHandler.TabPagesCatalog[tmpKey].PluginObject.Config.Ports.ContainsKey(port))
          {
            try
            {
              this.minaryMain.PluginHandler.TabPagesCatalog[tmpKey].PluginObject.OnNewData(newData);
            }
            catch (Exception ex)
            {
              LogCons.Inst.Write(ex.StackTrace);
            }
          }
        }

        // We got UDP data.
      }
      else if (splitter[0] == "DNSREP" || splitter[0] == "DNSREQ" || splitter[0] == "UDP")
      {
        foreach (string tmpKey in this.minaryMain.PluginHandler.TabPagesCatalog.Keys)
        {
          if (this.minaryMain.PluginHandler.IsPluginActive(tmpKey) &&
              this.minaryMain.PluginHandler.TabPagesCatalog[tmpKey].PluginObject.Config.Ports.ContainsKey(port))
          {
            try
            {
              this.minaryMain.PluginHandler.TabPagesCatalog[tmpKey].PluginObject.OnNewData(newData);
            }
            catch (Exception ex)
            {
              LogCons.Inst.Write(ex.StackTrace);
            }
          }
        }


      // We got data
      }
      else if (splitter[0] == "GENERIC")
      {
        foreach (string key in this.tabPageCatalog.Keys)
        {
          if (this.minaryMain.PluginHandler.IsPluginActive(key) &&
              this.tabPageCatalog[key].PluginObject.Config.Ports.ContainsKey(port))
          {
            try
            {
              this.tabPageCatalog[key].PluginObject.OnNewData(newData);
            }
            catch (Exception ex)
            {
              LogCons.Inst.Write(ex.StackTrace);
            }
          }
        }
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="pipeStream"></param>
    /// <param name="streamReader"></param>
    private static void ClosePipeStream(NamedPipeServerStream pipeStream, StreamReader streamReader)
    {
      if (pipeStream != null)
      {
        pipeStream.Close();
        pipeStream = null;
      }

      if (streamReader != null)
      {
        streamReader.Close();
        streamReader = null;
      }
    }

    #endregion

  }
}
