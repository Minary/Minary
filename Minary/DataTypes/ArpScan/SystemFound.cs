namespace Minary.DataTypes.ArpScan
{


  public struct SystemFound
  {

    #region PROPERTIES

    public string MacAddress { get; set; }

    public string IpAddress { get; set; }

    #endregion


    #region PUBLIC

    public SystemFound(string macAddress, string ipAddress)
    {
      this.MacAddress = macAddress;
      this.IpAddress = ipAddress;
    }

    #endregion

  }
}
