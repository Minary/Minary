namespace Minary.DSL
{
  using Minary.Template.DataTypes.Template;
  using MinaryLib.DataTypes;
  using System;
  using System.Threading;


  public class Calls
  {

    #region MEMBERS

    private const int MaxArpScanRuntime = 100000;
    private const int MaxSelectedTargetSystems = 10;
    private const int MaxSystemsToScan = 255;
    private static ManualResetEvent manualEventArpScanStopped = new ManualResetEvent(false);
    private MinaryMain minaryMain;
    private RecordMinaryTemplate minaryTemplate;

    #endregion
 

    #region PUBLIC

    public Calls(MinaryMain minaryMain, RecordMinaryTemplate minaryTemplate)
    {
      this.minaryMain = minaryMain;
      this.minaryTemplate = minaryTemplate;
    }


    public void ScanNetwork()
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: ScanNetwork()"));

      if (this.minaryTemplate == null || this.minaryTemplate.AttackConfig == null)
      {
        throw new Exception("The template is invalid");
      }

      if (this.minaryTemplate.AttackConfig.ScanNetwork != 1)
      {
        return;
      }

      System.ComponentModel.BindingList<string> targetList = new System.ComponentModel.BindingList<string>();
      manualEventArpScanStopped.Reset();
      Minary.ArpScan.Presentation.ArpScan.StartArpScanInBackground(this.ArpScanStopped, MaxSystemsToScan);

      // If ArpScan did not return after "maxArpScanRuntime" milliseconds
      // interrupt the scanning process.
      if (manualEventArpScanStopped.WaitOne(MaxArpScanRuntime))
      {
        Minary.ArpScan.Presentation.ArpScan.StopRunningArpScan();
      }
    }


    public int GetCurrentNumberOfTargetSystems()
    {
      return Minary.ArpScan.Presentation.ArpScan.NumberTargetSystems();
    }


    public void SelectTargetSystems(int noTargetSystems = MaxSelectedTargetSystems)
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: SelectTargetSystems()"));

      if (noTargetSystems <= 0)
      {
        return;
      }

      if (noTargetSystems > MaxSelectedTargetSystems)
      {
        return;
      }

      Minary.ArpScan.Presentation.ArpScan.SelectRandomSystems(noTargetSystems);
    }


    public void StartAttack()
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: StartAttack()"));
      this.minaryMain.StartAttacksOnBackground();
    }


    public void HideAllTabPages()
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: HideAllTabPages()"));
      this.minaryMain.MinaryTabPageHandler.HideAllTabPages();
    }


    public void ActivatePlugin(string pluginName)
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: ActivatePlugin() \"{0}\"", pluginName));
      this.minaryMain.PluginHandler.ActivatePlugin(pluginName);
    }


    public void DeactivatePlugin(string pluginName)
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: DeactivatePlugin() \"{0}\"", pluginName));
      this.minaryMain.PluginHandler.DeactivatePlugin(pluginName);
    }


    public void LoadPluginData(string tabPageName, TemplatePluginData pluginData)
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: LoadPluginData() \"{0}\"", tabPageName));
      Minary.DataTypes.MinaryExtension realPluginObj = this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName];
      realPluginObj.PluginObject.OnLoadTemplateData(pluginData);
    }


    public void ResetAllPlugins()
    {
      this.minaryMain.GetLogConsole.LogMessage(string.Format("Calls interface: ResetAllPluginsn()"));
      this.minaryMain.ResetAllPlugins();
    }

    #endregion


    #region PRIVATE

    private void ArpScanStopped()
    {
      manualEventArpScanStopped.Set();
    }

    #endregion

  }
}