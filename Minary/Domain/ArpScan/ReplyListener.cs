namespace Minary.Domain.ArpScan
{
  using Minary.DataTypes.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using SharpPcap;
  using System.Collections.Generic;
  using System.Linq;


  public class ReplyListener : IObservableArpResponse
  {

    #region MEMBERS

    private ArpScanConfig arpScanConfig;
    private List<IObserverArpResponse> observers = new List<IObserverArpResponse>();

    #endregion


    #region PUBLIC

    public ReplyListener(ArpScanConfig arpScanConfig)
    {
      this.arpScanConfig = arpScanConfig;
    }


    public void StartReceivingArpPackets()
    {
      this.arpScanConfig.Communicator.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
      this.arpScanConfig.Communicator.StartCapture(); //.ReceivePackets(0, this.PacketHandler);
    }

    #endregion


    #region PRIVATE

    private void device_OnPacketArrival(object sender, CaptureEventArgs e)
//    private void PacketHandler(Packet packet)
    {
      var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
      if (packet == null ||
          packet is PacketDotNet.EthernetPacket == false)
      {
        return;
      }

      var ether = ((PacketDotNet.EthernetPacket)packet);
      if (ether.Type != PacketDotNet.EthernetType.Arp)
      {
        return;
      }

      var arpPacket = packet.Extract<PacketDotNet.ArpPacket>(); // (typeof(PacketDotNet.ArpPacket));
      if (arpPacket == null ||
          arpPacket.Operation != PacketDotNet.ArpOperation.Response)
      {
          return;
      }

      
      var senderMac = string.Join("-", this.ByteToHexString(arpPacket.SenderHardwareAddress.GetAddressBytes()));
      var senderIp = new System.Net.IPAddress(arpPacket.SenderProtocolAddress.GetAddressBytes()).ToString();
      
      SystemFound newRecord = new SystemFound(senderMac, senderIp);
      this.NotifyNewRecord(newRecord);
    }


    private string[] ByteToHexString(byte[] bytes)
    {
      var rs = new List<string>();

      foreach (var b in bytes)
      {
        var hex = $"{b:x2}";
        rs.Add(hex);
      }

      return rs.ToArray();
    }

    #endregion


    #region INTERFACE: IObservableArpResponse

    public void AddObserver(IObserverArpResponse observer)
    {
      this.observers.Add(observer);
    }


    public void NotifyNewRecord(SystemFound systemData)
    {
      this.observers.ForEach(elem => elem.UpdateNewRecord(systemData));
    }

    #endregion

  }
}
