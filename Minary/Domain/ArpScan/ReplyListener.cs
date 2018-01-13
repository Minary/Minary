namespace Minary.Domain.ArpScan
{
  using Minary.DataTypes.ArpScan;
  using Minary.DataTypes.Enum;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using PcapDotNet.Core;
  using PcapDotNet.Packets;
  using PcapDotNet.Packets.Arp;
  using PcapDotNet.Packets.Ethernet;
  using System;
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
      // Set "arp reply" filter
      using (BerkeleyPacketFilter filter = this.arpScanConfig.Communicator.CreateFilter("arp and arp[6:2] = 2"))
      {
        this.arpScanConfig.Communicator.SetFilter(filter);
      }

      this.arpScanConfig.Communicator.ReceivePackets(0, this.PacketHandler);
    }

    #endregion


    #region PRIVATE

    private void PacketHandler(Packet packet)
    {
      if (packet == null ||
          packet.Length <= 0 ||
          packet.IsValid == false)
      {
        return;
      }

      if (packet.Ethernet.EtherType != EthernetType.Arp ||
          packet.Ethernet.Arp.Operation != ArpOperation.Reply)
      {
        return;
      }

      var senderMac = string.Join("-", this.ByteToHexString(packet.Ethernet.Arp.SenderHardwareAddress.ToArray()));
      var senderIp = new System.Net.IPAddress(packet.Ethernet.Arp.SenderProtocolAddress.ToArray()).ToString();

      LogCons.Inst.Write(LogLevel.Info, $"PacketHandler(): {senderMac}/{senderIp}");

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
