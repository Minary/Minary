namespace Minary.Domain.AttackService.Service
{
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;
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


    #region PRIVATE

    private void OnServiceExited(object sender, System.EventArgs e)
    {
      int exitCode = -99999;

      try
      {
        exitCode = this.poisoningEngProc.ExitCode;
      }
      catch (Exception ex)
      {
        exitCode = -99999;
      }

      LogCons.Inst.Write(LogLevel.Warning, "ArpPoisoning.OnServiceExited(): Service exited unexpectedly. Exit code {0}", exitCode);

      this.poisoningEngProc.EnableRaisingEvents = false;
      this.poisoningEngProc.Exited += null;
      this.attackServiceHandler.OnServiceExited(this.serviceName);
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
      string workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), Config.ApeServiceDir);
      string timeStamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
      string targetHostsPath = Config.APETargetHostsPath;
      string processParameters = string.Format("-x {0}", serviceParameters.SelectedIfcId);

      if (serviceParameters.TargetList.Count <= 0)
      {
        this.serviceStatus = ServiceStatus.NotRunning;
        LogCons.Inst.Write(LogLevel.Warning, "ArpPoisoning.StartService(): No target system selected");
        return ServiceStatus.NotRunning;
      }

      // Write APE targetSystem hosts to list
      foreach (string tmpTargetMac in serviceParameters.TargetList.Keys)
      {
        targetHosts += string.Format("{0},{1}\r\n", serviceParameters.TargetList[tmpTargetMac], tmpTargetMac);
        LogCons.Inst.Write(LogLevel.Debug, "ArpPoisoning.StartService(): Poisoning targetSystem system: {0}/{1}", tmpTargetMac, serviceParameters.TargetList[tmpTargetMac]);
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
      this.poisoningEngProc.StartInfo.WindowStyle = Debugging.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      this.poisoningEngProc.StartInfo.CreateNoWindow = Debugging.IsDebuggingOn ? true : false;
      this.poisoningEngProc.EnableRaisingEvents = true;
      this.poisoningEngProc.Exited += new EventHandler(this.OnServiceExited);
      this.serviceStatus = ServiceStatus.Running;

      LogCons.Inst.Write(LogLevel.Debug, "ArpPoisoning.StartService(): CommandLine:{0} {1}", apeBinaryPath, processParameters);
      this.poisoningEngProc.Start();

      return ServiceStatus.Running;
    }


    public ServiceStatus StopService()
    {
      if (this.poisoningEngProc == null)
      {
        LogCons.Inst.Write(LogLevel.Warning, "ArpPoisoning.StopService(): Can't stop attack service because it never was started");
        this.serviceStatus = ServiceStatus.NotRunning;
        return ServiceStatus.NotRunning;
      }

      this.poisoningEngProc.EnableRaisingEvents = false;
      this.poisoningEngProc.Exited += null;
      this.serviceStatus = ServiceStatus.NotRunning;
      LogCons.Inst.Write(LogLevel.Info, "ArpPoisoning.StopService(): EnableRaisingEvents:{0}", this.poisoningEngProc.EnableRaisingEvents);

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
        LogCons.Inst.Write(LogLevel.Error, "ArpPoisoning.StopService(Exception): {0}", ex.Message);
      }

      return ServiceStatus.NotRunning;
    }

    #endregion

    #endregion

  }
}
