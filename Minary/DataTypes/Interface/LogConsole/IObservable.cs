namespace Minary.DataTypes.Interface.LogConsole
{
  using System.Collections.Generic;


  public interface IObservable
  {
    void AddObserver(IObserver observer);

    void Notify(List<string> newLogRecords);
  }
}
