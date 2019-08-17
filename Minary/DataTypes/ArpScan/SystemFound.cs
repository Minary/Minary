namespace Minary.DataTypes.ArpScan
{

  public struct SystemFound
  {

    #region PROPERTIES

    public string MacAddress { get; set; }

    public string IpAddress { get; set; }

    public string Type { get; set; }

    #endregion


    #region PUBLIC

    public SystemFound(string macAddress, string ipAddress, string type)
    {
      this.MacAddress = macAddress;
      this.IpAddress = ipAddress;
      this.Type = type;
    }

    #endregion

  }
}
