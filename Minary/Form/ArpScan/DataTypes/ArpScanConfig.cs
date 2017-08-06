namespace Minary.Form.ArpScan.DataTypes
{
  using Minary.Form.ArpScan.DataTypes.Interface;
  using System;
  

  public class ArpScanConfig
  {

    #region PROPERTIES

    public string InterfaceId { get; set; }

    public string GatewayIp { get; set; }

    public string LocalIp { get; set; }

    public string NetworkStartIp { get; set; }

    public string NetworkStopIp { get; set; }

    public int MaxNumberSystemsToScan { get; set; }

    public IObserver ObserverClass { get; set; }

    public bool IsDebuggingOn { get; set; }

    #endregion

  }
}