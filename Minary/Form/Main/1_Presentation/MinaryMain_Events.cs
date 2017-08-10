namespace Minary.Form
{
  using Minary.Common;
  using Minary.DataTypes.Interface;
  using Minary.DataTypes.Struct;
  using Minary.LogConsole.Main;
  using Minary.MiniBrowser;
  using MinaryLib.AttackService;
  using System;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.IO;
  using System.Linq;
  using System.Net.NetworkInformation;
  using System.Threading;
  using System.Windows.Forms;


  public partial class MinaryMain : IMinaryState
  {

    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CB_Interfaces_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        NetworkInterfaceConfig interfaceStruct = NetworkFunctions.GetIfcById(NetworkFunctions.Interfaces[this.cb_Interfaces.SelectedIndex].Id);

        if (interfaceStruct.Name != null && interfaceStruct.Name.Length > 0)
        {
          string interfaceName = interfaceStruct.Name.Length > 40 ? interfaceStruct.Name.Substring(0, 40) + " ..." : interfaceStruct.Name;
          string vendor = this.macVendorHandler.GetVendorByMac(interfaceStruct.GatewayMac);

          if (vendor != null && vendor.Length > 50)
          {
            vendor = string.Format("{0} ...", vendor.Substring(0, 50));
          }

          this.tb_GatewayIp.Text = interfaceStruct.DefaultGw;
          this.tb_GatewayMac.Text = interfaceStruct.GatewayMac;
          this.tb_Vendor.Text = vendor;
          this.tb_NetworkStartIp.Text = interfaceStruct.NetworkAddr;
          this.tb_NetworkStopIp.Text = interfaceStruct.BroadcastAddr;
          this.gb_Interfaces.Text = string.Format("{0} / {1}", interfaceStruct.IpAddress, interfaceName);

          this.currentIpAddress = interfaceStruct.IpAddress;
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(ex.StackTrace);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MinaryMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      /*
       * 1. Stop data input thread (named pipe)
       * 2. Stop poisoning thread
       * 3. Stop sniffing thread
       * 4. Shut down all plugins.
       *
       */

      // Set the Wait cursor.
      this.Cursor = Cursors.WaitCursor;

      // Shut down all plugins
      if (!this.bgwOnStopAttack.IsBusy)
      {
        this.bgwOnStopAttack.RunWorkerAsync();
      }

      // Remove all static ARP entries
      ProcessStartInfo procStartInfo = new ProcessStartInfo("arp", "-d *");
      procStartInfo.WindowStyle = Debugging.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      Process procClearArpCache = new Process();
      procClearArpCache.StartInfo = procStartInfo;
      procClearArpCache.Start();
      procClearArpCache.WaitForExit(3000);
      procClearArpCache.Close();

      // Set the default cursor.
      this.Cursor = Cursors.Default;

      System.Environment.Exit(0);
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

      string pluginName = string.Empty;
      pluginName = this.dgv_MainPlugins.Rows[e.RowIndex].Cells[0].Value.ToString();

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
      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up) == false)
      {
        string message = "Can't check for updates. Internet connection is down.";
        LogCons.Inst.Write(message);
        MessageDialog.ShowInformation(string.Empty, message, this);

        return;
      }

      Thread updateAvailableThread = new Thread(delegate ()
      {
        if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up) == false)
        {
          return;
        }

        try
        {
          Minary.Form.Updates.Config.UpdateData updateMetaData = Minary.Common.Updates.FetchUpdateInformationFromServer();

          if (updateMetaData.IsUpdateAvaliable == true)
          {
            Updates.FormNewVersion newVersion = new Updates.FormNewVersion();
            newVersion.TopMost = true;
            newVersion.ShowDialog();
          }
          else
          {
            string message = "No new updates available.";
            LogCons.Inst.Write(message);
            MessageDialog.ShowInformation("Update information", message, this);
          }
        }
        catch (Exception)
        {
        }
      });

      updateAvailableThread.Start();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.MinaryMain_FormClosing(null, null);
      base.Dispose();
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
        MessageDialog.ShowInformation("Update information", message, this);
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
      LogCons.Inst.Write("Error occurred ({0}): {1}", sender.ToString(), e.ToString());
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

      string templateFileName = this.ofd_ImportSession.FileName;

      try
      {
        Template.Presentation.LoadTemplate loadTemplatePresentation = new Template.Presentation.LoadTemplate(this, templateFileName);
        loadTemplatePresentation.ShowDialog();

        if (loadTemplatePresentation != null &&
            loadTemplatePresentation.TemplateData != null &&
            !string.IsNullOrEmpty(loadTemplatePresentation.TemplateData.TemplateConfig.Name))
        {
          this.tb_TemplateName.Text = loadTemplatePresentation.TemplateData.TemplateConfig.Name;
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("An error occurred while loading template \"{0}\".\r\n\r\n{1}", Path.GetFileName(templateFileName), ex.Message);
        LogCons.Inst.Write(message);
        MessageDialog.ShowWarning(string.Empty, message, this);
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
      this.LoadNicSettings();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Template.Presentation.CreateTemplate createTemplateView = new Template.Presentation.CreateTemplate(this);
      createTemplateView.ShowDialog();
    }


    private void UnloadToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.tb_TemplateName.Text = string.Empty;
      this.templateTaskLayer.RemoveAllTemplatePatternsFromPlugins();
    }


    private void BeepToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.tsmi_Beep.Text = string.Format("Beep (is {0})", !this.inputModuleHandler.IsBeepOn == true ? "on" : "off");
      this.inputModuleHandler.IsBeepOn = !this.inputModuleHandler.IsBeepOn;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MinaryMain_Shown(object sender, EventArgs e)
    {
      // Import and load session/template file
      if (this.CommandLineArguments != null && this.CommandLineArguments.Length > 0)
      {
        this.LoadUserTemplate(this.CommandLineArguments[0]);
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
    public delegate void BGW_OnStartAttackDelegate(object sender, DoWorkEventArgs e);
    private void BGW_OnStartAttack(object sender, DoWorkEventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new BGW_OnStartAttackDelegate(this.BGW_OnStartAttack), new object[] { sender, e });
        return;
      }

      e.Result = false;
      this.Cursor = Cursors.WaitCursor;

      // Disable all GUI elements
      Utils.TryExecute2(this.DisableGuiElements);

      // Start all plugins
      Utils.TryExecute2(this.StartAllPlugins);

      // Start all services
      ServiceParameters currentServiceParams = new ServiceParameters()
      {
        SelectedIfcIndex = this.cb_Interfaces.SelectedIndex,
        SelectedIfcId = NetworkFunctions.GetNetworkInterfaceIdByIndexNumber(this.cb_Interfaces.SelectedIndex),
        TargetList = (from target in this.arpScanHandler.TargetList
                      where target.Attack == true
                      select new { target.MacAddress, target.IpAddress }).
                        ToDictionary(elem => elem.MacAddress, elem => elem.IpAddress)
      };

      try
      {
        this.StartAllServices(currentServiceParams);

        // NOTE: The "Completed" method does not receive the
        //       caught exceptions. The outcome has to be done
        //       via return value  :/
        e.Result = true;
      }
      catch
      {
      }
    }


    private void BGW_OnStartAttackCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
