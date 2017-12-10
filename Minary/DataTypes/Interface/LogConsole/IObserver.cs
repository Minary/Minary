namespace Minary.DataTypes.Interface.LogConsole
{
  using System.Collections.Generic;


  public interface IObserver
  {
    void UpdateLog(List<string> newLogMessages);
  }
}
