namespace Minary.DataTypes.Interface
{
  using System.Collections.Generic;

  public interface IObserver
  {
    void UpdateLog(List<string> newLogMessages);
  }
}