// bool retVal = (bool)e.Result;

      this.attackStarted = true;

      if (e.Error != null)
      {
        LogCons.Inst.Write("Minary.BGW_OnStartAttackCompleted(): EXCEPTION: {0}\r\n\r\n{1}", e.Error.Message, e.Error.StackTrace);
        this.bgwOnStopAttack.RunWorkerAsync();
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write("Minary.BGW_OnStartAttackCompleted(): Was cancelled");
        this.bgwOnStopAttack.RunWorkerAsync();
      }
      //else if (retVal == false)
      //{
      //  LogCons.Inst.Write("Minary.BGW_OnStartAttackCompleted(): Procedure has completed unsuccessfully");
      //  this.bgwOnStopAttack.RunWorkerAsync();
      //}
      //else if (retVal == true)
      else
      {
        LogCons.Inst.Write("Minary.BGW_OnStartAttackCompleted(): Procedure has completed successfully (bgwOnStopAttack==null:{0})", this.bgwOnStopAttack == null);
      }

      this.Cursor = Cursors.Default;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void BGW_OnStopAttackDelegate(object sender, DoWorkEventArgs e);
    private void BGW_OnStopAttack(object sender, DoWorkEventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new BGW_OnStopAttackDelegate(this.BGW_OnStopAttack), new object[] { sender, e });
        return;
      }

      this.Cursor = Cursors.WaitCursor;

      // Enable GUI elements
      Utils.TryExecute2(this.EnableGuiElements);

      // Stop all plugins
      Utils.TryExecute2(this.pluginHandler.StopAllPlugins);

      // Stop all services
      Utils.TryExecute2(this.attackServiceHandler.StopAllServices);
    }


    private void BGW_OnStopAttackCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        LogCons.Inst.Write("Minary.BGW_OnStopAttackCompleted(): EXCEPTION: {0}\r\n\r\n{1}", e.Error.Message, e.Error.StackTrace);
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write("Minary.BGW_OnStopAttackCompleted(): Was cancelled");
      }
      else
      {
        LogCons.Inst.Write("Minary.BGW_OnStopAttackCompleted(): Procedure completed");
      }

      this.attackStarted = false;
      this.Cursor = Cursors.Default;
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
      foreach (string key in this.pluginHandler.TabPagesCatalog.Keys)
      {
        LogCons.Inst.Write("Minary.BGW_OnStartAllPlugins(): PluginName:{0}, IsPluginActive:{1}", key, this.pluginHandler.IsPluginActive(key));

        try
        {
          if (this.pluginHandler.IsPluginActive(key))
          {
            this.pluginHandler.TabPagesCatalog[key].PluginObject.OnStartAttack();
          }
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(ex.StackTrace);
        }
      }
    }


    private void StartAllServices(ServiceParameters serviceParameters)
    {
      foreach (string tmpKey in this.attackServiceHandler.AttackServices.Keys)
      {
        try
        {
          LogCons.Inst.Write("Minary.StartAllServices(): Starting {0}/{1}", tmpKey, this.attackServiceHandler.AttackServices[tmpKey].ServiceName);
          ServiceStatus newServiceStatus = this.attackServiceHandler.AttackServices[tmpKey].StartService(serviceParameters);
          this.SetNewAttackServiceState(tmpKey, newServiceStatus);
        }
        catch (Exception ex)
        {
          this.SetNewAttackServiceState(tmpKey, ServiceStatus.Error);
          throw;
        }
      }
    }

    #endregion

  }
}
