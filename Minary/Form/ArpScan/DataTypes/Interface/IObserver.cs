namespace Minary.Form.ArpScan.DataTypes.Interface
{

  public interface IObserver
  {
    void UpdateProgressbar(int progress);
    void AddRecordToDgv(string inputData);
    void ProcessStopped();
  }
}
