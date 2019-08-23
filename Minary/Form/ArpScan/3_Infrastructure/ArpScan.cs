namespace Minary.Form.ArpScan.Infrastructure
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Text.RegularExpressions;
  using System.Threading;
  using System.Xml.Linq;


  public class ArpScan
  {

    #region MEMBERS

private string arpScanProcName = "ArpScan";
    private Process arpScanProc;
    private string baseDir;
    private string arpScanBin;
    private string data = string.Empty;
    private ArpScanConfig arpScanConf;

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
      this.arpScanProc.StartInfo.Arguments = string.Format("{0} {1} {2} -x -v", this.arpScanConf.InterfaceId, this.arpScanConf.NetworkStartIp, this.arpScanConf.NetworkStopIp);
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
        try
        {
          proc.Kill();
        }
        catch (Exception)
        {
        }
      }
    }

    #endregion


    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnArpScanExited(object sender, EventArgs e)
    {
      if (this.arpScanConf.OnArpScanStopped != null)
      {
        this.arpScanConf.OnArpScanStopped();
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDataRecived(object sender, DataReceivedEventArgs e)
    {
      var type = string.Empty;
      var ipAddress = string.Empty;
      var macAddress = string.Empty;
      var vendor = string.Empty;
      var note = string.Empty;

      if (string.IsNullOrEmpty(e.Data))
      {
        return;
      }

      if (e.Data != "</arp>")
      {
        this.data += e.Data.Trim();
      }
      else if (e.Data == "</arp>")
      {
        this.data += e.Data.Trim();
        if (Regex.Match(this.data, @"(<arp>.*?</arp>)", RegexOptions.IgnoreCase).Success == false)
        {
          this.data = string.Empty;
          return;
        }
        
        try
        {
          var systemData = this.ParseSystemDataFromXml(this.data);

          if (systemData.Type == "request")
          {
            this.arpScanConf.OnRequestSent(systemData.IpAddress);
          }
          else if (systemData.Type == "reply")
          {
            this.arpScanConf.OnReplyDataReceived(systemData);
          }
        }
        catch
        {
        }

        this.data = string.Empty;
      }
    }


    private SystemFound ParseSystemDataFromXml(string data)
    {
        // Extract newly detected system
        XDocument xmlContent = XDocument.Parse(data);
        var packetEntries = from service in xmlContent.Descendants("arp")
                            select new
                            {
                              Type = service.Element("type").Value,
                              IpAddress = service.Element("ip").Value,
                              MacAddress = service.Element("mac").Value
                            };

        if (packetEntries == null)
        {
          throw new Exception("Could not parse data from XML structure.");
        }
        else if (packetEntries.Count() <= 0)
        {
          throw new Exception("Could not create an object from the XML structure");
        }
        else if(packetEntries.Count() > 1)
        {
          throw new Exception("More than one object was generated from the XML structure");
        }

        var tmpType = packetEntries.First();
        var newSystem = new SystemFound(tmpType.MacAddress, tmpType.IpAddress, tmpType.Type.ToLower().Trim());

        return newSystem;
    }

    #endregion

  }
}
