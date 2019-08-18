namespace Minary.Form.GuiSimple.Presentation
{
  using Minary.MiniBrowser;
  using Minary.DataTypes.Enum;
  using Minary.Domain.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using MinaryLib.AttackService.Class;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows.Forms;


  public partial class GuiSimpleUserControl
  {

    #region MEMBERS
    

    #endregion


    private void GetGuiSimpleTargetList()
    {
    }


    public void WriteTargetSystems2AttackServices()
    {
      // Prepare dictionary with target systems MAC/IP to feed the  
      // AttackService config file write method
      Dictionary<string, string> currentTargetSystems = new Dictionary<string, string>();
      lock (this.targetStringList)
      {
        for (int i = 0; i < this.dgv_GuiSimple.Rows.Count; i++)
        {
          DataGridViewRow row = this.dgv_GuiSimple.Rows[i];
          var ipAddress = row.Cells["IpAddress"].Value.ToString();
          var macAddress = row.Cells["MacAddress"].Value.ToString();

          try
          {
            var attack = (bool)(row.Cells?["Attack"]?.Value ?? false);

            if (attack == true)
            {
              currentTargetSystems.Add(macAddress, ipAddress);
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show($"IP:{ipAddress}\r\nMAC:{macAddress}\r\nException: {ex.Message}\r\n{ex.StackTrace}");
          }
        }
      }

      // Pass MAC/IP dictionary to the registered AttackServices
      // where the the .targethosts file is rewritten.
      foreach (var tmpKey in this.minaryObj.MinaryAttackServiceHandler.AttackServices.Keys)
      {
        try
        {
          var fullName = this.minaryObj.MinaryAttackServiceHandler.AttackServices[tmpKey].ServiceName;
          LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl.WriteTargetSystems2AttackServices(): Writing .targethosts tmpKey:{tmpKey}, fullName:{fullName}");
          this.minaryObj.MinaryAttackServiceHandler.AttackServices[tmpKey].WriteTargetSystemsConfigFile(currentTargetSystems);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl.WriteTargetSystems2AttackServices(EXC): {0} => {1}", tmpKey, ex.Message);
          //this.SetNewAttackServiceState(tmpKey, ServiceStatus.Error);
          //throw;
        }
      }
    }


    #region EVENTS

    private void DgvGuiSimple_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex != this.dgv_GuiSimple.Columns["Attack"].Index)
      {
        return;
      }

      // Handle logging
      var targetIp = this.dgv_GuiSimple.Rows[e.RowIndex].Cells["IpAddress"].Value;
      this.dgv_GuiSimple.EndEdit();
      var startAttack = (bool)this.dgv_GuiSimple.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
      var action = startAttack == true ? "Starting" : "Stopping";

      LogCons.Inst.Write(LogLevel.Info, $"{action} attack on {targetIp}");

      // Write .targethosts file for all AttackServices
      this.WriteTargetSystems2AttackServices();
    }


    private void GuiSimpleUserControl_VisibleChanged(object sender, EventArgs e)
    {
      if (this.Visible == false ||
          this.Disposing == true)
      {
        this.bgw_ArpScanSender.CancelAsync();
        this.bgw_ArpScanListener.CancelAsync();
        this.bgw_RemoveInactiveSystems.CancelAsync();
        this.minaryObj?.MinaryAttackServiceHandler?.StopAllServices();
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/GuiSimpleUserControl_VisibleChanged: GuiSimple/ARPScan/AttackServices  stopped");

        return;
      }

      this.targetStringList.Clear();
      this.StartArpScanListener();
      this.StartArpScanSender();
      this.StartRemoveInactiveSystems();

      // Make all plugins prepare their environment before the actual attack begins.
      this.minaryObj.PrepareAttackAllPlugins();

      // After the plugins were prepared start all attack services.
      try
      {
        var currentServiceParams = this.GetCurrentServiceParamsConfig();
        this.minaryObj.StartAttackAllServices(currentServiceParams);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"GuiSimpleUserControl/BGW_ArpScanListener(EXCEPTION3): {ex.Message}");
        return;
      }

      LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/GuiSimpleUserControl_VisibleChanged: GuiSimple/ARPScan/AttackServices started");
    }
    

    private void DGV_GuiSimple_DoubleClick(object sender, EventArgs e)
    {
    }


    private void DGV_GuiSimple_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = this.dgv_GuiSimple.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          this.dgv_GuiSimple.ClearSelection();
          this.dgv_GuiSimple.Rows[hti.RowIndex].Selected = true;
          this.dgv_GuiSimple.CurrentCell = this.dgv_GuiSimple.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"GuiSimple: {ex.Message}");
        this.dgv_GuiSimple.ClearSelection();
      }
    }


    private void DGV_GuiSimple_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
      {
        return;
      }

      try
      {
        DataGridView.HitTestInfo hti = this.dgv_GuiSimple.HitTest(e.X, e.Y);
        if (hti.RowIndex >= 0)
        {
          this.cms_TargetActions.Show(this.dgv_GuiSimple, e.Location);
        }
      }
      catch
      {
      }
    }


    #region CMS Actions

    private void TSMI_OpenInMiniBrowser_Click(object sender, EventArgs e)
    {
      var hostName = string.Empty;
      var url = string.Empty;

      try
      {
        var currentIndex = this.dgv_GuiSimple.CurrentCell.RowIndex;
        hostName = this.dgv_GuiSimple.Rows[currentIndex].Cells["IpAddress"].Value.ToString();
        url = $"http://{hostName}/";
      }
      catch (ArgumentOutOfRangeException aoorex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"GuiSimpleUserControl/TSMI_OpenInMiniBrowser_Click(AOOREX): {aoorex.Message}");
        return;
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"GuiSimpleUserControl/TSMI_OpenInMiniBrowser_Click(EX): {ex.Message}");
      }

      Browser miniBrowser = new Browser(url, string.Empty, string.Empty, string.Empty);
      miniBrowser.Show();
    }

    #endregion


    #region EVENTS: BGW_ArpScanSender

    private void BGW_ArpScanSender_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        LogCons.Inst.Write(LogLevel.Error, "GuiSimpleUserControl/BGW_ArpScanSender(): Completed with error");
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/BGW_ArpScanSender(): Completed by cancellation");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl/BGW_ArpScanSender(): Completed successfully");
      }
    }


    private void BGW_ArpScanSender_DoWork(object sender, DoWorkEventArgs e)
    {
      // In an endless loop scan all MAC addresses within the 
      // subnet until the BGW is cancelled.
      int roundCounter = 0;
      int sleepSecondsOnError = 5;

      while (true)
      {
        LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl/BGW_ArpScanSender(): ARP scan round {roundCounter} started");
        if (this.bgw_ArpScanSender.CancellationPending == true)
        {
          LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl/BGW_ArpScanSender(): ARP scan cancelled");
          break;
        }

        try
        {
          var arpScanConfig = this.GetArpScanConfig();
          this.arpScanner.StartArpScan(arpScanConfig);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"GuiSimpleUserControl/BGW_ArpScanSender(EXCEPTION2.0): {ex.Message}\r\n{ex.StackTrace}");
          LogCons.Inst.Write(LogLevel.Error, $"GuiSimpleUserControl/BGW_ArpScanSender(EXCEPTION2.1): Sleeping for {sleepSecondsOnError} seconds");
          System.Threading.Thread.Sleep(sleepSecondsOnError * 1000);
        }

        LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl/BGW_ArpScanSender(): ARP scan round {roundCounter} finished");
        roundCounter++;
        System.Threading.Thread.Sleep(3000);
      }

      LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl/BGW_ArpScanSender(): ARP scan stopped after {roundCounter} rounds");
    }
    
    #endregion


    #region EVENTS: BGW_ArpScanListener

    private void BGW_ArpScanListener_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        LogCons.Inst.Write(LogLevel.Error, "GuiSimpleUserControl/BGW_ArpScanListener(): Completed with error");
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/BGW_ArpScanListener(): Completed by cancellation");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/BGW_ArpScanListener(): Completed successfully");
      }
    }


    private void BGW_ArpScanListener_DoWork(object sender, DoWorkEventArgs e)
    {
      LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/BGW_ArpScanListener(): Background worker started");


      LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/BGW_ArpScanListener(): Background worker stopped");
    }

    #endregion


    #region EVENTS: BGW_RemoveInactiveSystems

    private void BGW_RemoveInactiveSystems_DoWork(object sender, DoWorkEventArgs e)
    {
      int roundCounter = 0;

      while (true)
      {
        LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl/BGW_RemoveInactiveSystems_DoWork(): Remove loop round {roundCounter} started");
        if (this.bgw_RemoveInactiveSystems.CancellationPending == true)
        {
          LogCons.Inst.Write(LogLevel.Info, $"GuiSimpleUserControl/BGW_RemoveInactiveSystems_DoWork(): Remove loop cancelled");
          break;
        }

        //this.RemoveOutdatedRecords();
        // Do not remove them but make them not selectable again

        System.Threading.Thread.Sleep(5000);
        roundCounter++;
      }
    }


    private void BGW_RemoveInactiveSystems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        LogCons.Inst.Write(LogLevel.Error, "GuiSimpleUserControl/BGW_RemoveInactiveSystems(): Completed with error");
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/BGW_RemoveInactiveSystems(): Completed by cancellation");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/BGW_RemoveInactiveSystems(): Completed successfully");
      }
    }

    #endregion
        
    #endregion


    #region PRIVATE

    private void StartArpScanListener(Action onScanDoneCallback = null)
    {
      // Initiate ARP scan cancellation
      if (this.bgw_ArpScanListener.IsBusy == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/ArpScan: ArpScanListener is already running");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/ArpScan: ArpScanListener started");
        this.bgw_ArpScanListener.RunWorkerAsync();
      }
    }


    private void StartArpScanSender(Action onScanDoneCallback = null)
    {
      // Initiate ARP scan cancellation
      if (this.bgw_ArpScanSender.IsBusy == false)
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/ArpScan: ArpScanSender started");
        this.bgw_ArpScanSender.RunWorkerAsync();
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/ArpScan: ArpScanSender can not be started");
      }
    }
        

    private void StartRemoveInactiveSystems(Action onScanDoneCallback = null)
    {
      // Initiate ARP scan cancellation
      if (this.bgw_RemoveInactiveSystems.IsBusy == false)
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/ArpScan: StartRemoveInactiveSystems started");
        this.bgw_RemoveInactiveSystems.RunWorkerAsync();
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "GuiSimpleUserControl/ArpScan: StartRemoveInactiveSystems can not be started");
      }
    }


    private StartServiceParameters GetCurrentServiceParamsConfig()
    {
      var currentServiceParams = new StartServiceParameters()
      {
        SelectedIfcIndex = this.minaryObj.CurrentInterfaceIndex,
        SelectedIfcId = this.minaryObj.NetworkHandler.GetNetworkInterfaceIdByIndex(this.minaryObj.CurrentInterfaceIndex),
        TargetList = new Dictionary<string, string>()
      };

      return currentServiceParams;
    }


    private ArpScanConfig GetArpScanConfig()
    {
      DataTypes.Struct.MinaryConfig minaryConfig = this.minaryObj.MinaryTaskFacade.CurrentMinaryConfig;

      // Populate ArpScanConfig object with values
      var arpScanConfig = new ArpScanConfig()
      {
        InterfaceId = minaryConfig.InterfaceId,
        GatewayIp = minaryConfig.GatewayIp,
        LocalIp = minaryConfig.LocalIp,
        LocalMac = minaryConfig.LocalMac?.Replace('-', ':'),
        NetworkStartIp = minaryConfig.StartIp,
        NetworkStopIp = minaryConfig.StopIp,
        TotalSystemsToScan = 0
      };

      return arpScanConfig;
    }

    #endregion

  }
}
