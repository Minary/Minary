namespace Minary.Form.ArpScan.DataTypes
{
  using System.ComponentModel;


  public class TargetRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private string ipAddress;
    private string macAddress;
    private bool attack;
    private string vendor;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public TargetRecord(string ipAddress, string macAddress, string vendor)
    {
      this.ipAddress = ipAddress;
      this.macAddress = macAddress;
      this.vendor = vendor;
      this.attack = false;
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


    public bool Attack
    {
      get { return this.attack; }
      set
      {
        this.attack = value;
        this.NotifyPropertyChanged("Attack");
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
