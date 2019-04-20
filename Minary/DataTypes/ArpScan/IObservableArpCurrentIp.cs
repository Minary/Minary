namespace Minary.DataTypes.ArpScan
{

  public interface IObservableArpCurrentIp
  {
    void AddObserverCurrentIp(IObserverArpCurrentIp observer);

    void NotifyProgressCurrentIp(string currentIp);
  }
}
