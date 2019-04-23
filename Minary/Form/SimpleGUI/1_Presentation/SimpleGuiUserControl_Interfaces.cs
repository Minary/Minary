namespace Minary.Form.SimpleGUI.Presentation
{
  using Minary.DataTypes.ArpScan;
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;


  public partial class SimpleGuiUserControl: IObserverArpResponse, IObserverArpRequest
  {


    #region INTERFACE: IObserverArpRequest

    public delegate void UpdateProgressBarDelegate(int progress);
    public void UpdateProgressbar(int progress)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new UpdateProgressBarDelegate(this.UpdateProgressbar), new object[] { progress });
        return;
      }

      LogCons.Inst.Write(LogLevel.Debug, "UpdateProgressbar: NewProgress={0}", progress);
    }

    #endregion


    #region INTERFACE: IObserverArpResponse
    
    #region PROPERTIES

    public bool IsCancellationPending { get { return this.bgw_ArpScanSender.CancellationPending; } set { this.bgw_ArpScanSender.CancelAsync(); } }

    public bool IsStopped { get; set; } = false;

    #endregion


    #region PUBLIC

    public delegate void UpdateNewRecordDelegate(SystemFound systemData);
    public void UpdateNewRecord(SystemFound systemData)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new UpdateNewRecordDelegate(this.UpdateNewRecord), new object[] { systemData });
        return;
      }

      //if (this.targetStringList.Contains(systemData.IpAddress) == true)
      //{
      //  LogCons.Inst.Write(LogLevel.Debug, $"ArpScan.UpdateTextBox(): {systemData.MacAddress}/{systemData.IpAddress} already exists");
      //  return;
      //}

      //try
      //{
      //  // Determine vendor
      //  string vendor = this.macVendorHandler.GetVendorByMac(systemData.MacAddress);
      //  if (systemData.IpAddress != this.gatewayIp &&
      //      systemData.IpAddress != this.localIp)
      //  {
      //    this.targetStringList.Add(systemData.IpAddress);
      //    this.TargetList.Add(new TargetRecord(systemData.IpAddress, systemData.MacAddress, vendor));
      //    LogCons.Inst.Write(LogLevel.Info, $"UpdateNewRecord(): Found new target system {systemData.MacAddress}/{systemData.IpAddress}");
      //  }
      //}
      //catch (Exception ex)
      //{
      //  LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
      //}
    }

    #endregion

    #endregion

  }
}
