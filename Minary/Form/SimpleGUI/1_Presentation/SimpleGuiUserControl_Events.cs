namespace Minary.Form.SimpleGUI.Presentation
{
  using Minary.DataTypes.Enum;
  using Minary.Domain.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using System;
  using System.ComponentModel;
  using System.Windows.Forms;


  public partial class SimpleGuiUserControl
  {

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
      ArpScanConfig arpScanConfig = null;
      ArpScanner arpScanner = null;

      try
      {
        arpScanConfig = this.GetArpScanConfig();
        arpScanner = new ArpScanner(arpScanConfig);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/BGW_ArpScanSender(EXCEPTION): {ex.Message}\r\n{ex.StackTrace}");
      }      
    }


    private void DGV_SimpleGui_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    
    private void SimpleGuiUserControl_VisibleChanged(object sender, System.EventArgs e)
    {
      if (this.Visible == true && 
          this.Disposing == false)
      {
        this.StartArpScanListener();
        this.StartArpScanSender();
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: SimpleGUI started");
      }
      else
      {
        this.bgw_ArpScanSender.CancelAsync();
        this.bgw_ArpScanListener.CancelAsync();
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: SimpleGUI stopped");
      }
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
      ArpScanConfig arpScanConfig = null;
      ReplyListener replyListener = null;

      try
      {
        arpScanConfig = this.GetArpScanConfig();
        replyListener = new ReplyListener(arpScanConfig);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"SimpleGuiUserControl/BGW_ArpScanListener(EXCEPTION1): {ex.Message}");
        return;
      }

      try
      {
        replyListener.AddObserver(this);
        replyListener.StartReceivingArpPackets();
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"BGW_ArpScanListener(EXCEPTION2): {ex.Message}");
        return;
      }

      LogCons.Inst.Write(LogLevel.Info, "BGW_ArpScanListener(): Background worker is started");
    }

    #endregion


    #region PRIVATE

    public void StartArpScanListener(Action onScanDoneCallback = null)
    {
      // Initiate ARP scan cancellation
      if (this.bgw_ArpScanListener.IsBusy == false)
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: ArpScanListener started");
        this.bgw_ArpScanListener.RunWorkerAsync();
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, "SimpleGuiUserControl/ArpScan: ArpScanListener can not be started");
      }
    }


    public void StartArpScanSender(Action onScanDoneCallback = null)
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


    private ArpScanConfig GetArpScanConfig()
    {
      DataTypes.Struct.MinaryConfig minaryConfig = this.minaryObj.MinaryTaskFacade.GetCurrentMinaryConfig();
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
