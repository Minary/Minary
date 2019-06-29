namespace Minary.Domain.AttackService
{
  using Minary.DataTypes.Enum;
  using Minary.Form;
  using Minary.Form.GuiAdvanced;
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
    private Dictionary<string, Assembly> dynamicallyLoadedAttackServices = new Dictionary<string, Assembly>();

    #endregion


    #region PROPERTIES

    public Dictionary<string, IAttackService> AttackServices { get; private set; } = new Dictionary<string, IAttackService>();

    #endregion


    #region PUBLIC

    public AttackServiceHandler(MinaryMain minaryInstance)
    {
      this.minaryInstance = minaryInstance;
    }


    /// <summary>
    /// 
    /// </summary>
    public void LoadAttackServicePlugins()
    {
      var fileName = string.Empty;
      var tempPluginPath = string.Empty;
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
      for (var plugCount = 0; plugCount < pluginList.Count; plugCount++)
      {
        if (!Directory.Exists(pluginList[plugCount]))
        {
          continue;
        }

        tempPluginPath = pluginList[plugCount];
        string[] pluginFiles = Directory.GetFiles(tempPluginPath, "as_*.dll");

        for (var i = 0; i < pluginFiles.Length; i++)
        {
          fileName = Path.GetFileNameWithoutExtension(pluginFiles[i]);
          LogCons.Inst.Write(LogLevel.Info, $"Found attack service plugin: {pluginFiles[i]}");
          
          // Create/Load instance of attack serviceplugin.
          try
          {
            this.InsertPluginIntoMainGui(pluginFiles[i]);
          }
          catch (Exception ex)
          {
            LogCons.Inst.Write(LogLevel.Error, $"Error occurred while loading attack service {fileName} : {ex.StackTrace}\r\n{ex.ToString()}");
            var message = $"Error occurred while loading attack service {fileName} : {ex.Message}";
            MessageDialog.Inst.ShowError(string.Empty, message, this.minaryInstance);
            continue;
          }
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void StopAllServices()
    {
      foreach (var tmpKey in this.AttackServices.Keys)
      {
        try
        {
          // Stop attack services that have the current state RUNNING.
          if (this.AttackServices[tmpKey].Status == ServiceStatus.Running)
          {
            ServiceStatus newServiceStatus = this.AttackServices[tmpKey].StopService();
            this.minaryInstance.SetNewAttackServiceState(tmpKey, newServiceStatus);
          }
        }
        catch (Exception ex)
        {
          this.minaryInstance.SetNewAttackServiceState(tmpKey, ServiceStatus.Error);
          LogCons.Inst.Write(LogLevel.Error, $"AttackServiceHandler.StopAllServices(Exception): {this.AttackServices[tmpKey].ServiceName}: {ex.Message}\r\n{ex.StackTrace}");
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    public void ShutDown()
    {
      foreach (var tmpKey in this.AttackServices.Keys)
      {
        try
        {
          this.AttackServices[tmpKey].StopService();
          this.minaryInstance.SetNewAttackServiceState(tmpKey, ServiceStatus.NotRunning);
        }
        catch (Exception ex)
        {
          this.minaryInstance.SetNewAttackServiceState(tmpKey, ServiceStatus.Error);
          LogCons.Inst.Write(LogLevel.Error, "AttackServiceHandler.ShutDown(Exception): {0}", ex.Message);
        }
      }
    }

    #endregion


    #region PRIVATE

    private List<string> GetAttackServicePluginList()
    {
      var baseDir = Directory.GetCurrentDirectory();
      var tempPluginPath = Path.Combine(baseDir, Config.AttackServicesPluginsDir);
      string[] tempPluginList = Directory.GetDirectories(tempPluginPath);
      List<string> pluginList = new List<string>();

      for (var i = 0; i < tempPluginList.Length; i++)
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

      var pluginName = $"Minary.AttackService.Main.{fileName}";
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
      LogCons.Inst.Write(LogLevel.Info, $"AttackServiceHandler.Register(): Service {attackService.ServiceName} registered");

      // Register the service in the attack service handler
      this.AttackServices.Add(attackService.ServiceName, attackService);

      // Register the service in the GUI 
      this.minaryInstance.RegisterAttackService(attackService.ServiceName);
    }


    public void OnServiceExited(string serviceName, int exitCode)
    {
      LogCons.Inst.Write(LogLevel.Error, $"OnServiceExited(): Service \"{serviceName}\" exited unexpectedly. Exit code {exitCode}");
      this.minaryInstance.OnServiceExicedUnexpectedly(serviceName);
    }


    public void LogMessage(string message, params object[] formatArgs)
    {
      LogCons.Inst.Write(LogLevel.Info, message, formatArgs);
    }

    #endregion

  }
}
