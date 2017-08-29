namespace Minary.DataTypes.ArpScan
{


  public interface IObserverArpRequest
  {
    bool IsCancellationPending { get; set; }

    bool IsStopped { get; set; }

    void UpdateProgressbar(int progress);
  }
}
