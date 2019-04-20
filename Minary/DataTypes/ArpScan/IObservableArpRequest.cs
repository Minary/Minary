namespace Minary.DataTypes.ArpScan
{

  public interface IObservableArpRequest
  {
    void AddObserverArpRequest(IObserverArpRequest observer);

    void NotifyProgressBarArpRequest(int progress);
  }
}
