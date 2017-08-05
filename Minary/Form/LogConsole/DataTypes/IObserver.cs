namespace Minary.LogConsole.DataTypes
{
  using System.Collections.Generic;

  public interface IObserver
  {
    void UpdateLog(List<string> newLogMessages);
  }
}
