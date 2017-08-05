namespace Minary.Form.ArpScan.Main.Config
{
  using System.ComponentModel;


  public class ScanSystem : INotifyPropertyChanged
  {

    #region MEMBERS

    private string targetIP;
    private string targetMAC;
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public ScanSystem(string targetIP, string targetMAC)
    {
      this.targetIP = targetIP;
      this.targetMAC = targetMAC;
    }

    #endregion


    #region PROPERTIES

    public string TargetIP
    {
      get { return targetIP; }
      set
      {
        targetIP = value;
        NotifyPropertyChanged("TargetIP");
      }
    }

    public string TargetMAC
    {
      get { return targetMAC; }
      set
      {
        targetMAC = value;
        NotifyPropertyChanged("TargetMAC");
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyName"></param>
    private void NotifyPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion

  }
}
