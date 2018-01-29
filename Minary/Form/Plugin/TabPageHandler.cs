namespace Minary.Form
{
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;
  using MinaryLib.Plugin;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;


  public class TabPageHandler : Control
  {

    #region MEMBERS

    private MinaryMain minaryMain;
    private TabControl tabControl;

    #endregion


    #region PROPERTIES

    public List<TabPage> VisibleTabPages
    {
      get { return this.tabControl.TabPages.Cast<TabPage>().Where(elem => elem.Text != "Minary").ToList(); }
      set { }
    }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tabControl"></param>
    /// <param name="minaryMain"></param>
    public TabPageHandler(TabControl tabControl, MinaryMain minaryMain)
    {
      this.tabControl = tabControl;
      this.minaryMain = minaryMain;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="newPluginObject"></param>
    /// <param name="newTabPageObject"></param>
    /// <returns></returns>
    public bool AddTabPageToCatalog(string pluginName, IPlugin newPluginObject, TabPage newTabPageObject)
    {
      if (string.IsNullOrEmpty(pluginName) || 
          newPluginObject == null || 
          newTabPageObject == null)
      {
        return false;
      }

      if (this.minaryMain.PluginHandler.TabPagesCatalog.ContainsKey(pluginName))
      {
        return false;
      }

      DataTypes.MinaryExtension newCatalogObject = new DataTypes.MinaryExtension() { PluginObject = newPluginObject, PluginTabPage = newTabPageObject };
      return this.minaryMain.PluginHandler.TabPagesCatalog.TryAdd(pluginName, newCatalogObject);
    }


    /// <summary>
    ///
    /// </summary>
    public delegate void HideAllTabPagesDelegate();
    public void HideAllTabPages()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new HideAllTabPagesDelegate(this.HideAllTabPages), new object[] { });
        return;
      }

      foreach (TabPage tmpTabPage in this.tabControl.TabPages)
      {
        try
        {
          LogCons.Inst.Write(LogLevel.Debug, $"Hiding tab page: {tmpTabPage.Name}");

          this.minaryMain.SetPluginStateByName(tmpTabPage.Text, "0");
          this.HideTabPage(tmpTabPage.Text);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"HideAllTabPages(): Error occurred while hiding tab page: {ex.Message}");
        }
      }

      // Wait untill all tabe pages are hidden
      // REMARK: Super ugly!! Maybe one day I'll fix it.
      int maxWait = 1000;
      int counter = 0;
      while (this.tabControl.TabPages.Count > 1 && 
             counter < maxWait)
      {
        System.Threading.Thread.Sleep(10);
        counter += 10;
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tabPageName"></param>
    public delegate void HideTabPageDelegate(string tabPageName);
    public void HideTabPage(string tabPageName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new HideTabPageDelegate(this.HideTabPage), new object[] { tabPageName });
        return;
      }

      if (string.IsNullOrEmpty(tabPageName) || 
          tabPageName.ToLower() == "minary")
      {
        return;
      }

      // Return if TabPage is not in catalog
      if (this.minaryMain.PluginHandler.TabPagesCatalog.ContainsKey(tabPageName) == false)
      {
        LogCons.Inst.Write(LogLevel.Error, $"{tabPageName} : Can't hide plugin as it is not found in the catalog");
        return;
      }

      try
      {
        this.minaryMain.SetPluginStateByName(tabPageName, "0");
        this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName].PluginTabPage.BeginInvoke(
          new MethodInvoker(delegate () { this.tabControl.TabPages.Remove(this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName].PluginTabPage); })
          );

        LogCons.Inst.Write(LogLevel.Info, $"{tabPageName} : Plugin tab is hidden.");
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"{tabPageName} : Error occurred while hiding tab page: {ex.Message}");
      }
    }


    /// <summary>
    ///
    /// </summary>
    public delegate void ShowAllTabPagesDelegate();
    public void ShowAllTabPages()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowAllTabPagesDelegate(this.ShowAllTabPages), new object[] { });
        return;
      }

      foreach (string key in this.minaryMain.PluginHandler.TabPagesCatalog.Keys)
      {
        if (!this.tabControl.TabPages.ContainsKey(key))
        {
          this.ShowTabPage(key);
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="tabPageName"></param>
    public delegate void ShowTabPageDelegate(string tabPageName);
    public void ShowTabPage(string tabPageName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowTabPageDelegate(this.ShowTabPage), new object[] { tabPageName });
        return;
      }

      if (string.IsNullOrEmpty(tabPageName) || 
          tabPageName.ToLower() == "minary")
      {
        return;
      }

      // Return if TabPage is not in catalog
      if (!this.minaryMain.PluginHandler.TabPagesCatalog.ContainsKey(tabPageName))
      {
        LogCons.Inst.Write(LogLevel.Error, $"Minary.ShowTabPage(): Can't display plugin \"{tabPageName}\" as it is not found in the catalog");
        return;
      }

      // Return if TabPage is already shown
      if (this.tabControl.TabPages.ContainsKey(tabPageName))
      {
        LogCons.Inst.Write(LogLevel.Info, $"Minary.ShowTabPage(): Plugin \"{tabPageName}\" is already activated and shown in the TabControl");
        return;
      }

      LogCons.Inst.Write(LogLevel.Info, "Minary.ShowTabPage(): Inserting plugin: {0}, No. loaded plugins:{1}", tabPageName, this.tabControl.TabPages.Count);
      // Insert tab page into tab control
      for (int i = 0; i < this.tabControl.TabPages.Count; i++)
      {
        try
        {
          if (this.tabControl.TabPages[i].Text.Contains("Minary") ||
              tabPageName.CompareTo(this.tabControl.TabPages[i].Text) < 0)
          {
            LogCons.Inst.Write(LogLevel.Info, $"Minary.ShowTabPage(): TRIAL {i}, Inserting plugin: {tabPageName}");
            this.tabControl.Invoke((MethodInvoker)delegate { this.tabControl.TabPages.Insert(i, this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName].PluginTabPage); });
            this.minaryMain.SetPluginStateByName(tabPageName, "1");
            //LogCons.Inst.Write(LogLevel.Info, "{0} : Displaying page in PageHandler", tabPageName);
            break;
          }
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"Minary.ShowTabPage(EXCEPTION): {tabPageName} : {ex.Message}");
        }
      }

      return;
    }

    #endregion

  }
}
