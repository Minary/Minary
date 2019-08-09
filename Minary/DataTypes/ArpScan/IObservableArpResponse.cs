namespace Minary.DataTypes.ArpScan
{


  public interface IObservableArpResponse
  {
    void AddObserverArpResponse(IObserverArpResponse observer);

    void NotifyArpResponseNewRecord(SystemFound systemData);
  }
}
