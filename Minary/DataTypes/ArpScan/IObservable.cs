namespace Minary.DataTypes.ArpScan
{


  public interface IObservable
  {
    void AddObserver(IObserver observer);
    void NotifyProgressBar(int progress);
    void NotifyNewRecord(string inputData);
  }
}
