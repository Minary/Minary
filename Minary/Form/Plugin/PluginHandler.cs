namespace Minary.Form
{
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using MinaryLib;
  using MinaryLib.AttackService.Interface;
  using MinaryLib.Plugin;
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.IO;
  using System.Reflection;
  using System.Windows.Forms;


  public class PluginHandler : Control, IPluginHost
  {

    #region MEMBERS

    private readonly IDictionary<string, Assembly> dynamicallyLoadedAssembly = new Dictionary<string, Assembly>();
    private MinaryMain minaryMain;

    #endregion


    #region PROPERTIES

    public ConcurrentDictionary<string, DataTypes.MinaryExtension> TabPagesCatalog { get; private set; } = new ConcurrentDictionary<string, DataTypes.MinaryExtension>();

    #endregion


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginHandler"/> class.
    ///
    /// </summary>
    /// <param name="minaryMain"></param>
    public PluginHandler(MinaryMain minaryMain)
    {
      this.minaryMain = minaryMain;

      // Hook "asslembly loading" events
      AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += this.ResolveAssembly;
      AppDomain.CurrentDomain.AssemblyResolve += this.ResolveAssembly;
    }


    /// <summary>
    /// Load all activated plugins
    /// </summary>
    public void LoadPlugins()
    {
      var fileName = string.Empty;
      var tempPluginPath = string.Empty;
      var pluginList = new List<string>();

      try
      {
        pluginList = this.GetPluginList();
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"Minary LoadPlugins Exception: {ex.Message}");
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
        string[] pluginFiles = Directory.GetFiles(tempPluginPath, "plugin_*.dll");

        for (var i = 0; i < pluginFiles.Length; i++)
        {
          fileName = Path.GetFileNameWithoutExtension(pluginFiles[i]);
          LogCons.Inst.Write(LogLevel.Info, "Found plugin: {0}", pluginFiles[i]);

          // Create/Load instance of plugin.
          try
          {
            this.InsertPluginIntoMainGui(tempPluginPath, pluginFiles[i]);
          }
          catch (Exception ex)
          {
            LogCons.Inst.Write(LogLevel.Error, $"Error occurred while loading plugin {fileName} : {ex.StackTrace}\r\n{ex.ToString()}");
            string message = $"Error occurred while loading plugin {fileName} : {ex.Message}";
            MessageDialog.Inst.ShowError(string.Empty, message, this.minaryMain);
            continue;
          }
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    public void ResetAllPlugins()
    {
      foreach (var key in this.TabPagesCatalog.Keys)
      {
        try
        {
          this.TabPagesCatalog[key].PluginObject.OnResetPlugin();
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"PluginHandler.ResetAllPlugins(): Exception: {ex.Message}\r\n{ex.StackTrace}");
        }
      }
    }

    
    /// <summary>
    ///
    /// </summary>
    public void StopAllPlugins()
    {
      foreach (var key in this.TabPagesCatalog.Keys)
      {
        try
        {
          this.TabPagesCatalog[key].PluginObject.OnStopAttack();
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"PluginHandler.ResetAllPlugins(): Exception: {ex.Message}\r\n{ex.StackTrace}");
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    public void RestoreLastPluginLoadState()
    {
      foreach (string key in this.TabPagesCatalog.Keys)
      {
        try
        {
          IPlugin tmpPluginObj = this.TabPagesCatalog[key].PluginObject;
          string currentPluginState = WinRegistry.GetValue(key, "state");

          if (currentPluginState == null)
          {
            LogCons.Inst.Write(LogLevel.Info, $"RestoreLastPluginLoadState(): No former state found for plugin {key}");
          }
          else if (currentPluginState.ToLower() == "on")
          {
            LogCons.Inst.Write(LogLevel.Info, $"Minary.RestoreLastPluginLoadState(): PluginName:{key} State:on");
            this.ActivatePlugin(key);
          }
          else
          {
            LogCons.Inst.Write(LogLevel.Info, $"Minary.RestoreLastPluginLoadState(): PluginName:{key} State:off");
            this.DeactivatePlugin(key);
          }
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"RestoreLastPluginLoadState(): Exception: {ex.Message}\r\n{ex.StackTrace}");
        }
      }
    }


    public bool IsPluginActive(string pluginName)
    {
      return this.minaryMain.PluginHandler.TabPagesCatalog[pluginName].IsActive;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="pluginName"></param>
    public delegate void ActivatePluginDelegate(string pluginName);
    public void ActivatePlugin(string pluginName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ActivatePluginDelegate(this.ActivatePlugin), new object[] { pluginName });
        return;
      }

      TabPage tabPage;
      int pluginRowDgv = this.minaryMain.MinaryTaskFacade.GetPluginDgvRowByName(pluginName);

      if (pluginRowDgv < 0)
      {
        throw new Exception($"The plugin \"{pluginName}\" does not exist");
      }

      if (this.minaryMain.UsedPlugins[pluginRowDgv].Active != "0")
      {
        return;
      }

      if ((tabPage = this.minaryMain.PluginHandler.FindTabPageInCatalog(pluginName)) == null)
      {
        throw new Exception($"The plugin tab page \"{pluginName}\" could not be found");
      }

      // Set new status in the registry (to survive the application stop)
      WinRegistry.SetValue(pluginName, "state", "on");

      // Set new status in the main GUI DGV
      this.minaryMain.MinaryTaskFacade.ActivatePlugin(pluginName);
      this.minaryMain.MinaryTabPageHandler.ShowTabPage(tabPage.Text);

      // Set new status in the tab page catalog
      this.TabPagesCatalog[pluginName].IsActive = true;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="pluginName"></param>
    public delegate void DeactivatePluginDelegate(string pluginName);
    public void DeactivatePlugin(string pluginName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new DeactivatePluginDelegate(this.DeactivatePlugin), new object[] { pluginName });
        return;
      }

      TabPage tabPage;
      int pluginRowDGV = this.minaryMain.MinaryTaskFacade.GetPluginDgvRowByName(pluginName);

      if (this.minaryMain.UsedPlugins[pluginRowDGV].Active != "1")
      {
        return;
      }

      if ((tabPage = this.minaryMain.PluginHandler.FindTabPageInCatalog(pluginName)) == null)
      {
        throw new Exception("Plugin tab page could not be found");
      }

      // Set new status in the registry (to survive the application stop)
      WinRegistry.SetValue(this.minaryMain.UsedPlugins[pluginRowDGV].PluginName, "state", "off");

      // Set new status in the main GUI DGV
      this.minaryMain.MinaryTaskFacade.DeactivatePlugin(pluginName);
      this.minaryMain.MinaryTabPageHandler.HideTabPage(tabPage.Text);

      // Set new status in the tab page catalog
      this.TabPagesCatalog[pluginName].IsActive = false;
    }

    #endregion


    #region PRIVATE METHODS

    /// <summary>
    /// Get plugin list
    /// </summary>
    /// <returns></returns>
    private List<string> GetPluginList()
    {
      var baseDir = Directory.GetCurrentDirectory();
      var tempPluginPath = Path.Combine(baseDir, Config.PluginsDir);
      string[] tempPluginList = Directory.GetDirectories(tempPluginPath);
      var pluginList = new List<string>();

      for (var i = 0; i < tempPluginList.Length; i++)
      {
        string[] pluginFiles = Directory.GetFiles(tempPluginList[i], "plugin_*.dll");

        if (pluginFiles.Length > 0)
        {
          pluginList.Add(tempPluginList[i]);
        }
      }

      return pluginList;
    }


    private void InsertPluginIntoMainGui(string tempPluginPath, string currentPluginFile)
    {
      Type objType;
      Assembly assemblyObj;
      var fileName = Path.GetFileNameWithoutExtension(currentPluginFile);

      if ((assemblyObj = Assembly.LoadFrom(currentPluginFile)) == null)
      {
        return;
      }

      this.dynamicallyLoadedAssembly.Add(assemblyObj.FullName, assemblyObj);
      var pluginName = $"Minary.Plugin.Main.{fileName}";
      objType = assemblyObj.GetType(pluginName, false, false);

      if (objType == null)
      {
        return;
      }

      var pluginProperties = new PluginProperties()
      {
        ApplicationBaseDir = Directory.GetCurrentDirectory(),
        PluginBaseDir = tempPluginPath,
        PatternSubDir = Config.PatternDir,
        HostApplication = (IPluginHost)this
      };

      /*
       * Add loaded plugin to ...
       * - the global "plugin list" (IPlugin)
       * - the "used plugins DGV" list
       * - the "plugin position" list (name + position)
       */
      object tmpPluginObj = Activator.CreateInstance(objType, pluginProperties);
      if ((tmpPluginObj is IPlugin) == false || 
          (tmpPluginObj is UserControl) == false)
      {
        return;
      }

      try
      {
        var newPluginIPlugin = (IPlugin)tmpPluginObj;
        var newPluginUserControl = (UserControl)tmpPluginObj;
        var newPluginTabPage = new TabPage(newPluginIPlugin.Config.PluginName);

        // Initialize new tab page ...
        newPluginTabPage.Controls.Add(newPluginIPlugin.PluginControl);
        newPluginTabPage.BackColor = this.minaryMain.TabPagePluginCatalog.BackColor;
        newPluginTabPage.ImageIndex = (int)Status.NotRunning;
        newPluginTabPage.BorderStyle = BorderStyle.None;

        // Let the plugin user control adapt its size when parent control (the tab control) resizes.
        newPluginUserControl.Dock = DockStyle.Fill;

        // Add new plugin to catalog ...
        this.minaryMain.MinaryTabPageHandler.AddTabPageToCatalog(newPluginIPlugin.Config.PluginName, newPluginIPlugin, newPluginTabPage);

        // Add new plugin to Minary plugins TabPage
        this.minaryMain.UsedPlugins.Add(new PluginTableRecord(newPluginIPlugin.Config.PluginName, newPluginIPlugin.Config.PluginType, newPluginIPlugin.Config.PluginDescription, "0"));

        // Call plugin initialization methods
        this.TabPagesCatalog[newPluginIPlugin.Config.PluginName].PluginObject.OnInit();
      }
      catch (ArgumentNullException ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"ArgumentNullException {fileName}: {ex.Message} {ex.StackTrace}");
      }
      catch (ArgumentException ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"ArgumentException {fileName}: {ex.Message} {ex.StackTrace}");
      }
      catch (NotSupportedException ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"NotSupportedException {fileName}: {ex.Message} {ex.StackTrace}");
      }
      catch (TargetInvocationException ex)
      {
        if (ex.InnerException != null)
        {
          LogCons.Inst.Write(LogLevel.Error, $"TargetInvocationException {fileName}: {ex.Message} - {ex.InnerException.Message}");
        }
        else
        {
          LogCons.Inst.Write(LogLevel.Error, $"TargetInvocationException {fileName}: {ex.Message} {ex.StackTrace}");
        }
      }
      catch (MethodAccessException ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"MethodAccessException {fileName}: {ex.Message} {ex.StackTrace}");
      }
      catch (MemberAccessException ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"MemberAccessException {fileName}: {ex.Message} {ex.StackTrace}");
      }
      catch (TypeLoadException ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"TypeLoadException {fileName}: {ex.Message} {ex.StackTrace}");
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"Exception {fileName}: {ex.Message} {ex.StackTrace}");
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="status"></param>
    private void SetStateInMinaryTable(string pluginName, string status)
    {
      foreach (PluginTableRecord tmpRecord in this.minaryMain.UsedPlugins)
      {
        if (tmpRecord.PluginName == pluginName)
        {
          tmpRecord.Active = status;
        }
      }
    }


    private Assembly ResolveAssembly(object sender, ResolveEventArgs e)
    {
      Assembly res;
      this.dynamicallyLoadedAssembly.TryGetValue(e.Name, out res);
      return res;
    }

    #endregion


    #region IPluginHost INTERFACE IMPLEMENTATION

    public string CurrentInterface { get { return this.minaryMain.GetCurrentInterface(); } }

    public string StartIP { get { return this.minaryMain.NetworkStartIp; } }

    public string StopIP { get { return this.minaryMain.NetworkStopIp; } }

    public string CurrentIP { get { return this.minaryMain.CurrentLocalIp; } }

    public List<Tuple<string, string, string>> ReachableSystemsList
    {
      get
      {
        List<Tuple<string, string, string>> retVal = new List<Tuple<string, string, string>>();

        foreach (TargetRecord tmpTarget in this.minaryMain.ArpScanHandler.TargetList)
        {
          retVal.Add(new Tuple<string, string, string>(tmpTarget.MacAddress, tmpTarget.IpAddress, tmpTarget.Vendor));
        }

        return retVal;
      }
    }
 
    public Form MainWindowForm { get { return this.minaryMain; } }

    public bool IsDebuggingOn { get { return Debugging.IsDebuggingOn; } }

    public string HostWorkingDirectory { get { return Directory.GetCurrentDirectory(); } }

    public Dictionary<string, IAttackService> AttackServiceList { get { return this.minaryMain.MinaryAttackServiceHandler.AttackServices; } }

    public void LogMessage(string message, params object[] formatArgs)
    {
      LogCons.Inst.Write(LogLevel.Info, message, formatArgs);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="callingPluginObj"></param>
    /// <param name="newPluginStatus"></param>
    public delegate void PluginSetStatusDelegate(object callingPluginObj, MinaryLib.Plugin.Status newPluginStatus);
    public void ReportPluginSetStatus(object callingPluginObj, MinaryLib.Plugin.Status newPluginStatus)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new PluginSetStatusDelegate(this.ReportPluginSetStatus), new object[] { callingPluginObj, newPluginStatus });
        return;
      }

      IPlugin plugin = null;
      TabPage tabPage = null;

      if (callingPluginObj == null)
      {
        return;
      }

      try
      {
        plugin = (IPlugin)callingPluginObj;
        tabPage = this.FindTabPageInCatalog(plugin.Config.PluginName);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"ReportPluginSetStatus(): {ex.ToString()}");
      }

      if (tabPage == null)
      {
        return;
      }

      var tmpNewPluginStatus = (int)newPluginStatus;
      int oldPluginStatus = tabPage.ImageIndex;

      tmpNewPluginStatus = (newPluginStatus >= 0) ? (int)newPluginStatus : (int)MinaryLib.Plugin.Status.NotRunning;
      tabPage.ImageIndex = tmpNewPluginStatus;

      if (oldPluginStatus == tmpNewPluginStatus)
      {
        return;
      }

      LogCons.Inst.Write(LogLevel.Info, $"{plugin.Config.PluginName} : CurrentState:{oldPluginStatus}, NewState:{tmpNewPluginStatus}");
    }


    /// <summary>
    /// Plugin is connecting back to register itself actively.
    /// Create ControlTab and customize the plugin/tab appearance.
    /// </summary>
    /// <param name="plugin"></param>
    /// <returns></returns>
    public void Register(IPlugin plugin)
    {
      // After plugin called back the host application for registration
      // call plugin initialization method
      this.TabPagesCatalog[plugin.Config.PluginName].PluginObject.OnResetPlugin();
      LogCons.Inst.Write(LogLevel.Info, $"{plugin.Config.PluginName} : Plugin is calling back for registration");
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="tabName"></param>
    /// <returns></returns>
    public TabPage FindTabPageInCatalog(string tabName)
    {
      if (string.IsNullOrEmpty(tabName) == false &&
          this.TabPagesCatalog?.ContainsKey(tabName) == true)
      {
        return this.TabPagesCatalog[tabName].PluginTabPage;
      }

      return null;
    }

    #endregion

  }
}
