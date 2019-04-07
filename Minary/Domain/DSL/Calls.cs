namespace Minary.Domain.DSL
{
  using Minary.DataTypes.Enum;
  using Minary.Form.Main;
  using Minary.Form.Template.DataTypes.Template;
  using Minary.LogConsole.Main;
  using MinaryLib.DataTypes;
  using System;


  public class Calls
  {

    #region MEMBERS

    private const int MaxArpScanRuntime = 100000;
    private const int MaxSelectedTargetSystems = 10;
    private const int MaxSystemsToScan = 255;
    private MinaryMain minaryMain;
    private MinaryTemplateData minaryTemplate;
    private Minary.Form.ArpScan.Presentation.ArpScan arpScanHandler;

    #endregion


    #region PUBLIC

    public Calls(MinaryMain minaryMain, MinaryTemplateData minaryTemplate)
    {
      this.minaryMain = minaryMain;
      this.minaryTemplate = minaryTemplate;
      this.arpScanHandler = minaryMain.ArpScanHandler;
    }


    public delegate void ScanNetworkDelegate(Action onArpScanDone);
    public void ScanNetwork(Action onArpScanDone)
    {
      if (this.minaryMain.InvokeRequired)
      {
        this.minaryMain.BeginInvoke(new ScanNetworkDelegate(this.ScanNetwork), new object[] { onArpScanDone });
        return;
      }

      this.onArpScanDone = onArpScanDone;
      LogCons.Inst.Write(LogLevel.Info, "Calls interface: ScanNetwork()");
      DataTypes.Struct.MinaryConfig minaryConfig = this.minaryMain.MinaryTaskFacade.GetCurrentMinaryConfig();

      try
      {
        this.arpScanHandler.ShowArpScanGui(this.minaryMain.targetList, minaryConfig, false);
        this.arpScanHandler.StartArpScan(this.OnScanDone);
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show($"Message:{ex.Message}\r\n\r\nStacktrace{ex.StackTrace}", "ERROR");
      }
    }

    private Action onArpScanDone;
    public void OnScanDone()
    {
      this.arpScanHandler.HideArpScanWindow();
      LogCons.Inst.Write(LogLevel.Info, $"OnScanDone(): DONE, No. targets:{this.minaryMain.targetList.Count}");
      LogCons.Inst.Write(LogLevel.Info, "OnScanDone(): TARGEST,{0}", string.Join(", ", this.minaryMain.targetList));

      if (this.onArpScanDone != null)
      {
        this.onArpScanDone();
      }
    }


    public int GetCurrentNumberOfTargetSystems()
    {
      return this.arpScanHandler.NumberTargetSystems();
    }


    public void SelectTargetSystems(int noTargetSystems = MaxSelectedTargetSystems)
    {
      LogCons.Inst.Write(LogLevel.Info, $"Calls interface: SelectTargetSystems() noTargetSystems={noTargetSystems}");

      if (noTargetSystems <= 0)
      {
        return;
      }

      if (noTargetSystems > MaxSelectedTargetSystems)
      {
        return;
      }

      this.arpScanHandler.SelectRandomSystems(noTargetSystems);
    }


    public delegate void StartAttackDelegate();
    public void StartAttack()
    {
      LogCons.Inst.Write(LogLevel.Info, "Calls interface: StartAttack()");

      this.minaryMain.BeginInvoke((Action)delegate {
        this.minaryMain.StartAttacksOnBackground();
      });
    }


    public void HideAllTabPages()
    {
      LogCons.Inst.Write(LogLevel.Info, "Calls interface: HideAllTabPages()");
      this.minaryMain.MinaryTabPageHandler.HideAllTabPages();
    }


    public void ActivatePlugin(string pluginName)
    {
      LogCons.Inst.Write(LogLevel.Info, $"Calls interface: ActivatePlugin() \"{pluginName}\"");
      this.minaryMain.PluginHandler.ActivatePlugin(pluginName);
    }


    public void DeactivatePlugin(string pluginName)
    {
      LogCons.Inst.Write(LogLevel.Info, $"Calls interface: DeactivatePlugin() \"{pluginName}\"");
      this.minaryMain.PluginHandler.DeactivatePlugin(pluginName);
    }


    public void LoadPluginData(string tabPageName, TemplatePluginData pluginData)
    {
      LogCons.Inst.Write(LogLevel.Info, $"Calls interface: LoadPluginData() \"{tabPageName}\"");
      Minary.DataTypes.MinaryExtension realPluginObj = this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName];
      realPluginObj.PluginObject.OnLoadTemplateData(pluginData);
    }


    public void ResetAllPlugins()
    {
      LogCons.Inst.Write(LogLevel.Info, "Calls interface: ResetAllPluginsn()");
      this.minaryMain.ResetAllPlugins();
    }

    #endregion

  }
}