namespace Minary.Form.ArpScan.Infrastructure
{
  using Minary.Form.ArpScan.DataTypes;
  using Minary.Form.ArpScan.DataTypes.Interface;
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;
  using System.Text.RegularExpressions;
  using System.Threading;


  public class ArpScan : IObservable
  {

    #region MEMBERS

    private string arpScanProcName = "ArpScan";
    private Process arpScanProc;
    private string baseDir;
    private string arpScanBin;
    private string data = string.Empty;
    private ArpScanConfig arpScanConf;
    private List<IObserver> observers;

    #endregion


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="ArpScan"/> class.
    ///
    /// </summary>
    public ArpScan()
    {
      this.baseDir = Directory.GetCurrentDirectory();
      this.arpScanBin = Path.Combine(this.baseDir, Minary.Config.ArpScanBinaryPath);
      this.observers = new List<IObserver>();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="arpConfig"></param>
    public void StartArpScan(ArpScanConfig arpConfig)
    {
      this.arpScanConf = arpConfig;

      if (!File.Exists(this.arpScanBin))
      {
        throw new Exception("ArpScan binary not found");
      }

      this.arpScanProc = new Process();
      this.arpScanProc.StartInfo.FileName = this.arpScanBin;
      this.arpScanProc.StartInfo.Arguments = string.Format("-s {0} {1} {2}", this.arpScanConf.InterfaceId, this.arpScanConf.NetworkStartIp, this.arpScanConf.NetworkStopIp);
      this.arpScanProc.StartInfo.UseShellExecute = false;
      this.arpScanProc.StartInfo.CreateNoWindow = this.arpScanConf.IsDebuggingOn ? false : true;
      this.arpScanProc.StartInfo.WindowStyle = this.arpScanConf.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;

      // set up output redirection
      this.arpScanProc.StartInfo.RedirectStandardOutput = true;
      this.arpScanProc.EnableRaisingEvents = true;

      // Set the data received handlers
      this.arpScanProc.OutputDataReceived += this.OnDataRecived;

      // Configure the process exited event
      this.arpScanProc.Exited += this.OnArpScanExited;
      this.arpScanProc.Disposed += this.OnArpScanExited;

      this.arpScanProc.Start();
      // arpScanProc.BeginErrorReadLine();
      this.arpScanProc.BeginOutputReadLine();

      Thread.Sleep(100);
    }


    /// <summary>
    ///
    /// </summary>
    public void StopArpScan()
    {
      if (this.arpScanProc != null && this.arpScanProc.HasExited == false && this.arpScanProc.Responding)
      {
        this.arpScanProc.Kill();
      }
    }


    /// <summary>
    ///
    /// </summary>
    public void KillAllRunningArpScans()
    {
      Process[] minaryInstances;
      if ((minaryInstances = Process.GetProcessesByName(this.arpScanProcName)) != null && minaryInstances.Length <= 0)
      {
        return;
      }

      foreach (Process proc in minaryInstances)
      {
        Common.Utils.TryExecute2(proc.Kill);
      }
    }

    #endregion


    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnArpScanExited(object sender, System.EventArgs e)
    {
      this.arpScanConf.ObserverClass.ProcessStopped();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDataRecived(object sender, DataReceivedEventArgs e)
    {
      if (e.Data == null || e.Data.Length <= 0)
      {
        return;
      }

      if (e.Data != "<EOF>")
      {
        this.data += e.Data + "\n";
      }
      else if (e.Data == "<EOF>")
      {
        Match trafficMatch = Regex.Match(this.data, @"(<arp>.*?</arp>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        if (trafficMatch.Success)
        {
          this.arpScanConf.ObserverClass.AddRecordToDgv(this.data);
          this.data = string.Empty;
        }
      }
    }

    #endregion


    #region INTERFACE: IObservable

    public void AddObserver(IObserver observer)
    {
      this.observers.Add(observer);
    }


    public void NotifyProgressBar(int progress)
    {
      foreach (IObserver observer in this.observers)
      {
        observer.UpdateProgressbar(progress);
      }
    }


    public void AddRecordToDgv(string inputData)
    {
      foreach (IObserver observer in this.observers)
      {
        observer.AddRecordToDgv(inputData);
      }
    }


    public void ProcessStopped()
    {
      foreach (IObserver observer in this.observers)
      {
        observer.ProcessStopped();
      }
    }

    #endregion

  }
}
