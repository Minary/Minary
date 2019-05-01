namespace Minary.Form.SimpleGUI.Presentation
{
  using Minary.DataTypes.ArpScan;
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;
  using System;
  using System.Linq;


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

      // Add new record line if system does not exist
      if (this.targetStringList.Where(elem => elem.IpAddress == systemData.IpAddress).ToList().Count <= 0)
      {
        try
        {
          // Determine vendor
          string vendor = this.macVendorHandler.GetVendorByMac(systemData.MacAddress);
          if (systemData.IpAddress != this.arpScanConfig.GatewayIp &&
              systemData.IpAddress != this.arpScanConfig.LocalIp)
          {
            this.targetStringList.Add(new SystemFoundSimple(systemData.MacAddress, systemData.IpAddress));
            LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/UpdateNewRecord(): Found new target system {systemData.MacAddress}/{systemData.IpAddress}");
          }
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
        }
      }
      else
      {
        LogCons.Inst.Write(LogLevel.Info, $"SimpleGuiUserControl/UpdateNewRecord(): Redetected target system {systemData.MacAddress}/{systemData.IpAddress}");
      }

      // Update "last seen" timestamp
      var rec = this.targetStringList.Where(elem => elem.MacAddress == systemData.MacAddress).FirstOrDefault();
      if (rec != null)
      {
        rec.LastSeen = DateTime.Now;
      }
    }

    #endregion

    #endregion

  }
}
