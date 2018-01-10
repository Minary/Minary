namespace Minary.DataTypes.Struct
{


  public struct NetworkInterfaceConfig
  {

    #region PROPERTIES

    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string IpAddress { get; set; }

    public string MacAddress { get; set; }

    public string NetworkAddr { get; set; }

    public string BroadcastAddr { get; set; }

    public string DefaultGw { get; set; }

    public string GatewayMac { get; set; }

    public bool IsUp { get; set; }

    #endregion

  }
}
