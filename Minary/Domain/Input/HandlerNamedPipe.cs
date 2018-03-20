namespace Minary.Domain.InputProcessor
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Form;
  using Minary.LogConsole.Main;
  using System;
  using System.Collections.Concurrent;
  using System.IO;
  using System.IO.Pipes;
  using System.Text.RegularExpressions;
  using System.Threading;


  public class HandlerNamedPipe : IInputProcessor
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
 
 
    #region INTERFACE: IInputProcessor
    
    #region PROPERTIES

    public bool IsBeepOn { get; set; }

    #endregion


    #region PUBLIC METHODS

    public HandlerNamedPipe(MinaryMain minaryMain)
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
    public void StartInputProcessing()
    {
      var failedOpenPipes = 0;
      stopThreads = false;
      
        // There are several concurrently running NamedPipes reading
        // input data. Start them all.
        for (var i = 0; i < Config.PipeInstances; i++)
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
            failedOpenPipes++;
            var errorMessage = $"Can't start named pipe no {i}.\r\n{ex.Message}";
            throw new Exception(errorMessage);
          }

          this.inputWorkerThreads[i] = new Thread(new ParameterizedThreadStart(this.DataInputThread));
          this.inputWorkerThreads[i].Start(i);
        }

        if (failedOpenPipes > 0)
        {
          var message = $"{failedOpenPipes} of {Config.PipeInstances} named pipes could not be started";
          throw new Exception(message);
        }
    }


    public void StopInputProcessing()
    {
      NamedPipeClientStream namedPipeClient = null;
      StreamWriter streamWriter = null;

      stopThreads = true;

      if (this.pipeStream == null)
      {
        return;
      }

      for (var i = 0; i < Config.PipeInstances; i++)
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
        }
        catch (TimeoutException tex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"{tex.StackTrace}\n{tex.ToString()}");
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"An error occurred while starting the sniffer : {ex.StackTrace}\n{ex.ToString()}");
        }
        finally
        {
          if (streamWriter != null)
          {
            Minary.Common.Utils.TryExecute2(streamWriter.Close);
          }

          if (namedPipeClient != null)
          {
            Minary.Common.Utils.TryExecute2(namedPipeClient.Close);
            namedPipeClient = null;
          }
        }
      }

      for (var i = 0; i < Config.PipeInstances; i++)
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

    #endregion

    #endregion
 

    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    private void DataProcessingThread()
    {
      var tmpRecord = string.Empty;
      var processIsStopped = false;

      while (processIsStopped == false)
      {
        // Process all records in queue
        while (this.inputDataQueue.IsEmpty == false)
        {
          if (this.inputDataQueue.TryDequeue(out tmpRecord) == false)
          {
            continue;
          }

          try
          {
            if (tmpRecord.StartsWith("QUIT"))
            {
              LogCons.Inst.Write(LogLevel.Info, "Minary.DataInput.InputModule.DataProcessingThread(): Received QUIT signal");
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
            LogCons.Inst.Write(LogLevel.Error, $"Minary.DataInput.InputModule.DataProcessingThread(): The following exception occurred: {ex.Message}");
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

      LogCons.Inst.Write(LogLevel.Info, "Minary.DataInput.InputModule.DataProcessingThread(): Exiting thread");
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="newData"></param>
    private void DataInputThread(object data)
    {
      var threadNo = (int)data;
      var tmpLine = string.Empty;

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
          LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);

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
              LogCons.Inst.Write(LogLevel.Error, $"{odex.StackTrace}\n{odex.ToString()}");
              break;
            }
            catch (Exception ex)
            {
              LogCons.Inst.Write(LogLevel.Error, $"{ex.StackTrace}\n{ex.ToString()}");
              break;
            }
          }
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"{ex.StackTrace}\n{ex.ToString()}");
        }
      }
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newData"></param>
    private void UpdateMainTB(string newData)
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
        LogCons.Inst.Write(LogLevel.Error, $"InputModules.UpdateMainTB(EXCEPTION): {ex.Message}");
        return;
      }

      if (!int.TryParse(splitter[5], out port))
      {
        throw new Exception("Port format is invalid");
      }
      
      // We got TCP data.
      if (splitter[0] == "TCP" ||
          splitter[0] == "HTTPS")
      {
        foreach (var tmpKey in this.minaryMain.PluginHandler.TabPagesCatalog.Keys)
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
              LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
            }
          }
        }

      // We got UDP data.
      }
      else if (splitter[0] == "DNSREP" || 
               splitter[0] == "DNSREQ" || 
               splitter[0] == "UDP")
      {
        foreach (var tmpKey in this.minaryMain.PluginHandler.TabPagesCatalog.Keys)
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
              LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
            }
          }
        }

      // We got data
      }
      else if (splitter[0] == "GENERIC")
      {
        foreach (var key in this.tabPageCatalog.Keys)
        {
          //          port IST 09
          if (this.minaryMain.PluginHandler.IsPluginActive(key) &&
              this.tabPageCatalog[key].PluginObject.Config.Ports.ContainsKey(port))
          {
            try
            {
              this.tabPageCatalog[key].PluginObject.OnNewData(newData);
            }
            catch (Exception ex)
            {
              LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
            }
          }
        }
      }
    }


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
