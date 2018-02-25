namespace Minary.Form
{
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Form.Updates.Presentation;
  using Minary.LogConsole.Main;
  using Minary.MiniBrowser;
  using MinaryLib.AttackService.Class;
  using MinaryLib.AttackService.Enum;
  using System;
  using System.ComponentModel;
  using System.IO;
  using System.Linq;
  using System.Net.NetworkInformation;
  using System.Windows.Forms;


  public partial class MinaryMain : IMinaryState
  {

    #region EVENTS

    private void BT_ScanLan_Click(object sender, EventArgs e)
    {
      try
      {
        this.Bt_ScanLan_Click(sender, e);
      }
      catch (Exception ex)
      {
        MessageDialog.Inst.ShowWarning("Exception occurred", ex.Message, this);
      }
    }


    private void BT_Attack_Click(object sender, EventArgs e)
    {
      try
      {
        this.Bt_Attack_Click(sender, e);
      }
      catch (Exception ex)
      {
        MessageDialog.Inst.ShowWarning("Exception occured", ex.Message, this);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CB_Interfaces_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        var interfaceStruct = this.nicHandler.IfcByIndex(this.cb_Interfaces.SelectedIndex);
        if (interfaceStruct.Name?.Length > 0)
        {
          var interfaceName = interfaceStruct.Name.Length > 40 ? interfaceStruct.Name.Substring(0, 40) + " ..." : interfaceStruct.Name;
          var vendor = this.macVendorHandler.GetVendorByMac(interfaceStruct.GatewayMac);

          if (vendor?.Length > 50)
          {
            vendor = $"{vendor.Substring(0, 50)} ...";
          }

          this.tb_GatewayIp.Text = interfaceStruct.DefaultGw;
          this.tb_GatewayMac.Text = interfaceStruct.GatewayMac;
          this.tb_Vendor.Text = vendor;
          this.tb_NetworkStartIp.Text = interfaceStruct.NetworkAddr;
          this.tb_NetworkStopIp.Text = interfaceStruct.BroadcastAddr;
          this.gb_Interfaces.Text = $"{interfaceStruct.IpAddress} / {interfaceName}";

          this.CurrentLocalIp = interfaceStruct.IpAddress;
          this.CurrentLocalMac = interfaceStruct.MacAddress;
          this.CurrentInterfaceId = interfaceStruct.Id;

          this.currentInterfaceIndex = this.cb_Interfaces.SelectedIndex;
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MinaryMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.ShutDownMinary();
    }


    /// <summary>
    /// Activate/Deactivate plugin
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Dgv_MainPlugins_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex != this.dgv_MainPlugins.Columns["Active"].Index)
      {
        return;
      }

      if (e.RowIndex < 0)
      {
        return;
      }

      var pluginName = this.dgv_MainPlugins.Rows[e.RowIndex].Cells[0].Value.ToString();
      if (this.usedPlugins[e.RowIndex].Active == "1")
      {
        this.pluginHandler.DeactivatePlugin(pluginName);
      }
      else
      {
        this.pluginHandler.ActivatePlugin(pluginName);
      }
    }


    /// <summary>
    /// Check for plugins, sniffer and main app updates
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GetUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      // Show error message if no network is available
      if (NetworkInterface.GetIsNetworkAvailable() == false)
      {
        string message = "Can't check for updates. Internet connection is down.";
        LogCons.Inst.Write(LogLevel.Warning, message);
        MessageDialog.Inst.ShowInformation("Update verification failed", message, this);

        return;
      }

      // Show update form  and check for updates
      var newVersionCheck = new FormCheckNewVersion();
//newVersionCheck.StartSearchingForUpdates();
      newVersionCheck.ShowDialog();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ShutDownMinary();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DebugginOnToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Debugging.IsDebuggingOn = !Debugging.IsDebuggingOn;
      this.tsmi_Debugging.Text = string.Format("Debugging (is {0})", Debugging.IsDebuggingOn == true ? "on" : "off");
      this.SetAppTitle(Debugging.IsDebuggingOn == true ? "Debugging" : string.Empty);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LogConsoleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LogCons.Inst.ShowLogConsole();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Minibrowser_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.miniBrowser = null;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_Minibrowser_Click(object sender, EventArgs e)
    {
      if (this.miniBrowser == null)
      {
        this.miniBrowser = new Browser(string.Empty, string.Empty, string.Empty, string.Empty);
        this.miniBrowser.FormClosed += this.Minibrowser_FormClosed;
        this.miniBrowser.Show();
      }
      else
      {
        string message = "Another instance of Minibrowser is already running";
        MessageDialog.Inst.ShowInformation("Update information", message, this);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TSMI_ResetAllPlugins_Click(object sender, EventArgs e)
    {
      this.ResetAllPlugins();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGV_MainPlugins_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      LogCons.Inst.Write(LogLevel.Error, "Error occurred ({0}): {1}", sender.ToString(), e.ToString());
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LoadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ofd_ImportSession.Filter = string.Format("Minary files (*.{0})|*.{0}", Minary.Config.MinaryFileExtension);
      this.ofd_ImportSession.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), Config.TemplatesDir);

      if (this.ofd_ImportSession.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      var templateFileName = this.ofd_ImportSession.FileName;

      try
      {
        Template.Presentation.LoadTemplate loadTemplatePresentation = new Template.Presentation.LoadTemplate(this, templateFileName);
        loadTemplatePresentation.ShowDialog();

        if (string.IsNullOrEmpty(loadTemplatePresentation?.TemplateData?.TemplateConfig.Name) == false)
        {
          this.tb_TemplateName.Text = loadTemplatePresentation.TemplateData.TemplateConfig.Name;
        }
      }
      catch (Exception ex)
      {
        string message = $"An error occurred while loading template \"{Path.GetFileName(templateFileName)}\".\r\n\r\n{ex.Message}";
        LogCons.Inst.Write(LogLevel.Error, message);
        MessageDialog.Inst.ShowWarning(string.Empty, message, this);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == (Keys.Control | Keys.D))
      {
        this.DebugginOnToolStripMenuItem_Click(null, null);
        return true;
      }

      return base.ProcessCmdKey(ref msg, keyData);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SearchNetworkInterfacesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      // If no interface was found reset current interface selection
      this.SetMinaryState();

      // When NIC settings cannot be loaded (due to disconnected NIC adapter)
      // clear network settings in the GUI
      if (this.LoadNicSettings() == false)
      {
        this.ClearCurrentNetworkState();
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var createTemplateView = new Template.Presentation.CreateTemplate(this);
      createTemplateView.ShowDialog();
    }


    private void UnloadTemplateToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.tb_TemplateName.Text = string.Empty;
      this.templateTaskLayer.UnloadTemplatePatternsFromPlugins();
    }


    private void BeepToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.tsmi_Beep.Text = string.Format("Beep (is {0})", !this.inputProcessorHandler.IsBeepOn == true ? "on" : "off");
      this.inputProcessorHandler.IsBeepOn = !this.inputProcessorHandler.IsBeepOn;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MinaryMain_Shown(object sender, EventArgs e)
    {
      // Verify whether system state is intact.
      var minaryState = MinaryState.StateOk;
      try
      {
        Minary.Domain.Main.SystemStateCheck.EvaluateMinaryState(out minaryState);
      }
      catch (Exception ex)
      {
        if ((minaryState & MinaryState.WinPcapMissing) == MinaryState.WinPcapMissing)
        {
          var pcapMissing = new FormWinPcapMissing();
          pcapMissing.ShowDialog();
        }
        else
        {
          var message = $"The following error occurred ({ex.Message}):\r\n\r\n{minaryState}";
          this.LogAndShowMessage(message, LogLevel.Error);
        }

        return;
      }

      // Import and load session/template file
      if (this.commandLineArguments?.Length > 0)
      {
        this.LoadUserTemplate(this.commandLineArguments[0]);
      }
      else
      {
        // Hide all plugins what have the status "off"
        this.pluginHandler.RestoreLastPluginLoadState();
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BGW_OnStartAttack(object sender, DoWorkEventArgs e)
    {
      var currentServiceParams = new StartServiceParameters()
        {
          SelectedIfcIndex = this.currentInterfaceIndex,
          SelectedIfcId = this.nicHandler.GetNetworkInterfaceIdByIndex(this.currentInterfaceIndex),
          TargetList = (from target in this.arpScanHandler.TargetList
                        where target.Attack == true
                        select new { target.MacAddress, target.IpAddress }).
                          ToDictionary(elem => elem.MacAddress, elem => elem.IpAddress)
        };

      this.StartAllServices(currentServiceParams);
    }


    private void BGW_OnStartAttackCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        this.StopAttack();
        var message = $"The following error occurred while starting attack services: \r\n\r\n{e.Error.Message}";
        LogCons.Inst.Write(LogLevel.Error, $"Minary.BGW_OnStartAttackCompleted(): {message}");
        MessageDialog.Inst.ShowWarning("Attack services", message, this);
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "Minary.BGW_OnStartAttackCompleted(): Was cancelled");
        this.StopAttack();
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "Minary.BGW_OnStartAttackCompleted(): Procedure has completed successfully");

        // Disable all GUI elements
        Utils.TryExecute2(this.DisableGuiElements);

        // Start all plugins
        Utils.TryExecute2(this.StartAllPlugins);
        this.attackStarted = true;
      }

      this.Cursor = Cursors.Default;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void StopAttack()
    {
      // Enable GUI elements
      Utils.TryExecute2(this.EnableGuiElements);

      // Stop all plugins
      Utils.TryExecute2(this.pluginHandler.StopAllPlugins);

      // Stop all services
      Utils.TryExecute2(this.attackServiceHandler.StopAllServices);

      this.attackStarted = false;
    }
 

    private void CertAuthorityToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.caCertificateHandler.ShowDialog();
    }


    private void ServerCertToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.caCertificateHandler.ShowDialog();
    }


    private void StartAllPlugins()
    {
      foreach (var key in this.pluginHandler.TabPagesCatalog.Keys)
      {
        LogCons.Inst.Write(LogLevel.Info, $"Minary.StartAllPlugins(): PluginName:{key}, IsPluginActive:{this.pluginHandler.IsPluginActive(key)}");

        try
        {
          if (this.pluginHandler.IsPluginActive(key))
          {
            this.pluginHandler.TabPagesCatalog[key].PluginObject.OnStartAttack();
          }
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, "Minary.StartAllPlugins(EXCEPTION): PluginName:{0}, Error:{1}\r\n{2}", key, ex.Message, ex.StackTrace);
        }
      }
    }


    private void StartAllServices(StartServiceParameters serviceParameters)
    {
      foreach (var tmpKey in this.attackServiceHandler.AttackServices.Keys)
      {
        try
        {
          LogCons.Inst.Write(LogLevel.Info, "Minary.StartAllServices(): Starting {0}/{1}", tmpKey, this.attackServiceHandler.AttackServices[tmpKey].ServiceName);
          ServiceStatus newServiceStatus = this.attackServiceHandler.AttackServices[tmpKey].StartService(serviceParameters);
          this.SetNewAttackServiceState(tmpKey, newServiceStatus);
        }
        catch
        {
          this.SetNewAttackServiceState(tmpKey, ServiceStatus.Error);
          throw;
        }
      }
    }

    #endregion

  }
}
