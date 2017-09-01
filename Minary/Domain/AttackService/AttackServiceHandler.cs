namespace Minary.Domain.AttackService
{
  using Minary.DataTypes.Enum;
  using Minary.Domain.AttackService.Service;
  using Minary.Form;
  using Minary.LogConsole.Main;
  using MinaryLib.AttackService;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using AService = DataTypes.AttackService;


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
      string injectCodeWorkingDirectory = Path.Combine(currentDirectory, Config.InjectCodeDir);
      string injectFileWorkingDirectory = Path.Combine(currentDirectory, Config.InjectFileDir);
      string requestRedirectWorkingDirectory = Path.Combine(currentDirectory, Config.RequestRedirectDir);
      string hostMappingWorkingDirectory = Path.Combine(currentDirectory, Config.HostMappingDir);

      this.minaryInstance = minaryInstance;
      this.attackServices = new Dictionary<string, IAttackService>();

      // APE - ARP Poisoning
      Dictionary<string, SubModule> arpPoisoningSubModules = new Dictionary<string, SubModule>();
      arpPoisoningSubModules.Add(AService.ArpPoisoning.SubModule.DnsPoisoning, new SubModule(AService.ArpPoisoning.SubModule.DnsPoisoning, apeWorkingDirectory, Config.DnsPoisoningHosts));
      arpPoisoningSubModules.Add(AService.ArpPoisoning.SubModule.Firewall, new SubModule(AService.ArpPoisoning.SubModule.Firewall, apeWorkingDirectory, Config.ApeFirewallRules));
      ArpPoisoning tmpDataSnifferArpPoison = new ArpPoisoning(this, AService.ArpPoisoning.Name, Path.Combine(Directory.GetCurrentDirectory(), Config.ApeServiceDir), arpPoisoningSubModules);
      this.attackServices.Add(AService.ArpPoisoning.Name, tmpDataSnifferArpPoison);
      this.MinaryMain.RegisterAttackService(AService.ArpPoisoning.Name);
      this.MinaryMain.SetNewAttackServiceState(AService.ArpPoisoning.Name, ServiceStatus.NotRunning);

      // APESniffer - Data sniffing
      Dictionary<string, SubModule> dataSniffingSubModules = new Dictionary<string, SubModule>();
      DataSniffer tmpDataSniffer = new DataSniffer(this, AService.DataSniffer.Name, Path.Combine(Directory.GetCurrentDirectory(), Config.ApeSnifferServiceDir), dataSniffingSubModules);
      this.attackServices.Add(AService.DataSniffer.Name, tmpDataSniffer);
      this.MinaryMain.RegisterAttackService(AService.DataSniffer.Name);
      this.MinaryMain.SetNewAttackServiceState(AService.DataSniffer.Name, ServiceStatus.NotRunning);

      // HttpReverseProxy
      Dictionary<string, SubModule> httpReverseProxySubModules = new Dictionary<string, SubModule>();
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.SslStrip, new SubModule(AService.HttpReverseProxyServer.SubModule.SslStrip, sslStripWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.DataSniffer, new SubModule(AService.HttpReverseProxyServer.SubModule.DataSniffer, dataSnifferWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.InjectCode, new SubModule(AService.HttpReverseProxyServer.SubModule.InjectCode, injectCodeWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.InjectFile, new SubModule(AService.HttpReverseProxyServer.SubModule.InjectFile, injectFileWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.RequestRedirect, new SubModule(AService.HttpReverseProxyServer.SubModule.RequestRedirect, requestRedirectWorkingDirectory, "plugin.config"));
      httpReverseProxySubModules.Add(AService.HttpReverseProxyServer.SubModule.HostMapping, new SubModule(AService.HttpReverseProxyServer.SubModule.HostMapping, hostMappingWorkingDirectory, "plugin.config"));
      HttpReverseProxy tmpDataSnifferHttpReverseProxy = new HttpReverseProxy(this, AService.HttpReverseProxyServer.Name, Path.Combine(Directory.GetCurrentDirectory(), Config.HttpReverseProxyServiceDir), httpReverseProxySubModules);
      this.attackServices.Add(AService.HttpReverseProxyServer.Name, tmpDataSnifferHttpReverseProxy);
      this.MinaryMain.RegisterAttackService(AService.HttpReverseProxyServer.Name);
      this.MinaryMain.SetNewAttackServiceState(AService.HttpReverseProxyServer.Name, ServiceStatus.NotRunning);
    }



    //public void StartAllServices(ServiceParameters serviceParameters)
    //{
    //  foreach (string tmpKey in this.attackServices.Keys)
    //  {
    //    try
    //    {
    //      LogCons.Inst.Write("AttackServiceHandler.StartAllServices(): Starting {0}/{1}", tmpKey, this.attackServices[tmpKey].ServiceName);
    //      ServiceStatus newServiceStatus = this.attackServices[tmpKey].StartService(serviceParameters);
    //      this.SetNewState(tmpKey, newServiceStatus);
    //    }
    //    catch (Exception)
    //    {
    //      this.SetNewState(tmpKey, ServiceStatus.Error);
    //      throw;
    //    }
    //  }
    //}


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
            this.MinaryMain.SetNewAttackServiceState(tmpKey, newServiceStatus);
          }
        }
        catch (Exception ex)
        {
          this.MinaryMain.SetNewAttackServiceState(tmpKey, ServiceStatus.Error);
          LogCons.Inst.Write(LogLevel.Error, "AttackServiceHandler.StopAllServices(Exception): {0}: {1}\r\n{2}", this.attackServices[tmpKey].ServiceName, ex.Message, ex.StackTrace);
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
          this.MinaryMain.SetNewAttackServiceState(tmpKey, ServiceStatus.NotRunning);
        }
        catch (Exception ex)
        {
          this.MinaryMain.SetNewAttackServiceState(tmpKey, ServiceStatus.Error);
          LogCons.Inst.Write(LogLevel.Error, "AttackServiceHandler.ShutDown(Exception): {0}", ex.Message);
        }
      }
    }


    public void OnServiceExited(string serviceName)
    {
      LogCons.Inst.Write(LogLevel.Error, "AttackServiceHandler.OnServiceExited(): Service {0} stopped unexpectedly", serviceName);
      this.MinaryMain.SetNewAttackServiceState(serviceName, ServiceStatus.Error);
      this.minaryInstance.OnServiceExicedUnexpectedly(serviceName);
    }

    #endregion


    #region PRIVATE

    /*
    private void SetNewState(string serviceName, ServiceStatus newStatus)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return;
      }

      if (this.attackServices.ContainsKey(serviceName) == false)
      {
        LogCons.Inst.Write("AttackServiceHandler.SetNewState(): Attack service \"{0}\" was never registered", serviceName);
        return;
      }

      if (this.attackServices[serviceName].Status == newStatus)
      {
        return;
      }

      LogCons.Inst.Write("AttackServiceHandler.SetNewState(): {0} has new state \"{1}\"", serviceName, newStatus.ToString());
      // Set actual service state
      this.attackServices[serviceName].Status = newStatus;

      // Propagate status switch to GUI
      // TODO: Should be solved by an observer!
      this.minaryInstance.SetNewAttackServiceState(serviceName, newStatus);
    }
    */

    #endregion
  }
}
