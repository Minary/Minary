namespace Minary.AttackService
{
  using Minary.AttackService.Service;
  using MinaryLib.AttackService;
  using MinaryLib.Plugin;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using AService = Minary.AttackService.DataTypes.AttackService;


  public class AttackServiceHandler
  {

    #region MEMBERS

    private MinaryMain minaryInstance;
    private Dictionary<string, IAttackService> attackServices;

    #endregion


    #region PROPERTIES

    public MinaryMain MinaryMain { get { return this.minaryInstance; } set { } }

    public Dictionary<string, IAttackService> AttackServices { get { return this.attackServices; } }

    #endregion


    #region PUBLIC

    public AttackServiceHandler(MinaryMain minaryInstance)
    {
      string currentDirectory = Directory.GetCurrentDirectory();
      string apeWorkingDirectory = Path.Combine(currentDirectory, Config.ApeServiceDir);
      string sslStripWorkingDirectory = Path.Combine(currentDirectory, Config.SslStripDir);
      string dataSnifferWorkingDirectory = Path.Combine(currentDirectory, Config.DataSnifferDir);
      string injectPayloadrWorkingDirectory = Path.Combine(currentDirectory, Config.InjectPayloadDir);
      string requestRedirectWorkingDirectory = Path.Combine(currentDirectory, Config.RequestRedirectDir);
      string hostMappingWorkingDirectory = Path.Combine(currentDirectory, Config.HostMappingDir);
      string arpScanWorkingDirectory = Path.Combine(currentDirectory, Config.ArpScanServiceDir);

      this.minaryInstance = minaryInstance;
      this.attackServices = new Dictionary<string, IAttackService>();

      // APE - ARP Poisoning
      Dictionary<string, SubModule> arpPoisoningSubModules = new Dictionary<string, SubModule>();
      arpPoisoningSubModules.Add(AService.ArpPoisoning.SubModule.DnsPoisoning, new SubModule(AService.ArpPoisoning.SubModule.DnsPoisoning, apeWorkingDirectory, Config.DnsPoisoningHosts));
      arpPoisoningSubModules.Add(AService.ArpPoisoning.SubModule.Firewall, new SubModule(AService.ArpPoisoning.SubModule.Firewall, apeWorkingDirectory, Config.ApeFirewallRules));
      ArpPoisoning tmpDataSnifferArpPoison = new ArpPoisoning(this, AService.ArpPoisoning.Name, Path.Combine(Directory.GetCurrentDirectory(), Config.ApeServiceDir), arpPoisoningSubModules);
      this.attackServices.Add(AService.ArpPoisoning.Name, tmpDataSnifferArpPoison);
      this.MinaryMain.RegisterService(AService.ArpPoisoning.Name);
      this.SetNewState(AService.ArpPoisoning.Name, ServiceStatus.NotRunning);

      // APESniffer - Data sniffing
      Dictionary<string, SubModule> dataSniffingSubModules = new Dictionary<string, SubModule>();
      DataSniffer tmpDataSniffer = new DataSniffer(this, AService.DataSniffer.Name, Path.Combine(Directory.GetCurrentDirectory(), Config.ApeSnifferServiceDir), dataSniffingSubModules);
      this.attackServices.Add(AService.DataSniffer.Name, tmpDataSniffer);
      this.MinaryMain.RegisterService(AService.DataSniffer.Name);
      this.SetNewState(AService.DataSniffer.Name, ServiceStatus.NotRunning);

      // HttpReverseProxy
      Dictionary<string, SubModule> httpReverseProxySubModules = new Dictionary<string, SubModule>();
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.SslStrip, new SubModule(AService.HttpReverseProxyServer.SubModule.SslStrip, sslStripWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.DataSniffer, new SubModule(AService.HttpReverseProxyServer.SubModule.DataSniffer, dataSnifferWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.InjectPayload, new SubModule(AService.HttpReverseProxyServer.SubModule.InjectPayload, injectPayloadrWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.RequestRedirect, new SubModule(AService.HttpReverseProxyServer.SubModule.RequestRedirect, requestRedirectWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.HostMapping, new SubModule(AService.HttpReverseProxyServer.SubModule.HostMapping, hostMappingWorkingDirectory, "plugin.config"));
      HttpReverseProxy tmpDataSnifferHttpReverseProxy = new HttpReverseProxy(this, AService.HttpReverseProxyServer.Name, Path.Combine(Directory.GetCurrentDirectory(), Config.HttpReverseProxyServiceDir), httpReverseProxySubModules);
      this.attackServices.Add(AService.HttpReverseProxyServer.Name, tmpDataSnifferHttpReverseProxy);
      this.MinaryMain.RegisterService(AService.HttpReverseProxyServer.Name);
      this.SetNewState(AService.HttpReverseProxyServer.Name, ServiceStatus.NotRunning);

      // ArpScan
      //      Dictionary<string, SubModule> arpScanSubModules = new Dictionary<string, SubModule>();
      //      arpScanSubModules.Add("ArpScan", new SubModule("ArpScan.ArpScan", arpScanWorkingDirectory, string.Empty));
      //// DataSniffer tmpDataSnifferArpScan = new DataSniffer(this, "ArpScan", Path.Combine(Directory.GetCurrentDirectory(), Config.ARPScanServiceDir), arpScanSubModules);
      //      this.attackServices.Add("ArpScan", tmpDataSnifferArpScan);
    }


    public void StartAllServices(ServiceParameters serviceParameters)
    {
      foreach (string tmpKey in this.attackServices.Keys)
      {
        try
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("AttackServiceHandler.StartAllServices(): Starting {0}/{1}", tmpKey, this.attackServices[tmpKey].ServiceName);
          ServiceStatus newServiceStatus = this.attackServices[tmpKey].StartService(serviceParameters);
          this.SetNewState(tmpKey, newServiceStatus);
        }
        catch (Exception ex)
        {
          this.SetNewState(tmpKey, ServiceStatus.Error);
          LogConsole.Main.LogConsole.LogInstance.LogMessage("AttackServiceHandler.StartAllServices(Exception): {0}\r\n{1}\r\n{2}", this.attackServices[tmpKey].ServiceName, ex.Message, ex.StackTrace);
        }
      }
    }


    public void StopAllServices()
    {
      foreach (string tmpKey in this.attackServices.Keys)
      {
        try
        {
          // Stop attack services that have the current state RUNNING.
          if (this.attackServices[tmpKey].Status == ServiceStatus.Running)
          {
            ServiceStatus newServiceStatus = this.attackServices[tmpKey].StopService();
            this.SetNewState(tmpKey, newServiceStatus);
          }
        }
        catch (Exception ex)
        {
          this.SetNewState(tmpKey, ServiceStatus.Error);
          LogConsole.Main.LogConsole.LogInstance.LogMessage("AttackServiceHandler.StopAllServices(Exception): {0}: {1}\r\n{2}", this.attackServices[tmpKey].ServiceName, ex.Message, ex.StackTrace);
        }
      }
    }


    public void ShutDown()
    {
      foreach (string tmpKey in this.attackServices.Keys)
      {
        try
        {
          this.attackServices[tmpKey].StopService();
          this.SetNewState(tmpKey, ServiceStatus.NotRunning);
        }
        catch (Exception ex)
        {
          this.SetNewState(tmpKey, ServiceStatus.Error);
          LogConsole.Main.LogConsole.LogInstance.LogMessage("AttackServiceHandler.ShutDown(Exception): {0}", ex.Message);
        }
      }
    }


    public void OnServiceExited(string serviceName)
    {
      LogConsole.Main.LogConsole.LogInstance.LogMessage("AttackServiceHandler.OnServiceExited(): Service {0} stopped unexpectedly working", serviceName);
      this.SetNewState(serviceName, ServiceStatus.Error);
      this.minaryInstance.OnServiceExicedUnexpectedly(serviceName);
    }

    #endregion


    #region PRIVATE

    private void SetNewState(string serviceName, ServiceStatus newStatus)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return;
      }

      LogConsole.Main.LogConsole.LogInstance.LogMessage("AttackServiceHandler.SetNewState(): {0} has new state {1}", serviceName, newStatus.ToString());
      // Set actual service state
      this.attackServices[serviceName].Status = newStatus;

      // Propagate status switch to GUI
      this.minaryInstance.SetNewAttackServiceState(serviceName, newStatus);
    }

    #endregion
  }
}
