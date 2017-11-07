namespace Minary.Form.ArpScan.Presentation
{
  using Minary.DataTypes.ArpScan;
  using Minary.DataTypes.Enum;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using System;
  using System.Linq;


  public partial class ArpScan : IObserverArpRequest, IObserverArpResponse
  {

    #region PROPERTIES

    public bool IsCancellationPending { get { return this.bgw_ArpScanSender.CancellationPending; } set { this.bgw_ArpScanSender.CancelAsync(); } }

    public bool IsStopped { get { return this.isStopped; } set { } }

    #endregion


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
      this.pb_ArpScan.PerformStep();
    }

    #endregion


    #region INTERFACE: IObserverArpResponse

    public delegate void UpdateNewRecordDelegate(SystemFound systemData);
    public void UpdateNewRecord(SystemFound systemData)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new UpdateNewRecordDelegate(this.UpdateNewRecord), new object[] { systemData });
        return;
      }

      if (this.targetRecords.Any(elem => elem.MacAddress == systemData.MacAddress &&
                                          elem.IpAddress == systemData.IpAddress) == true)
      {
        LogCons.Inst.Write(LogLevel.Debug, "ArpScan.UpdateTextBox(): {0}/{1} already exists", systemData.MacAddress, systemData.IpAddress);
        return;
      }

      try
      {
        // Determine vendor
        string vendor = this.macVendorHandler.GetVendorByMac(systemData.MacAddress);
        if (systemData.IpAddress != this.gatewayIp && systemData.IpAddress != this.localIp)
        {
          this.targetList.Add(systemData.IpAddress);
          this.targetRecords.Add(new TargetRecord(systemData.IpAddress, systemData.MacAddress, vendor));
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
      }
    }

    #endregion

  }
}
