namespace Minary.DataTypes.ArpScan
{
  public struct SystemFound
  {
    public string MacAddress;
    public string IpAddress;


    public SystemFound(string macAddress, string ipAddress)
    {
      this.MacAddress = macAddress;
      this.IpAddress = ipAddress;
    }
  }
}
