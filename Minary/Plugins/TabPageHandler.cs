namespace Minary
{
  using MinaryLib.Plugin;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;


  public class TabPageHandler : Control
  {

    #region MEMBERS

    private MinaryMain minaryMain;
    private TabControl tabController;

    #endregion


    #region PROPERTIES

    public List<TabPage> VisibleTabPages
    {
      get { return this.tabController.TabPages.Cast<TabPage>().Where(elem => elem.Text != "Minary").ToList(); }
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
      this.tabController = tabControl;
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
      if (string.IsNullOrEmpty(pluginName) || newPluginObject == null || newTabPageObject == null)
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

      foreach (TabPage tmpTabPage in this.tabController.TabPages)
      {
        try
        {
          this.minaryMain.SetPluginStateByName(tmpTabPage.Text, "0");
          this.HideTabPage(tmpTabPage.Text);
        }
        catch (Exception ex)
        {
          Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Error occurred while hiding tab page: {0}", ex.Message);
        }
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

      if (string.IsNullOrEmpty(tabPageName) || tabPageName.ToLower() == "Minary")
      {
        return;
      }

      // Return if TabPage is not in catalog
      if (!this.minaryMain.PluginHandler.TabPagesCatalog.ContainsKey(tabPageName))
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("{0} : Can't hide plugin as it is not found in the catalog", tabPageName);
        return;
      }

      try
      {
        this.minaryMain.SetPluginStateByName(tabPageName, "0");
        this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName].PluginTabPage.BeginInvoke(
          new MethodInvoker(delegate () { this.tabController.TabPages.Remove(this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName].PluginTabPage); })
          );
        
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("{0} : Plugin tab is hidden.", tabPageName);
      }
      catch (Exception ex)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("{0} : Error occurred while hiding tab page: {1}", tabPageName, ex.Message);
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
        if (!this.tabController.TabPages.ContainsKey(key))
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

      if (string.IsNullOrEmpty(tabPageName) || tabPageName.ToLower() == "Minary")
      {
        return;
      }

      // Return if TabPage is not in catalog
      if (!this.minaryMain.PluginHandler.TabPagesCatalog.ContainsKey(tabPageName))
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("{0} : Can't display plugin as it is not found in the catalog", tabPageName);
        return;
      }

      // Return if TabPage is already shown
      if (this.tabController.TabPages.ContainsKey(tabPageName))
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("{0} : Plugin is already activated and shown in the TabControl", tabPageName);
        return;
      }

      // Insert tab page into tab control
      for (int i = 0; i < this.tabController.TabPages.Count; i++)
      {
        try
        {
          if (this.tabController.TabPages[i].Text.Contains("Minary") ||
              tabPageName.CompareTo(this.tabController.TabPages[i].Text) < 0)
          {
            this.tabController.Invoke((MethodInvoker)delegate { this.tabController.TabPages.Insert(i, this.minaryMain.PluginHandler.TabPagesCatalog[tabPageName].PluginTabPage); });
            this.minaryMain.SetPluginStateByName(tabPageName, "1");
            Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(@"{0} : Displaying page in PageHandler", tabPageName);
            break;
          }
        }
        catch (Exception ex)
        {
          Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(@"{0} : {1}", tabPageName, ex.Message);
        }
      }

      return;
    }

    #endregion

  }
}
