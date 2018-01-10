namespace Minary.Form.ArpScan.Main.Config
{
  using System.ComponentModel;


  public class ScanSystem : INotifyPropertyChanged
  {

    #region MEMBERS

    private string targetIp;
    private string targetMac;
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public ScanSystem(string targetIp, string targetMac)
    {
      this.targetIp = targetIp;
      this.targetMac = targetMac;
    }

    #endregion


    #region PROPERTIES

    public string TargetIP
    {
      get { return this.targetIp; }
      set
      {
        this.targetIp = value;
        this.NotifyPropertyChanged("TargetIP");
      }
    }

    public string TargetMAC
    {
      get { return this.targetMac; }
      set
      {
        this.targetMac = value;
        this.NotifyPropertyChanged("TargetMAC");
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
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion

  }
}
