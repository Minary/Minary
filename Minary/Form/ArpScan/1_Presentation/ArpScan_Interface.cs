namespace Minary.Form.ArpScan.Presentation
{
  using Minary.DataTypes.ArpScan;
  using Minary.DataTypes.Enum;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using System;
  using System.Text.RegularExpressions;


  public partial class ArpScan : IObserverArpResponse, IObserverArpCurrentIp
  {

    #region PROPERTIES

    public bool IsCancellationPending { get { return this.bgw_ArpScanSender.CancellationPending; } set { this.bgw_ArpScanSender.CancelAsync(); } }

    public bool IsStopped { get; set; } = true;

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

      if (systemData.Type != "reply")
      {
        return;
      }

      if (this.targetStringList.Contains(systemData.IpAddress) == true)
      {
        LogCons.Inst.Write(LogLevel.Debug, $"ArpScan.UpdateTextBox(): {systemData.MacAddress}/{systemData.IpAddress} already exists");
        return;
      }

      try
      {
        // Determine vendor
        string vendor = this.macVendorHandler.GetVendorByMac(systemData.MacAddress);
        if (systemData.IpAddress != this.gatewayIp && 
            systemData.IpAddress != this.localIp)
        {
          this.targetStringList.Add(systemData.IpAddress);
          this.TargetList.Add(new TargetRecord(systemData.IpAddress, systemData.MacAddress, vendor));
          LogCons.Inst.Write(LogLevel.Info, $"UpdateNewRecord(): Found new target system {systemData.MacAddress}/{systemData.IpAddress}");
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
      }
    }

    #endregion


    #region INTERFACE IObserverCurrentIp

    public delegate void UpdateCurrentIpDelegate(string currentIp);
    public void UpdateCurrentIp(string currentIp)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new UpdateCurrentIpDelegate(this.UpdateCurrentIp), new object[] { currentIp });
        return;
      }

      // Set current IP in ARP scan GUI
      if (string.IsNullOrEmpty(currentIp) ||
          Regex.Match(currentIp, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Success == false)
      {
        return;
      }

      this.tssl_CurrentIpValue.Text = currentIp;

      // Set progres bar
      this.currentArpScanConfig.StartStopCounter++;
      float percentage = (float)100/this.currentArpScanConfig.TotalSystemsToScan * this.currentArpScanConfig.StartStopCounter;
      // this.pb_ArpScan.PerformStep();
      var newPercentage = Convert.ToInt32(percentage);
      if (newPercentage > 100)
      {
          LogCons.Inst.Write(LogLevel.Debug, $"UpdateCurrentIp(): New percentage value is above 100: {newPercentage}");
          newPercentage = 100;
        }

      this.pb_ArpScan.Value = newPercentage;


    }

    #endregion

  }
}
