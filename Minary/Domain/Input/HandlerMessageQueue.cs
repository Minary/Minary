namespace Minary.Domain.InputProcessor
{
  using Minary.Form;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.LogConsole.Main;
  using System;
  using System.Collections.Concurrent;
  using System.Messaging;
  using System.Threading;


  public class HandlerMessageQueue : IInputProcessor
  {

    #region MEMBERS

    private const string _LOCAL_ENDPOINT = @".\Private$\MinaryMQ";
    private static bool stopThreads = false;
    private MinaryMain minaryMain;
    private MessageQueue minaryQueue;
    private MessageEnumerator minaryQueueEnumerator;
    private ConcurrentDictionary<string, DataTypes.MinaryExtension> tabPageCatalog;
    private Thread processInputDataThread;

    #endregion


    #region INTERFACE: IInputProcessor


    #region PROPERTIES

    public bool IsBeepOn { get; set; }

    #endregion


    #region PUBLIC METHODS

    public HandlerMessageQueue(MinaryMain minaryMain)
    {
      this.minaryMain = minaryMain;
      this.IsBeepOn = false;
      this.tabPageCatalog = this.minaryMain.PluginHandler.TabPagesCatalog;
    }


    /// <summary>
    ///
    /// </summary>
    public void StartInputProcessing()
    {
      SystemComponents.MSMQHandler mqHandler = new SystemComponents.MSMQHandler();

      LogCons.Inst.Write(LogLevel.Fatal, "MSMQ is {0}installed", mqHandler.IsMSMQInstalled() == true ? string.Empty:"NOT ");
      LogCons.Inst.Write(LogLevel.Fatal, "MSMQ is {0}running", mqHandler.IsMSMQRunning() == true ? string.Empty : "NOT ");

      if (mqHandler.IsMSMQInstalled() == false)
      {
        mqHandler.Install();
      }

      stopThreads = false;

      try
      {
        this.minaryQueue = new MessageQueue(_LOCAL_ENDPOINT);
        this.minaryQueueEnumerator = this.minaryQueue.GetMessageEnumerator2();
        this.processInputDataThread = new Thread(this.DataProcessingThread);
        this.processInputDataThread.Start();
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Fatal, "An error occurred while starting the input processor MessageQueue : {0}\r\nStacktrace: {1}", ex.Message, ex.StackTrace);
        string message = string.Format("An error occurred while starting the input processor MessageQueue : {0}\r\n\r\nStacktrace: {1}", ex.Message, ex.StackTrace);
        MessageDialog.Inst.ShowError(string.Empty, message, this.minaryMain);
      }
    }


    /// <summary>
    ///
    /// </summary>
    public void StopInputProcessing()
    {
      if (this.processInputDataThread == null ||
          this.processInputDataThread.IsAlive == false)
      {
        return;
      }

      if (this.minaryQueue == null)
      {
        return;
      }

      this.minaryQueue.Close();
    }

    #endregion

    #endregion


    #region PRIVATE

    public void DataProcessingThread()
    {
      while (this.minaryQueueEnumerator.MoveNext())
      {
        Message msg = (Message)this.minaryQueueEnumerator.Current;
        //msg.Body
        LogCons.Inst.Write(LogLevel.Debug, "Message({0}): {1}", msg.Id, msg.Label);
      }
    }

    #endregion

  }
}
