namespace Minary.LogConsole.Task
{
  using Minary.DataTypes.Interface.LogConsole;
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Threading;


  public class LogConsole : IObservable
  {

    #region MEMBERS

    private static bool stopThreads = false;
    private Thread processLogDataThread;
    private ConcurrentQueue<string> logDataQueue = new ConcurrentQueue<string>();
    private List<IObserver> observers = new List<IObserver>();

    #endregion


    #region PUBLIC

    public LogConsole()
    {
      this.processLogDataThread = new Thread(this.LogDataProcessingThread);
      this.processLogDataThread.Start();
    }


    public void AddLogMessage(string logMessage)
    {
      var timeStamp = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
      var realLogMessage = $"{timeStamp} - {logMessage}";

      this.logDataQueue.Enqueue(realLogMessage);
    }

    #endregion


    #region PRIVATE

    public void LogDataProcessingThread()
    {
      var tmpDataQueue = new List<string>();
      var tmpRecord = string.Empty;

      while (stopThreads == false)
      {
        if (this.logDataQueue.IsEmpty == true)
        {
          Thread.Sleep(300);
          continue;
        }

        tmpDataQueue.Clear();

        // Process all records in queue
        while (!this.logDataQueue.IsEmpty && tmpDataQueue.Count <= 3)
        {
          tmpRecord = string.Empty;
          if (this.logDataQueue.TryDequeue(out tmpRecord))
          {
            tmpDataQueue.Add(tmpRecord);
          }
        }

        this.Notify(tmpDataQueue);
      }
    }

    #endregion


    #region INTERFACE: IObservable

    public void AddObserver(IObserver observer)
    {
      if (observer != null)
      {
        this.observers.Add(observer);
      }
    }


    public void Notify(List<string> newLogRecords)
    {
      foreach (var observer in this.observers)
      {
        observer.UpdateLog(newLogRecords);
      }
    }

    #endregion

  }
}
