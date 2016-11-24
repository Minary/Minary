namespace Minary
{
  using Minary.ArpScan.DataTypes;
  using Minary.Common;
  using MinaryLib;
  using MinaryLib.AttackService;
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

    private readonly IDictionary<string, Assembly> dynamicallyLoadedAssembly;
    private MinaryMain minaryMain;
    private ConcurrentDictionary<string, DataTypes.MinaryExtension> tabPagesCatalog;

    #endregion


    #region PROPERTIES

    public ConcurrentDictionary<string, DataTypes.MinaryExtension> TabPagesCatalog { get { return this.tabPagesCatalog; } set { } }

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
      this.tabPagesCatalog = new ConcurrentDictionary<string, DataTypes.MinaryExtension>();
      this.dynamicallyLoadedAssembly = new Dictionary<string, Assembly>();

      // Hook "asslembly loading" events
      AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += this.ResolveAssembly;
      AppDomain.CurrentDomain.AssemblyResolve += this.ResolveAssembly;
    }


    /// <summary>
    /// Load all activated plugins
    /// </summary>
    public void LoadPlugins()
    {
      string fileName;
      string tempPluginPath;
      List<string> pluginList = null;

      try
      {
        pluginList = this.GetPluginList();
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary LoadPlugins Exception: {0}", ex.Message);
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
        string[] pluginFiles = Directory.GetFiles(tempPluginPath, "plugin_*.dll");

        for (int i = 0; i < pluginFiles.Length; i++)
        {
          fileName = Path.GetFileNameWithoutExtension(pluginFiles[i]);
          LogConsole.Main.LogConsole.LogInstance.LogMessage("Found plugin: {0}", pluginFiles[i]);

          // Create/Load instance of plugin.
          try
          {
            this.InsertPluginIntoMainGui(tempPluginPath, pluginFiles[i]);
          }
          catch (Exception ex)
          {
            LogConsole.Main.LogConsole.LogInstance.LogMessage("Error occurred while loading plugin {0} : {1}\r\n{2}", fileName, ex.StackTrace, ex.ToString());
            MessageBox.Show(string.Format("Error occurred while loading plugin {0} : {1}", fileName, ex.Message));

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
      foreach (string key in this.tabPagesCatalog.Keys)
      {
        try
        {
          this.tabPagesCatalog[key].PluginObject.OnResetPlugin();
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("PluginHandler.ResetAllPlugins(): Exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
        }
      }
    }

    
    /// <summary>
    ///
    /// </summary>
    public void StopAllPlugins()
    {
      foreach (string key in this.tabPagesCatalog.Keys)
      {
        try
        {
          this.tabPagesCatalog[key].PluginObject.OnStopAttack();
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("PluginHandler.ResetAllPlugins(): Exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
        }
      }
    }

    
    /// <summary>
    ///
    /// </summary>
    public void RestoreLastPluginLoadState()
    {
      foreach (string key in this.tabPagesCatalog.Keys)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.RestoreLastPluginLoadState(): PluginName:{0}", key);
        try
        {
          IPlugin tmpPluginObj = this.tabPagesCatalog[key].PluginObject;
          string currentPluginState = Utils.GetRegistryValue(key, "state");

          if (currentPluginState == null)
          {
            LogConsole.Main.LogConsole.LogInstance.LogMessage("RestoreLastPluginLoadState(): No former state found for plugin {0}", key);
          }
          else if (currentPluginState.ToLower() == "on")
          {
            this.ActivatePlugin(key);
          }
          else
          {
            this.DeactivatePlugin(key);
          }
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("RestoreLastPluginLoadState(): Exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
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
      int pluginRowDGV = this.minaryMain.MinaryTaskFacade.GetPluginDGVRowByName(pluginName);

      if (this.minaryMain.UsedPlugins[pluginRowDGV].Active != "0")
      {
        return;
      }

      if ((tabPage = this.minaryMain.PluginHandler.FindTabPageInCatalog(pluginName)) == null)
      {
        throw new Exception("Plugin tab page could not be found");
      }

      // Set new status in the registry (to survive the application stop)
      Utils.SetRegistryValue(pluginName, "state", "on");

      // Set new status in the main GUI DGV
      this.minaryMain.MinaryTaskFacade.ActivatePlugin(pluginName);
      this.minaryMain.MinaryTabPageHandler.ShowTabPage(tabPage.Text);

      // Set new status in the tab page catalog
      this.tabPagesCatalog[pluginName].IsActive = true;
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
      int pluginRowDGV = this.minaryMain.MinaryTaskFacade.GetPluginDGVRowByName(pluginName);

      if (this.minaryMain.UsedPlugins[pluginRowDGV].Active != "1")
      {
        return;
      }

      if ((tabPage = this.minaryMain.PluginHandler.FindTabPageInCatalog(pluginName)) == null)
      {
        throw new Exception("Plugin tab page could not be found");
      }

      // Set new status in the registry (to survive the application stop)
      Utils.SetRegistryValue(this.minaryMain.UsedPlugins[pluginRowDGV].PluginName, "state", "off");

      // Set new status in the main GUI DGV
      this.minaryMain.MinaryTaskFacade.DeactivatePlugin(pluginName);
      this.minaryMain.MinaryTabPageHandler.HideTabPage(tabPage.Text);

      // Set new status in the tab page catalog
      this.tabPagesCatalog[pluginName].IsActive = false;
    }

    #endregion


    #region PRIVATE METHODS

    /// <summary>
    /// Get plugin list
    /// </summary>
    /// <returns></returns>
    private List<string> GetPluginList()
    {
      string baseDir = Directory.GetCurrentDirectory();
      string tempPluginPath = Path.Combine(baseDir, Config.PluginsDir);
      string[] tempPluginList = Directory.GetDirectories(tempPluginPath);
      List<string> pluginList = new List<string>();

      for (int i = 0; i < tempPluginList.Length; i++)
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
      string fileName = Path.GetFileNameWithoutExtension(currentPluginFile);

      //      if ((assemblyObj = Assembly.LoadFile(currentPluginFile)) == null)
      //Assembly assemblyObj2 = Assembly.Load(currentPluginFile);
      if ((assemblyObj = Assembly.LoadFrom(currentPluginFile)) == null)
      {
        return;
      }

      this.dynamicallyLoadedAssembly.Add(assemblyObj.FullName, assemblyObj);

      string pluginName = string.Format("Minary.Plugin.Main.{0}", fileName);
      objType = assemblyObj.GetType(pluginName, false, false);

      if (objType == null)
      {
        return;
      }

      PluginProperties pluginProperties = new PluginProperties()
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
      if (!(tmpPluginObj is IPlugin) || !(tmpPluginObj is UserControl))
      {
        return;
      }

      try
      {
        IPlugin newPluginIPlugin = (IPlugin)tmpPluginObj;
        UserControl newPluginUserControl = (UserControl)tmpPluginObj;
        TabPage newPluginTabPage = new TabPage(newPluginIPlugin.Config.PluginName);

        // Initialize new tab page ...
        newPluginTabPage.Controls.Add(newPluginIPlugin.PluginControl);
        newPluginTabPage.BackColor = this.minaryMain.TabPagePluginCatalog.BackColor;
        newPluginTabPage.ImageIndex = (int)MinaryLib.Plugin.Status.NotRunning;
        newPluginTabPage.BorderStyle = BorderStyle.None;

        // Let the plugin user control adapt its size when parent control (the tab control) resizes.
        newPluginUserControl.Dock = DockStyle.Fill;

        // Add new plugin to catalog ...
        this.minaryMain.MinaryTabPageHandler.AddTabPageToCatalog(newPluginIPlugin.Config.PluginName, newPluginIPlugin, newPluginTabPage);

        // Add new plugin to Minary plugins TabPage
        this.minaryMain.DGVUsedPlugins.Add(new PluginTableRecord(newPluginIPlugin.Config.PluginName, newPluginIPlugin.Config.PluginType, newPluginIPlugin.Config.PluginDescription, "0"));

        // Call plugin initialization methods
        this.tabPagesCatalog[newPluginIPlugin.Config.PluginName].PluginObject.OnInit();
      }
      catch (ArgumentNullException ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArgumentNullException {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
      }
      catch (ArgumentException ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArgumentException {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
      }
      catch (NotSupportedException ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("NotSupportedException {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
      }
      catch (TargetInvocationException ex)
      {
        if (ex.InnerException != null)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("TargetInvocationException {0}: {1} - {2}", fileName, ex.Message, ex.InnerException.Message);
        }
        else
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("TargetInvocationException {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
        }
      }
      catch (MethodAccessException ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("MethodAccessException {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
      }
      catch (MemberAccessException ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("MemberAccessException {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
      }
      catch (TypeLoadException ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("TypeLoadException {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("Exception {0}: {1} {2}", fileName, ex.Message, ex.StackTrace);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="status"></param>
    private void SetStateInMinaryTable(string pluginName, string status)
    {
      foreach (PluginTableRecord tmpRecord in this.minaryMain.DGVUsedPlugins)
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

        foreach (TargetRecord tmpTarget in ArpScan.Presentation.ArpScan.GetInstance().TargetList)
        {
          retVal.Add(new Tuple<string, string, string>(tmpTarget.MacAddress, tmpTarget.IpAddress, tmpTarget.Vendor));
        }

        return retVal;
      }
    }
 
    public Form MainWindowForm { get { return this.minaryMain; } }

    public bool IsDebuggingOn { get { return Debugging.IsDebuggingOn(); } }

    public string HostWorkingDirectory { get { return Directory.GetCurrentDirectory(); } }

    public Dictionary<string, IAttackService> AttackServiceList { get { return this.minaryMain.MinaryAttackServiceHandler.AttackServices; } }

    public void LogMessage(string message, params object[] formatArgs)
    {
      LogConsole.Main.LogConsole.LogInstance.LogMessage(message, formatArgs);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginObj"></param>
    /// <param name="status"></param>
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

        if (tabPage != null)
        {
          int tmpNewPluginStatus = (int)newPluginStatus;
          int oldPluginStatus = tabPage.ImageIndex;

          tmpNewPluginStatus = (newPluginStatus >= 0) ? (int)newPluginStatus : (int)MinaryLib.Plugin.Status.NotRunning;
          tabPage.ImageIndex = tmpNewPluginStatus;
          LogConsole.Main.LogConsole.LogInstance.LogMessage(@"{0} : CurrentState:{1}, NewState:{2}", plugin.Config.PluginName, oldPluginStatus, tmpNewPluginStatus);
        }
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ReportPluginSetStatus() : {0}", ex.ToString());
      }
    }


    /// <summary>
    /// Plugin is connecting back to register itself actively.
    /// Create ControlTab and customize the plugin/tab appearance.
    /// </summary>
    /// <param name="pPlugin"></param>
    /// <returns></returns>
    public void Register(IPlugin plugin)
    {
      // After plugin called back the host application for registration
      // call plugin initialization methods
      this.tabPagesCatalog[plugin.Config.PluginName].PluginObject.OnResetPlugin();
      this.tabPagesCatalog[plugin.Config.PluginName].PluginObject.OnStartUpdate();

      LogConsole.Main.LogConsole.LogInstance.LogMessage(@"{0} : Plugin is calling back for registration", plugin.Config.PluginName);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="tabName"></param>
    /// <returns></returns>
    public TabPage FindTabPageInCatalog(string tabName)
    {
      if (!string.IsNullOrEmpty(tabName) &&
          this.tabPagesCatalog != null &&
          this.tabPagesCatalog.ContainsKey(tabName))
      {
        return this.tabPagesCatalog[tabName].PluginTabPage;
      }

      return null;
    }

    #endregion

  }
}
