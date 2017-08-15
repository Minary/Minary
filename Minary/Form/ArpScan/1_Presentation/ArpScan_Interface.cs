namespace Minary.Form.ArpScan.Presentation
{
  using Minary.DataTypes.ArpScan;
  using Minary.LogConsole.Main;


  public partial class ArpScan : IObserver
  {

    public bool IsCancellationPending { get { return this.bgw_ArpScan.CancellationPending; } set { this.bgw_ArpScan.CancelAsync(); } }

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


    public void UpdateNewRecord(string inputData)
    {
      LogCons.Inst.Write("UpdateNewRecord: inputData={0}", inputData);
      this.UpdateTextBox(inputData);
    }

  }
}
