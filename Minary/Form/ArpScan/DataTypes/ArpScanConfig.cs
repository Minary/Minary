namespace Minary.Form.ArpScan.DataTypes
{
  using Minary.DataTypes.ArpScan;
  using PcapDotNet.Core;


  public class ArpScanConfig
  {

    #region PROPERTIES

    public string InterfaceId { get; set; }

    public string GatewayIp { get; set; }

    public string LocalIp { get; set; }

    public string LocalMac { get; set; }

    public string NetworkStartIp { get; set; }

    public string NetworkStopIp { get; set; }

    public int MaxNumberSystemsToScan { get; set; }

    public IObserverArpRequest ObserverClass { get; set; }

    public PacketCommunicator Communicator { get; set; }

    #endregion

  }
}