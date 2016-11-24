namespace Minary.ArpScan.DataTypes
{
  using System.ComponentModel;

  public class TargetRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private string ipAddress;
    private string macAddress;
    private bool status;
    private string vendor;
    private string lastScanDate;
    private string note;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public TargetRecord(string ipAddress, string macAddress, string vendor, string lastScanDate, string note)
    {
      this.ipAddress = ipAddress;
      this.macAddress = macAddress;
      this.vendor = vendor;
      this.status = false;
      this.lastScanDate = lastScanDate;
      this.note = note;
    }

    #endregion


    #region PROPERTIES

    public string IpAddress
    {
      get { return this.ipAddress; }
      set
      {
        this.ipAddress = value;
        this.NotifyPropertyChanged("IpAddress");
      }
    }


    public string MacAddress
    {
      get { return this.macAddress; }
      set
      {
        this.macAddress = value;
        this.NotifyPropertyChanged("MacAddress");
      }
    }


    public string Vendor
    {
      get { return this.vendor; }
      set
      {
        this.vendor = value;
      }
    }



    public bool Status
    {
      get { return this.status; }
      set
      {
        this.status = value;
        this.NotifyPropertyChanged("Status");
      }
    }


    public string LastScanDate
    {
      get { return this.lastScanDate; }
      set
      {
        this.lastScanDate = value;
      }
    }


    public string Note
    {
      get { return this.note; }
      set
      {
        this.note = value;
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyName"></param>
    private void NotifyPropertyChanged(string name)
    {
      if (PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }

    #endregion

  }
}
