namespace Minary.Domain.AttackService
{
  using Minary.DataTypes.Enum;
  using Minary.Form;
  using Minary.LogConsole.Main;
  using MinaryLib.AttackService.Class;
  using MinaryLib.AttackService.Enum;
  using MinaryLib.AttackService.Interface;
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Reflection;


  public class AttackServiceHandler : IAttackServiceHost
  {

    #region MEMBERS

    private MinaryMain minaryInstance;
    private Dictionary<string, IAttackService> attackServices;
    private Dictionary<string, Assembly> dynamicallyLoadedAttackServices;

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
      this.dynamicallyLoadedAttackServices = new Dictionary<string, Assembly>();
    }


    public void LoadAttackServicePlugins()
    {
      string fileName;
      string tempPluginPath;
      List<string> pluginList = null;

      try
      {
        pluginList = this.GetAttackServicePluginList();
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "Minary LoadAttackServicePlugins Exception: {0}", ex.Message);
        pluginList = new List<string>();
        return;
      }

      // Iterate through all plugin directories.
      for (int plugCount = 0; plugCount < pluginList.Count; plugCount++)
      {
        if (!Directory.Exists(pluginList[plugCount]))
        {
          continue;
        }

        tempPluginPath = pluginList[plugCount];
        string[] pluginFiles = Directory.GetFiles(tempPluginPath, "as_*.dll");

        for (int i = 0; i < pluginFiles.Length; i++)
        {
          fileName = Path.GetFileNameWithoutExtension(pluginFiles[i]);
          LogCons.Inst.Write(LogLevel.Info, "Found attack service plugin: {0}", pluginFiles[i]);
          
          // Create/Load instance of attack serviceplugin.
          try
          {
            this.InsertPluginIntoMainGui(pluginFiles[i]);
          }
          catch (Exception ex)
          {
            LogCons.Inst.Write(LogLevel.Error, "Error occurred while loading attack service {0} : {1}\r\n{2}", fileName, ex.StackTrace, ex.ToString());
            string message = string.Format("Error occurred while loading attack service {0} : {1}", fileName, ex.Message);
            MessageDialog.Inst.ShowError(string.Empty, message, this.minaryInstance);
            continue;
          }
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

    #endregion


    #region PRIVATE

    private List<string> GetAttackServicePluginList()
    {
      string baseDir = Directory.GetCurrentDirectory();
      string tempPluginPath = Path.Combine(baseDir, Config.AttackServicesPluginsDir);
      string[] tempPluginList = Directory.GetDirectories(tempPluginPath);
      List<string> pluginList = new List<string>();

      for (int i = 0; i < tempPluginList.Length; i++)
      {
        string[] pluginFiles = Directory.GetFiles(tempPluginList[i], "as_*.dll");

        if (pluginFiles.Length > 0)
        {
          pluginList.Add(tempPluginList[i]);
        }
      }

      return pluginList;
    }


    private void InsertPluginIntoMainGui(string currentPluginFile)
    {
      Type objType;
      Assembly assemblyObj;
      string fileName = Path.GetFileNameWithoutExtension(currentPluginFile);

      if ((assemblyObj = Assembly.LoadFrom(currentPluginFile)) == null)
      {
        return;
      }

      this.dynamicallyLoadedAttackServices.Add(assemblyObj.FullName, assemblyObj);

      string pluginName = string.Format("Minary.AttackService.Main.{0}", fileName);
      objType = assemblyObj.GetType(pluginName, false, false);

      if (objType == null)
      {
        return;
      }

      AttackServiceParameters attackServiceParams = new AttackServiceParameters()
      {
        AttackServiceHost = this,
        AttackServicesWorkingDirFullPath = Path.Combine(Directory.GetCurrentDirectory(), Config.AttackServicesPluginsDir),
        PipeName = Config.PipeName
      };

      object tmpPluginObj = Activator.CreateInstance(objType, attackServiceParams);
    }

    #endregion


    #region INTERFACE: IAttackServiceHandler

    public bool IsDebuggingOn { get { return Minary.Common.Debugging.IsDebuggingOn; } }

    public void Register(IAttackService attackService)
    {
      LogCons.Inst.Write(LogLevel.Info, "AttackServiceHandler.Register(): Service {0} registered", attackService.ServiceName);

      // Register the service in the attack service handler
      this.attackServices.Add(attackService.ServiceName, attackService);

      // Register the service in the GUI 
      this.MinaryMain.RegisterAttackService(attackService.ServiceName);
    }


    public void OnServiceExited(string serviceName, int exitCode)
    {
      LogCons.Inst.Write(LogLevel.Error, "OnServiceExited(): Service \"{0}\" exited unexpectedly. Exit code {0}", serviceName, exitCode);
      this.minaryInstance.OnServiceExicedUnexpectedly(serviceName);
    }


    public void LogMessage(string message, params object[] formatArgs)
    {
      LogCons.Inst.Write(LogLevel.Info, message, formatArgs);
    }

    #endregion

  }
}
