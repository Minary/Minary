namespace Minary.AttackService.Service
{
  using Minary.ArpScan.DataTypes;
  using Minary.Common;
  using MinaryLib.AttackService;
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;

  public class ArpPoisoning : IAttackService
  {

    #region MEMBERS

    private readonly string serviceName;
    private readonly string workingDirectory;
    private readonly Dictionary<string, SubModule> subModules;
    private ServiceStatus serviceStatus;

    private Process poisoningEngProc;
    private AttackServiceHandler attackServiceHandler;

    #endregion


    #region PUBLIC

    public ArpPoisoning(AttackServiceHandler attackServiceHandler, string serviceName, string serviceWorkingDir, Dictionary<string, SubModule> subModules)
    {
      this.attackServiceHandler = attackServiceHandler;
      this.serviceName = serviceName;
      this.workingDirectory = serviceWorkingDir;
      this.subModules = subModules;
      this.serviceStatus = ServiceStatus.NotRunning;
    }

    #endregion


    #region INTERFACE IAttackService implementation

    #region PROPERTIES

    public string ServiceName { get { return this.serviceName; } set { } }

    public string WorkingDirectory { get { return this.workingDirectory; } set { } }

    public Dictionary<string, SubModule> SubModules { get { return this.subModules; } set { } }

    public ServiceStatus Status { get { return this.serviceStatus; } set { this.serviceStatus = value; } }

    #endregion


    #region PUBLIC

    public ServiceStatus StartService(ServiceParameters serviceParameters)
    {
      string targetHosts = string.Empty;
      ArpScan.Presentation.ArpScan arpScan;
      string workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), Config.ApeServiceDir);
      string timeStamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
      string targetHostsPath = Config.APETargetHostsPath;
      string processParameters = string.Format("-x {0}", serviceParameters.SelectedIfcId);

      if ((arpScan = ArpScan.Presentation.ArpScan.GetInstance()) == null)
      {
        return ServiceStatus.NotRunning;
      }

      if (arpScan.TargetList.Count <= 0)
      {
        this.serviceStatus = ServiceStatus.NotRunning;
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpPoisoning.StartService(): There were no systems selected to attack");
        return ServiceStatus.NotRunning;
      }

      // Write APE targetSystem hosts to list
      foreach (TargetRecord targetRecord in arpScan.TargetList)
      {
        if (targetRecord.Status)
        {
          targetHosts += string.Format("{0},{1}\r\n", targetRecord.IpAddress, targetRecord.MacAddress);
          LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpPoisoning.StartService(): Poisoning targetSystem system: {0}/{1}", targetRecord.MacAddress, targetRecord.IpAddress);
        }
      }

      using (StreamWriter outputFile = new StreamWriter(targetHostsPath))
      {
        targetHosts = targetHosts.Trim();
        outputFile.Write(targetHosts);
      }

      // Start process
      string apeBinaryPath = Path.Combine(Directory.GetCurrentDirectory(), Config.ApeBinaryPath);

      this.poisoningEngProc = new Process();
      this.poisoningEngProc.StartInfo.WorkingDirectory = workingDirectory;
      this.poisoningEngProc.StartInfo.FileName = apeBinaryPath;
      this.poisoningEngProc.StartInfo.Arguments = processParameters;
      this.poisoningEngProc.StartInfo.WindowStyle = Debugging.IsDebuggingOn() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      this.poisoningEngProc.StartInfo.CreateNoWindow = Debugging.IsDebuggingOn() ? true : false;
      this.poisoningEngProc.EnableRaisingEvents = true;
      this.poisoningEngProc.Exited += new EventHandler(this.OnServiceExited);
      this.serviceStatus = ServiceStatus.Running;

      LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpPoisoning.StartService(): CommandLine:{0} {1}", apeBinaryPath, processParameters);
      this.poisoningEngProc.Start();

      return ServiceStatus.Running;
    }


    public ServiceStatus StopService()
    {
      if (this.poisoningEngProc == null)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpPoisoning.StopService(): Can't stop attack service because it never was started");
        this.serviceStatus = ServiceStatus.NotRunning;
        return ServiceStatus.NotRunning;
      }

      this.poisoningEngProc.EnableRaisingEvents = false;
      this.poisoningEngProc.Exited += null;
      this.serviceStatus = ServiceStatus.NotRunning;
      LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpPoisoning.StopService(): EnableRaisingEvents:{0}", this.poisoningEngProc.EnableRaisingEvents);

      try
      {
        if (this.poisoningEngProc != null && !this.poisoningEngProc.HasExited)
        {
          this.poisoningEngProc.Kill();
          this.poisoningEngProc = null;
        }
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpPoisoning.StopService(Exception): {0}", ex.Message);
      }

      return ServiceStatus.NotRunning;
    }

    #endregion

    #endregion


    #region PRIVATE

    private void OnServiceExited(object sender, System.EventArgs e)
    {
      LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpPoisoning.OnServiceExited(): Service exited unexpectedly. Exit code {0}", this.poisoningEngProc.ExitCode);

      this.poisoningEngProc.EnableRaisingEvents = false;
      this.poisoningEngProc.Exited += null;
      this.attackServiceHandler.OnServiceExited(this.serviceName);
    }

    #endregion

  }
}
