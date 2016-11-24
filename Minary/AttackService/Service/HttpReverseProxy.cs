namespace Minary.AttackService.Service
{
  using Minary.Common;
  using MinaryLib.AttackService;
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;

  public class HttpReverseProxy : IAttackService
  {

    #region MEMBERS

    private readonly string serviceName;
    private readonly string workingDirectory;
    private readonly Dictionary<string, SubModule> subModules;
    private ServiceStatus serviceStatus;

    private Process httpReverseProxyProc;
    private AttackServiceHandler attackServiceHandler;

    #endregion


    #region PUBLIC

    public HttpReverseProxy(AttackServiceHandler attackServiceHandler, string serviceName, string serviceWorkingDir, Dictionary<string, SubModule> subModules)
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


    #region

    public ServiceStatus StartService(ServiceParameters serviceParameters)
    {
      string timeStamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
      Config.NetworkInterface ifcSelected = Config.GetIfcByID(serviceParameters.SelectedIfcId);
      string httpReverseProxyBinaryPath = Path.Combine(Directory.GetCurrentDirectory(), Config.HttpReverseProxyBinaryPath);
      string processParameters = "80 /d";

      // Start process
      this.httpReverseProxyProc = new Process();
      this.httpReverseProxyProc.StartInfo.WorkingDirectory = this.workingDirectory;
      this.httpReverseProxyProc.StartInfo.FileName = httpReverseProxyBinaryPath;
      this.httpReverseProxyProc.StartInfo.Arguments = processParameters;
      this.httpReverseProxyProc.StartInfo.WindowStyle = Debugging.IsDebuggingOn() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      this.httpReverseProxyProc.StartInfo.CreateNoWindow = Debugging.IsDebuggingOn() ? true : false;
      this.httpReverseProxyProc.EnableRaisingEvents = true;
      this.httpReverseProxyProc.Exited += new EventHandler(this.OnServiceExited);

      LogConsole.Main.LogConsole.LogInstance.LogMessage("HttpReverseProxy.StartService(): CommandLine:{0} {1}", httpReverseProxyBinaryPath, processParameters);
      this.serviceStatus = ServiceStatus.Running;
      this.httpReverseProxyProc.Start();

      return ServiceStatus.Running;
    }


    public ServiceStatus StopService()
    {
      if (this.httpReverseProxyProc == null)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("HttpReverseProxy.StopService(): Can't stop attack service because it never was started");
        this.serviceStatus = ServiceStatus.NotRunning;
        return ServiceStatus.NotRunning;
      }

      this.httpReverseProxyProc.EnableRaisingEvents = false;
      this.httpReverseProxyProc.Exited += null;
      this.serviceStatus = ServiceStatus.NotRunning;
      LogConsole.Main.LogConsole.LogInstance.LogMessage("HttpReverseProxy.StopService(): EnableRaisingEvents:{0}", this.httpReverseProxyProc.EnableRaisingEvents);

      try
      {
        if (this.httpReverseProxyProc != null && !this.httpReverseProxyProc.HasExited)
        {
          this.httpReverseProxyProc.Kill();
          this.httpReverseProxyProc = null;
        }
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("HttpReverseProxy.StopService(Exception): {0}", ex.Message);
      }

      return ServiceStatus.NotRunning;
    }

    #endregion

    #endregion


    #region PRIVATE

    private void OnServiceExited(object sender, System.EventArgs e)
    {
      LogConsole.Main.LogConsole.LogInstance.LogMessage("HttpReverseProxy.OnServiceExited(): Service exited unexpectedly. Exit code {0}", this.httpReverseProxyProc.ExitCode);

      this.httpReverseProxyProc.EnableRaisingEvents = false;
      this.httpReverseProxyProc.Exited += null;
      this.attackServiceHandler.OnServiceExited(this.serviceName);
    }

    #endregion

  }
}
