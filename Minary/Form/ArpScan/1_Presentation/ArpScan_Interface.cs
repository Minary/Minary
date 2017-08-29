namespace Minary.Form.ArpScan.Presentation
{
  using Minary.DataTypes.ArpScan;
  using Minary.LogConsole.Main;


  public partial class ArpScan : IObserverArpRequest, IObserverArpResponse
  {

    public bool IsCancellationPending { get { return this.bgw_ArpScanSender.CancellationPending; } set { this.bgw_ArpScanSender.CancelAsync(); } }

    public bool IsStopped { get { return this.isStopped; } set { } }



    #region INTERFACE: IObserverArpRequest

    public delegate void UpdateProgressBarDelegate(int progress);
    public void UpdateProgressbar(int progress)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new UpdateProgressBarDelegate(this.UpdateProgressbar), new object[] { progress });
        return;
      }

      LogCons.Inst.Write("UpdateProgressbar: NewProgress={0}", progress);
      this.pb_ArpScan.PerformStep();
    }

    #endregion


    #region INTERFACE: IObserverArpResponse

    public void UpdateNewRecord(SystemFound systemData)
    {
      LogCons.Inst.Write("UpdateNewRecord: IpAddress={0}, MacAddress={1}", systemData.IpAddress, systemData.MacAddress);
      this.UpdateTextBox(systemData);
    }

    #endregion

  }
}
