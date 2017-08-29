namespace Minary.DataTypes.ArpScan
{

  public interface IObservableArpRequest
  {
    void AddObserver(IObserverArpRequest observer);

    void NotifyProgressBar(int progress);
  }
}
