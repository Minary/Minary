namespace Minary.DataTypes.ArpScan
{


  public interface IObservableArpResponse
  {
    void AddObserver(IObserverArpResponse observer);

    void NotifyNewRecord(SystemFound systemData);
  }
}
