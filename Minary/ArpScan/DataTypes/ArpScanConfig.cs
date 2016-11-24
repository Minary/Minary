namespace Minary.ArpScan.DataTypes
{
  using System;

  public delegate void OnDataCallback(string data);

  public class ArpScanConfig
  {

    #region PROPERTIES

    public string InterfaceId { get; set; }

    public string GatewayIp { get; set; }

    public string LocalIp { get; set; }

    public string NetworkStartIp { get; set; }

    public string NetworkStopIp { get; set; }

    public int MaxNumberSystemsToScan { get; set; }

    public Action OnArpScanStopped { get; set; }

    public OnDataCallback OnDataReceived { get; set; }

    public bool IsDebuggingOn { get; set; }

    #endregion

  }
}