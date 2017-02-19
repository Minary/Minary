namespace Minary
{
  using Minary.Common;
  using Minary.MacVendors;
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


  public partial class MinaryMain
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
        Config.NetworkInterface interfaceStruct = Config.GetIfcByID(Config.Interfaces[this.cb_Interfaces.SelectedIndex].Id);

        if (interfaceStruct.Name != null && interfaceStruct.Name.Length > 0)
        {
          string interfaceName = interfaceStruct.Name.Length > 40 ? interfaceStruct.Name.Substring(0, 40) + " ..." : interfaceStruct.Name;
          string vendor = MacVendor.GetInstance().GetVendorByMac(interfaceStruct.GatewayMac);

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
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.StackTrace);
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

      // 0. Set the Wait cursor.
      this.Cursor = Cursors.WaitCursor;

      // 1. Shut down all plugins
      this.pluginHandler.StopAllPlugins();
      this.bgwOnStopAttack.RunWorkerAsync();

      // 2. Shut all attack services down
      // cAttackServiceHandler.ShutDown();

      // 3. Kill all APE processes : WARNING!!!  it kills also the APE Depoisoning process!!
      // Process[] lAPEProcesses;
      // if ((lAPEProcesses = Process.GetProcessesByName(Config.APEName)) != null && lAPEProcesses.Length > 0)
      // {
      //   foreach (Process proc in lAPEProcesses)
      //   {
      //     try { proc.Kill(); }
      //     catch (Exception) { }
      //   }
      // }

      // 4. Remove all static ARP entries
      ProcessStartInfo procStartInfo = new ProcessStartInfo("arp", "-d *");
      procStartInfo.WindowStyle = Debugging.IsDebuggingOn ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
      System.Diagnostics.Process procClearArpCache = new System.Diagnostics.Process();
      procClearArpCache.StartInfo = procStartInfo;
      procClearArpCache.Start();
      procClearArpCache.WaitForExit(3000);
      procClearArpCache.Close();

      // 5. Set the default cursor.
      this.Cursor = Cursors.Default;

      // 6. Sometimes process cant stop correctly and stays running. Therefore ... clack clack booom!
      // if (System.Windows.Forms.Application.MessageLoop)
      //   System.Windows.Forms.Application.Exit();
      // else
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
      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        Thread updateAvailableThread = new Thread(delegate()
        {
          if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
          {
            try
            {
              Minary.Updates.Config.UpdateData updateMetaData = Minary.Common.Updates.FetchUpdateInformationFromServer();

              if (updateMetaData.IsUpdateAvaliable == true)
              {
                Updates.FormNewVersion newVersion = new Updates.FormNewVersion();
                newVersion.TopMost = true;
                newVersion.ShowDialog();
              }
              else
              {
                Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("No new updates available.");
                MessageBox.Show("No new updates available.", "Update information", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
            }
            catch (Exception)
            {
            }
          }
        });
        updateAvailableThread.Start();
      }
      else
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Can't check for updates. Internet connection is down.");
        MessageBox.Show("Can't check for updates. Internet connection is down.", "Update information", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
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
      Minary.LogConsole.Main.LogConsole.LogInstance.ShowLogConsole();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_ScanLAN_Click(object sender, EventArgs e)
    {
      if (!Config.IsAdministrator())
      {
        MessageBox.Show("Administrator privileges are required to use this program.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
      else if (this.cb_Interfaces.SelectedIndex < 0)
      {
        MessageBox.Show("No network interface selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
      else
      {
        Minary.ArpScan.Presentation.ArpScan.ShowArpScanGui(this, Config.Interfaces[this.cb_Interfaces.SelectedIndex].Id, this.tb_NetworkStartIp.Text, this.tb_NetworkStopIp.Text, this.tb_GatewayIp.Text, ref this.targetList);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Bt_Attack_Click(object sender, EventArgs e)
    {
      try
      {
        this.StartAttacksOnBackground();
      }
      catch (Exception ex)
      {
        string message = string.Format("The following error occurred while initiating attack procedures:\r\n\r\n{0}", ex.Message);
        MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
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
        MessageBox.Show("Another instance of Minibrowser is already running", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
      Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Error occurred ({0}): {1}", sender.ToString(), e.ToString());
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
        // this.loadTemplatePresentation.StartLoadingTemplate(this.ofd_ImportSession.FileName);
        if (loadTemplatePresentation != null &&
            loadTemplatePresentation.TemplateData != null &&
            !string.IsNullOrEmpty(loadTemplatePresentation.TemplateData.TemplateConfig.Name))
        {
          this.tb_TemplateName.Text = loadTemplatePresentation.TemplateData.TemplateConfig.Name;
        }
      }
      catch (Exception ex)
      {
        string errorMessage = string.Format("An error occurred while loading template \"{0}\".\r\n\r\n{1}", Path.GetFileName(templateFileName), ex.Message);
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(errorMessage);
        MessageBox.Show(errorMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        DebugginOnToolStripMenuItem_Click(null, null);
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


    private void beepToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.tsmi_Beep.Text = string.Format("Beep (is {0})", !this.inputModule.IsBeepOn == true ? "on" : "off");
      this.inputModule.IsBeepOn = !this.inputModule.IsBeepOn;
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
    public delegate void BGW_OnStartPluginsDelegate(object sender, DoWorkEventArgs e);
    private void BGW_OnStartAllPlugins(object sender, DoWorkEventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new BGW_OnStartPluginsDelegate(this.BGW_OnStartAllPlugins), new object[] { sender, e });
        return;
      }

      // Start all plugins
      foreach (string key in this.pluginHandler.TabPagesCatalog.Keys)
      {
        try
        {
          Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.BGW_OnStartAllPlugins(): PluginName:{0}, IsPluginActive:{1}", key, this.pluginHandler.IsPluginActive(key));

          if (this.pluginHandler.IsPluginActive(key))
          {
            this.pluginHandler.TabPagesCatalog[key].PluginObject.OnStartAttack();
          }
        }
        catch (Exception ex)
        {
          Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.StackTrace);
        }
      }

      // Start all services
      ServiceParameters currentServiceParams = new ServiceParameters()
      {
        SelectedIfcIndex = this.cb_Interfaces.SelectedIndex,
        SelectedIfcId = Config.GetNetworkInterfaceIDByIndexNumber(this.cb_Interfaces.SelectedIndex)
      };

      this.attackServiceHandler.StartAllServices(currentServiceParams);
    }

    private void BGW_OnStartAttackCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (!e.Cancelled && e.Error == null)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.BGW_OnStartAttackCompleted(): Done");
      }
      else if (e.Cancelled)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.BGW_OnStartAttackCompleted(): Was cancelled");
      }
      else
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.BGW_OnStartAttackCompleted(): Erors occurred");
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void BGW_OnStopPluginsDelegate(object sender, DoWorkEventArgs e);
    private void BGW_OnStopAllPlugins(object sender, DoWorkEventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new BGW_OnStopPluginsDelegate(this.BGW_OnStopAllPlugins), new object[] { sender, e });
        return;
      }

      // Stop all plugins
      foreach (string key in this.pluginHandler.TabPagesCatalog.Keys)
      {
        try
        {
          this.pluginHandler.TabPagesCatalog[key].PluginObject.OnStopAttack();
        }
        catch (Exception ex)
        {
          Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.StackTrace);
        }
      }

      // Stop all services
      this.attackServiceHandler.StopAllServices();
    }


    private void BGW_OnStopAttackCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (!e.Cancelled && e.Error == null)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.BGW_OnStopAttackCompleted(): Done");
      }
      else if (e.Cancelled)
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.BGW_OnStopAttackCompleted(): Was cancelled");
      }
      else
      {
        Minary.LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary.BGW_OnStopAttackCompleted(): Erors occurred");
      }
    }


    private void CertAuthorityToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.caCertificateHandler.ShowDialog();
    }

    private void serverCertToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.caCertificateHandler.ShowDialog();
    }

    #endregion

  }
}
