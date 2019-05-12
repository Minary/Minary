namespace Minary.Form.SimpleGUI.Presentation
{
  using Minary.MiniBrowser;
  using Minary.DataTypes.Enum;
  using Minary.Domain.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using MinaryLib.AttackService.Class;
  using System;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows.Forms;


  public partial class SimpleGuiUserControl
  {

    #region MEMBERS
    
    protected ArpScanConfig arpScanConfig = null;
    private ArpScanner arpScanner = null;
    public ReplyListener replyListener = null;

    #endregion


    #region EVENTS

    private void DgvSimpleGui_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex != this.dgv_SimpleGui.Columns["Attack"].Index)
      {
        return;
      }
      
      var targetIp = this.dgv_SimpleGui.Rows[e.RowIndex].Cells["IpAddress"].Value;
      var targetMac = this.dgv_SimpleGui.Rows[e.RowIndex].Cells["MacAddress"].Value;

      this.dgv_SimpleGui.EndEdit();
      var startAttack = (bool)this.dgv_SimpleGui.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
      var action = startAttack == true ? "Starting" : "Stopping";

      LogCons.Inst.Write(LogLevel.Info, $"{action} attack on {targetIp}");      
    }


    private void SimpleGuiUserControl_VisibleChanged(object sender, EventArgs e)
    {
      if (this.Visible == true &&
          this.Disposing == false)
      {
        // Configure ARP scan object
        //this.arpScanner.Config = this.GetArpScanConfig();
        // Instanciate ARP scanner object
        try
        {
          this.arpScanConfig = this.GetArpScanConfig();
          this.arpScanner = new ArpScanner(this.arpScanConfig);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/SimpleGuiUserControl_VisibleChanged(EXCEPTION): {ex.Message}\r\n{ex.StackTrace}");
        }

        try
        {
          this.replyListener = new ReplyListener(this.arpScanConfig);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/BGW_ArpScanListener(EXCEPTION1): {ex.Message}");
          return;
        }

        try
        {
          this.replyListener.AddObserver(this);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/BGW_ArpScanListener(EXCEPTION2): {ex.Message}");
          return;
        }

        this.StartArpScanListener();
        this.StartArpScanSender();
        this.StartRemoveInactiveSystems();
        
        // Start attack services
        // First let all plugins prepare their environment before 
        // the actual attack begins.
        this.minaryObj.PrepareAttackAllPlugins();

        // After the plugins were prepared start all
        // attack services.
        var currentServiceParams = new StartServiceParameters()
        {
          SelectedIfcIndex = this.minaryObj.CurrentInterfaceIndex,
          SelectedIfcId = this.minaryObj.NetworkHandler.GetNetworkInterfaceIdByIndex(this.minaryObj.CurrentInterfaceIndex),
          TargetList = (from target in this.minaryObj.ArpScanHandler.TargetList
                        where target.Attack == true
                        select new { target.MacAddress, target.IpAddress }).
                            ToDictionary(elem => elem.MacAddress, elem => elem.IpAddress)
        };

        this.minaryObj.StartAttackAllServices(currentServiceParams);
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/SimpleGuiUserControl_VisibleChanged: SimpleGUI/ARPScan/AttackServices started");
      }
      else
      {
        this.bgw_ArpScanSender.CancelAsync();
        this.bgw_ArpScanListener.CancelAsync();
        this.bgw_RemoveInactiveSystems.CancelAsync();
        this.minaryObj?.MinaryAttackServiceHandler?.StopAllServices();
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/SimpleGuiUserControl_VisibleChanged: SimpleGUI/ARPScan/AttackServices  stopped");
      }
    }
    

    private void DGV_SimpleGui_DoubleClick(object sender, System.EventArgs e)
    {
    }


    private void DGV_SimpleGui_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = this.dgv_SimpleGui.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          this.dgv_SimpleGui.ClearSelection();
          this.dgv_SimpleGui.Rows[hti.RowIndex].Selected = true;
          this.dgv_SimpleGui.CurrentCell = this.dgv_SimpleGui.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"SimpleGUI: {ex.Message}");
        this.dgv_SimpleGui.ClearSelection();
      }
    }


    private void DGV_SimpleGui_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
      {
        return;
      }

      try
      {
        DataGridView.HitTestInfo hti = this.dgv_SimpleGui.HitTest(e.X, e.Y);
        if (hti.RowIndex >= 0)
        {
          this.cms_TargetActions.Show(this.dgv_SimpleGui, e.Location);
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
        var currentIndex = this.dgv_SimpleGui.CurrentCell.RowIndex;
        hostName = this.dgv_SimpleGui.Rows[currentIndex].Cells["IpAddress"].Value.ToString();
        url = $"http://{hostName}/";
      }
      catch (ArgumentOutOfRangeException aoorex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/TSMI_OpenInMiniBrowser_Click(AOOREX): {aoorex.Message}");
        return;
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/TSMI_OpenInMiniBrowser_Click(EX): {ex.Message}");
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
        LogCons.Inst.Write(LogLevel.Error, "SimpleGuiUserControl/BGW_ArpScanSender(): Completed with error");
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/BGW_ArpScanSender(): Completed by cancellation");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/BGW_ArpScanSender(): Completed successfully");
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
        LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/BGW_ArpScanSender(): ARP scan round {roundCounter} started");
        if (this.bgw_ArpScanSender.CancellationPending == true)
        {
          LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/BGW_ArpScanSender(): ARP scan cancelled");
          break;
        }

        try
        {
          this.arpScanner.StartScanning();
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/BGW_ArpScanSender(EXCEPTION2.0): {ex.Message}\r\n{ex.StackTrace}");
          LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/BGW_ArpScanSender(EXCEPTION2.1): Sleeping for {sleepSecondsOnError} seconds");
          System.Threading.Thread.Sleep(sleepSecondsOnError * 1000);
        }

        LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/BGW_ArpScanSender(): ARP scan round {roundCounter} finished");
        roundCounter++;
        System.Threading.Thread.Sleep(3000);
      }

      LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/BGW_ArpScanSender(): ARP scan stopped after {roundCounter} rounds");
    }
    
    #endregion


    #region EVENTS: BGW_ArpScanListener

    private void BGW_ArpScanListener_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        LogCons.Inst.Write(LogLevel.Error, "SimpleGuiUserControl/BGW_ArpScanListener(): Completed with error");
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/BGW_ArpScanListener(): Completed by cancellation");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/BGW_ArpScanListener(): Completed successfully");
      }
    }


    private void BGW_ArpScanListener_DoWork(object sender, DoWorkEventArgs e)
    {
      LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/BGW_ArpScanListener(): Background worker started");

      try
      {
        this.replyListener.StartReceivingArpPackets();
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"BGW_ArpScanListener(EXCEPTION2): {ex.Message}");
        return;
      }

      LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/BGW_ArpScanListener(): Background worker stopped");
    }

    #endregion


    #region EVENTS: BGW_RemoveInactiveSystems

    private void BGW_RemoveInactiveSystems_DoWork(object sender, DoWorkEventArgs e)
    {
      int roundCounter = 0;

      while (true)
      {
        LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/BGW_RemoveInactiveSystems_DoWork(): Remove loop round {roundCounter} started");
        if (this.bgw_RemoveInactiveSystems.CancellationPending == true)
        {
          LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/BGW_RemoveInactiveSystems_DoWork(): Remove loop cancelled");
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
        LogCons.Inst.Write(LogLevel.Error, "SimpleGuiUserControl/BGW_RemoveInactiveSystems(): Completed with error");
      }
      else if (e.Cancelled == true)
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/BGW_RemoveInactiveSystems(): Completed by cancellation");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/BGW_RemoveInactiveSystems(): Completed successfully");
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
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: ArpScanListener is already running");
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: ArpScanListener started");
        this.bgw_ArpScanListener.RunWorkerAsync();
      }
    }


    private void StartArpScanSender(Action onScanDoneCallback = null)
    {
      // Initiate ARP scan cancellation
      if (this.bgw_ArpScanSender.IsBusy == false)
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: ArpScanSender started");
        this.bgw_ArpScanSender.RunWorkerAsync();
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: ArpScanSender can not be started");
      }
    }
        

    private void StartRemoveInactiveSystems(Action onScanDoneCallback = null)
    {
      // Initiate ARP scan cancellation
      if (this.bgw_RemoveInactiveSystems.IsBusy == false)
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: StartRemoveInactiveSystems started");
        this.bgw_RemoveInactiveSystems.RunWorkerAsync();
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: StartRemoveInactiveSystems can not be started");
      }
    }


    private ArpScanConfig GetArpScanConfig()
    {
      DataTypes.Struct.MinaryConfig minaryConfig = this.minaryObj.MinaryTaskFacade.GetCurrentMinaryConfig();
      this.communicator = PcapHandler.Inst.OpenPcapDevice(this.minaryObj.CurrentInterfaceId, 1);

      // ArpScanner
      ArpScanConfig arpScanConfig = new ArpScanConfig()
      {
        InterfaceId = minaryConfig.InterfaceId,
        GatewayIp = minaryConfig.GatewayIp,
        LocalIp = minaryConfig.LocalIp,
        LocalMac = minaryConfig.LocalMac?.Replace('-', ':'),
        NetworkStartIp = minaryConfig.StartIp,
        NetworkStopIp = minaryConfig.StopIp,
        MaxNumberSystemsToScan = -1,
        ObserverClass = this,
        Communicator = this.communicator
      };

      return arpScanConfig;
    }

    #endregion

  }
}
