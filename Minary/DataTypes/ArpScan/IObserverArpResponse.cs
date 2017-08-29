namespace Minary.DataTypes.ArpScan
{


  public interface IObserverArpResponse
  {
    bool IsCancellationPending { get; set; }

    bool IsStopped { get; set; }

    void UpdateNewRecord(SystemFound systemData);
  }
}
