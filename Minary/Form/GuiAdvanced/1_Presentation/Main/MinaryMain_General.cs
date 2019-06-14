namespace Minary.Form.GuiAdvanced
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Struct;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
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
        this.Text = $"{Config.ApplicationName}  {Config.MinaryVersion:0.0} ({appTitle})";
      }
      else
      {
        this.Text = $"{Config.ApplicationName}  {Config.MinaryVersion:0.0}";
      }
    }


    /// <summary>
    ///
    /// </summary>
    public delegate bool LoadNicSettingsDelegate();
    public bool LoadNicSettings()
    {
      if (this.InvokeRequired == true)
      {
        this.BeginInvoke(new LoadNicSettingsDelegate(this.LoadNicSettings), new object[] { });
        return true;
      }

      var temp = string.Empty;

      // Empty Interfaces ComboBox and repopulate it with found interfaces.
      this.cb_Interfaces.Items.Clear();
      if (!NetworkInterface.GetIsNetworkAvailable())
      {
        return false;
      }

      // Return if no internal interface listing could be generated
      this.nicHandler.LoadInterfaces();
      if (this.nicHandler.Interfaces.Count <= 0)
      {
        return false;
      }

      // Dump all interfaces to the Log console
      foreach (NetworkInterfaceConfig tmpInterface in this.nicHandler.Interfaces)
      {
        if (tmpInterface.IsUp)
        {
          var interfaceData = $"Ifc descr : {tmpInterface.Description}\r\nIfc ID : {tmpInterface.Id}\r\n" +
                              $"IpAddress : {tmpInterface.IpAddress}\r\nGW IpAddress : {tmpInterface.DefaultGw}\r\n" +
                              $"GW MacAddress : {tmpInterface.GatewayMac}";
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
          LogCons.Inst.Write(LogLevel.Debug, interfaceData);
        }
      }

      // Select a default interface.
      if (this.tb_NetworkStartIp.Text.Length == 0)
      {
        this.tb_NetworkStartIp.Text = this.nicHandler.IfcByIndex(0).NetworkAddr;
      }

      if (this.tb_NetworkStopIp.Text.Length == 0)
      {
        this.tb_NetworkStopIp.Text = this.nicHandler.IfcByIndex(0).BroadcastAddr;
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
        LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
      }

      return true;
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
      var reval = 0;

      if (targetList == null || targetList.Count <= 0)
      {
        return reval;
      }

      foreach (var tmpRecord in targetList)
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
