namespace Minary.DataTypes.ArpScan
{


  public interface IObserver
  {
    bool IsCancellationPending { get; set; }

    void UpdateProgressbar(int progress);
    void UpdateNewRecord(string inputData);
  }
}
