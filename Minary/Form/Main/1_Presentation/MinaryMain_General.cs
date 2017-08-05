namespace Minary.Form
{
  using Minary.Form.ArpScan.DataTypes;
  using System;
  using System.ComponentModel;
  using System.Net.NetworkInformation;
  using System.Windows.Forms;


  public partial class MinaryMain : Form
  {

    #region PUBLIC

    /// <summary>
    ///
    /// </summary>
    /// <param name="pRowNum"></param>
    public void FlipPluginStateByRowID(int rowNum)
    {
      if (rowNum < 0)
      {
        return;
      }

      if (rowNum >= this.dgv_MainPlugins.Rows.Count)
      {
        return;
      }

      (this.dgv_MainPlugins.Rows[rowNum].Cells["Active"] as DataGridViewCheckBoxCell).Value = false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pRowNum"></param>
    public void SetPluginStateByName(string pluginName, string newValue)
    {
      if (string.IsNullOrEmpty(pluginName))
      {
        return;
      }

      if (string.IsNullOrEmpty(newValue))
      {
        return;
      }

      if (newValue != "0" && newValue != "1")
      {
        return;
      }

      foreach (DataGridViewRow tmpRow in this.dgv_MainPlugins.Rows)
      {
        if (tmpRow.Cells["PluginName"].Value.ToString() == pluginName &&
            tmpRow.Cells["Active"] is DataGridViewCheckBoxCell)
        {
          DataGridViewCheckBoxCell currentCell = (DataGridViewCheckBoxCell)tmpRow.Cells["Active"];
          (tmpRow.Cells["Active"] as DataGridViewCheckBoxCell).Value = newValue;
          break;
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="appTitle"></param>
    public void SetAppTitle(string appTitle)
    {
      if (appTitle.Length > 0)
      {
        this.Text = string.Format("{0}  {1:0.0} ({2})", Config.ApplicationName, Config.MinaryVersion, appTitle);
      }
      else
      {
        this.Text = string.Format("{0}  {1:0.0}", Config.ApplicationName, Config.MinaryVersion);
      }
    }


    /// <summary>
    ///
    /// </summary>
    public void LoadNicSettings()
    {
      string temp = string.Empty;
      NetworkInterface[] activeInterfaces;
      // Empty Interfaces ComboBox and repopulate it with found interfaces.
      this.cb_Interfaces.Items.Clear();

      if (!NetworkInterface.GetIsNetworkAvailable())
      {
        return;
      }

      try
      {
        this.allAttachednetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        activeInterfaces = Minary.Config.DetermineActiveInterfaces(this.allAttachednetworkInterfaces);
      }
      catch (NetworkInformationException niex)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(niex.StackTrace);
        return;
      }
      catch (Exception ex)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.StackTrace);
        return;
      }

      if (Config.NumInterfaces() <= 0)
      {
        return;
      }

      // Determine interface name

      // Dump all interfaces to the Log console
      foreach (Config.NetworkInterface tmpInterface in Config.Interfaces)
      {
        if (tmpInterface.IsUp)
        {
          string interfaceData = string.Format(
                                               "Ifc descr : {0}\r\nIfc ID : {1}\r\nIpAddress : {2}\r\nGW IpAddress : {3}\r\nGW MacAddress : {4}",
                                               tmpInterface.Description,
                                               tmpInterface.Id,
                                               tmpInterface.IpAddress,
                                               tmpInterface.DefaultGw,
                                               tmpInterface.GatewayMac);
          // Add interface to combo box in GUI
          if (tmpInterface.Description.Length > 50)
          {
            temp = tmpInterface.Description.Substring(0, 50) + " ...";
          }
          else
          {
            temp = tmpInterface.Description;
          }

          this.cb_Interfaces.Items.Add(temp);
          Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(interfaceData);
        }
      }

      // Select a default interface.
      if (this.tb_NetworkStartIp.Text.Length == 0)
      {
        this.tb_NetworkStartIp.Text = Config.Interfaces[0].NetworkAddr;
      }

      if (this.tb_NetworkStopIp.Text.Length == 0)
      {
        this.tb_NetworkStopIp.Text = Config.Interfaces[0].BroadcastAddr;
      }

      try
      {
        if (this.cb_Interfaces.Items.Count > 0)
        {
          this.cb_Interfaces.SelectedIndex = 0;
        }
      }
      catch (Exception ex)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.StackTrace);
      }
    }


    public void ResetAllPlugins()
    {
      this.tb_TemplateName.Text = string.Empty;
      this.tabPageHandler.HideAllTabPages();
      this.tabPageHandler.ShowAllTabPages();
      this.pluginHandler.ResetAllPlugins();
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="newTargetList"></param>
    /// <returns></returns>
    private int TargetCounter(BindingList<TargetRecord> targetList)
    {
      int reval = 0;

      if (targetList == null || targetList.Count <= 0)
      {
        return reval;
      }

      foreach (TargetRecord tmpRecord in targetList)
      {
        if (tmpRecord.Attack)
        {
          reval++;
        }
      }

      return reval;
    }

    #endregion

  }
}
