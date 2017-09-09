﻿namespace Minary.Domain.ArpScan
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
      Packet packet = null;
      PacketCommunicatorReceiveResult result;

      this.arpScanConfig.Communicator.SetFilter(this.arpScanConfig.Communicator.CreateFilter("arp and arp[6:2] = 2"));

      while (true)
      {
        System.Threading.Thread.Sleep(200);

        if (this.observers.Any(elem => elem.IsStopped == true))
        {
          continue;
        }

        // Receive and evaluate ARP response packet
        result = this.arpScanConfig.Communicator.ReceivePacket(out packet);
        if (result == PacketCommunicatorReceiveResult.Timeout)
        {
        }
        else if (result == PacketCommunicatorReceiveResult.Ok)
        {
          this.PacketHandler(packet);
        }
        else
        {
          throw new Exception("Fatal Pcap exception occurred");
        }
      }
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

      byte[] macAddrBytes = new byte[packet.Ethernet.Arp.SenderHardwareAddress.Count];
      packet.Ethernet.Arp.SenderHardwareAddress.CopyTo(macAddrBytes, 0);
      string senderMac = Common.NetworkFunctions.MacByteArrayToString(macAddrBytes);

      byte[] ipAddrBytes = new byte[packet.Ethernet.Arp.SenderProtocolAddress.Count];
      packet.Ethernet.Arp.SenderProtocolAddress.CopyTo(ipAddrBytes, 0);
      string senderIp = Common.NetworkFunctions.IpByteArrayToString(ipAddrBytes);

      SystemFound newSystem = new SystemFound() { MacAddress = senderMac, IpAddress = senderIp };
      LogCons.Inst.Write(LogLevel.Info, "ReplyListener.PacketHandler(): Found new target system {0}/{1}", senderMac, senderIp);
      SystemFound newRecord = new SystemFound(senderMac, senderIp);
      this.NotifyNewRecord(newRecord);
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