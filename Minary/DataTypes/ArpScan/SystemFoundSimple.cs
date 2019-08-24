namespace Minary.DataTypes.ArpScan
{
  using System;
  using System.ComponentModel;


  public class SystemFoundSimple : INotifyPropertyChanged
  {

    #region MEMBERS

    private string macAddress = string.Empty;
    private string ipAddress = string.Empty;
    private DateTime lastSeen = DateTime.Now;

    public event PropertyChangedEventHandler PropertyChanged;

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


    public DateTime LastSeen
    {
      get { return this.lastSeen; }
      set
      {
        this.lastSeen = value;
        this.NotifyPropertyChanged("LastSeen");
      }
    }

    #endregion


    #region PUBLIC

    public SystemFoundSimple(string macAddress, string ipAddress)
    {
      this.MacAddress = macAddress;
      this.IpAddress = ipAddress;
      this.LastSeen = DateTime.Now;
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
