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


    #region PUBLIC

    public ServiceStatus StartService(ServiceParameters serviceParameters)
    {
      string hostName = "localhost";
      DateTime validityStartDate = DateTime.Now;
      DateTime validityEndDate = validityStartDate.AddYears(10);
      string certificateFileName = "defaultCertificate.pfx";
      string certificateDirectoryName = "Certificates";
      string certificateDirectoryFullPath = Path.Combine(this.workingDirectory, certificateDirectoryName);
      string certificateFileFullPath = Path.Combine(certificateDirectoryFullPath, certificateFileName);
      string certificateRelativePath = Path.Combine(certificateDirectoryName, certificateFileName);

      string httpReverseProxyBinaryPath = Path.Combine(Directory.GetCurrentDirectory(), Config.HttpReverseProxyBinaryPath);
      string processParameters = string.Format("/httpport 80 /httpsport 443 /loglevel info /certificate {0}", certificateRelativePath);

      // If certificate directory does not exist create it
      if (!Directory.Exists(certificateDirectoryFullPath))
      {
        Directory.CreateDirectory(certificateDirectoryFullPath);
      }

      // If certificate file does not exist create it.
      if (!File.Exists(certificateFileFullPath))
      {
        NativeWindowsLib.Crypto.Crypto.CreateNewCertificate(certificateFileFullPath, hostName, validityStartDate, validityEndDate);
      }

      // Abort if HTTP port is already in use by another process.
      if (NetworkFunctions.IsPortAvailable(80) == false)
      {
        throw new Exception("HTTP port is already in use");
      }

      // Abort if HTTPS port is already in use by another process.
      if (NetworkFunctions.IsPortAvailable(443) == false)
      {
        throw new Exception("HTTPS port is already in use");
      }

      // Start process
      this.httpReverseProxyProc = new Process();
      this.httpReverseProxyProc.StartInfo.WorkingDirectory = this.workingDirectory;
      this.httpReverseProxyProc.StartInfo.FileName = httpReverseProxyBinaryPath;
      this.httpReverseProxyProc.StartInfo.Arguments = processParameters;
      this.httpReverseProxyProc.StartInfo.WindowStyle = Debugging.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      this.httpReverseProxyProc.StartInfo.CreateNoWindow = Debugging.IsDebuggingOn ? true : false;
      this.httpReverseProxyProc.EnableRaisingEvents = true;
      this.httpReverseProxyProc.Exited += new EventHandler(this.OnServiceExited);

      LogCons.Inst.Write(LogLevel.Debug, "HttpReverseProxy.StartService(): CommandLine:{0} {1}", httpReverseProxyBinaryPath, processParameters);
      this.serviceStatus = ServiceStatus.Running;
      this.httpReverseProxyProc.Start();

      return ServiceStatus.Running;
    }


    public ServiceStatus StopService()
    {
      if (this.httpReverseProxyProc == null)
      {
        LogCons.Inst.Write(LogLevel.Warning, "HttpReverseProxy.StopService(): Can't stop attack service because it never was started");
        this.serviceStatus = ServiceStatus.NotRunning;
        return ServiceStatus.NotRunning;
      }

      this.httpReverseProxyProc.EnableRaisingEvents = false;
      this.httpReverseProxyProc.Exited += null;
      this.serviceStatus = ServiceStatus.NotRunning;

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
        LogCons.Inst.Write(LogLevel.Error, "HttpReverseProxy.StopService(Exception): {0}", ex.Message);
      }
      finally
      {
        this.httpReverseProxyProc = null;
      }

      return ServiceStatus.NotRunning;
    }

    #endregion

    #endregion


    #region PRIVATE

    private void OnServiceExited(object sender, System.EventArgs e)
    {
      LogCons.Inst.Write(LogLevel.Error, "HttpReverseProxy.OnServiceExited(): Service exited unexpectedly. Exit code {0}", this.httpReverseProxyProc.ExitCode);

      this.httpReverseProxyProc.EnableRaisingEvents = false;
      this.httpReverseProxyProc.Exited += null;
      this.attackServiceHandler.OnServiceExited(this.serviceName);
    }

    #endregion

  }
}
