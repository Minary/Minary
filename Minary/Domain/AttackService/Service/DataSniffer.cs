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


  public class DataSniffer : IAttackService
  {

    #region MEMBERS

    private readonly string serviceName;
    private readonly string workingDirectory;
    private readonly Dictionary<string, SubModule> subModules;
    private ServiceStatus serviceStatus;
    private Process snifferProc;
    private AttackServiceHandler attackServiceHandler;

    #endregion


    #region PUBLIC

    public DataSniffer(AttackServiceHandler attackServiceHandler, string serviceName, string serviceWorkingDir, Dictionary<string, SubModule> subModules)
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
      string apeSnifferBinaryPath = Path.Combine(Directory.GetCurrentDirectory(), Config.ApeSnifferBinaryPath);
      string workingDirectory = Path.Combine(Directory.GetCurrentDirectory(), Config.ApeSnifferServiceDir);
      string processParameters = string.Format("-s {0} -p {1}", serviceParameters.SelectedIfcId, Config.PipeName);

      this.snifferProc = new Process();
      this.snifferProc.StartInfo.FileName = apeSnifferBinaryPath;
      this.snifferProc.StartInfo.Arguments = processParameters;
      this.snifferProc.StartInfo.WorkingDirectory = workingDirectory;
      this.snifferProc.StartInfo.WindowStyle = Debugging.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      this.snifferProc.StartInfo.CreateNoWindow = Debugging.IsDebuggingOn ? true : false;
      this.snifferProc.EnableRaisingEvents = true;
      this.snifferProc.Exited += new EventHandler(this.OnServiceExited);

      LogCons.Inst.Write(LogLevel.Debug, "DataSniffer.StartService(): CommandLine:{0} {1}", apeSnifferBinaryPath, processParameters);
      this.serviceStatus = ServiceStatus.Running;
      this.snifferProc.Start();

      return ServiceStatus.Running;
    }


    public ServiceStatus StopService()
    {
      if (this.snifferProc == null)
      {
        LogCons.Inst.Write(LogLevel.Warning, "DataSniffer.StopService(): Can't stop attack service because it never was started");
        this.serviceStatus = ServiceStatus.NotRunning;
        return ServiceStatus.NotRunning;
      }

      this.snifferProc.EnableRaisingEvents = false;
      this.snifferProc.Exited += null;
      this.serviceStatus = ServiceStatus.NotRunning;
      
      try
      {
        if (this.snifferProc != null && !this.snifferProc.HasExited)
        {
          this.snifferProc.Kill();
          this.snifferProc = null;
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "DataSniffer.StopService(Exception): {0}", ex.Message);
      }

      return ServiceStatus.NotRunning;
    }

    #endregion

    #endregion


    #region PRIVATE

    private void OnServiceExited(object sender, System.EventArgs e)
    {
      LogCons.Inst.Write(LogLevel.Error, "DataSniffer.OnServiceExited(): Service exited unexpectedly. Exit code {0}", this.snifferProc.ExitCode);

      this.snifferProc.EnableRaisingEvents = false;
      this.snifferProc.Exited += null;
      this.attackServiceHandler.OnServiceExited(this.serviceName);

      
    }

    #endregion

  }
}