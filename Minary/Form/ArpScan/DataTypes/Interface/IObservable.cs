namespace Minary.Form.ArpScan.DataTypes.Interface
{
  using System.Collections.Generic;


  public interface IObservable
  {
    void AddObserver(IObserver observer);

    void NotifyProgressBar(int progress);
    void AddRecordToDgv(string inputData);
    void ProcessStopped();
  }
}
