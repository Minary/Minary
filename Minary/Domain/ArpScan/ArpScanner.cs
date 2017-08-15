namespace Minary.Domain.ArpScan
{
  using Minary.DataTypes.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.Form.ArpScan.Presentation;
  using Minary.LogConsole.Main;
  using PcapDotNet.Core;
  using PcapDotNet.Packets;
  using PcapDotNet.Packets.Arp;
  using PcapDotNet.Packets.Ethernet;
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Net;


  public class ArpScanner : IObservable
  {

    #region MEMBERS

    private ArpScanConfig config;
    private ArpScan arpScanForm;

    private byte[] localMacBytes = new byte[6];
    private byte[] localIpBytes = new byte[4];

    private ReadOnlyCollection<byte> localMacBytesCollection;
    private ReadOnlyCollection<byte> localIpBytesCollection;

    private List<IObserver> observers = new List<IObserver>();

    #endregion


    #region PUBLIC

    public ArpScanner(ArpScanConfig config, ArpScan arpScanForm)
    {
      char[] separators = { '-', ':', ' ', '.' };
      this.config = config;
      this.arpScanForm = arpScanForm;

      // Byte arrays
      this.localMacBytes = config.LocalMac.Split(separators).Select(s => Convert.ToByte(s, 16)).ToArray();
      this.localIpBytes = IPAddress.Parse(config.LocalIp).GetAddressBytes();

      // Byte collections
      this.localMacBytesCollection = new ReadOnlyCollection<byte>(this.localMacBytes);
      this.localIpBytesCollection = new ReadOnlyCollection<byte>(this.localIpBytes);
    }


    public void StartScanning()
    {
      LivePacketDevice selectedDevice = this.GetPcapDevice(this.config.InterfaceId);
      PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1);
      uint startIpInt = this.IpStringToInteger(this.config.NetworkStartIp);
      uint stopIpInt = this.IpStringToInteger(this.config.NetworkStopIp);
      uint totalIps = stopIpInt - startIpInt;

      if (this.VerifyAddressRange(startIpInt, stopIpInt) == false)
      {
        throw new Exception("Something is wrong with the start/stop addresses");
      }
      int percentageCounter = 10;
      string message = string.Format($"config.NetworkStartIp={this.config.NetworkStartIp}\r\n" +
        $"config.NetworkStopIp={this.config.NetworkStopIp}\r\n" +
        $"startIpInt={startIpInt}\r\n" +
        $"stopIpInt={stopIpInt}\r\n" +
        $"totalIps={totalIps}");

      for (int counter = 0; counter < totalIps; counter++)
      {
        // Break the loop if an observer
        // has set the cancelling flag
        if (this.observers.Where(elem => elem.IsCancellationPending == true).Count() > 0)
        {
          LogCons.Inst.Write($"ArpScanner.StartScanning(): Cancelling was set");
          break;
        }

        uint tmpIpInt = (uint)(startIpInt + counter);
        LogCons.Inst.Write($"ArpScanner.StartScanning(): ArpPing to: {tmpIpInt}");

        try
        {
          Packet arpPacket = this.BuildArpWhoHasPacket(tmpIpInt);
          System.Threading.Thread.Sleep(5);
          communicator.SendPacket(arpPacket);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write($"ArpScanner.StartScanning(): Exception occurred: {ex.Message}");
          System.Threading.Thread.Sleep(5);
        }

        int currentPercentage = this.CalculatePercentage(totalIps, counter);
        if (currentPercentage >= percentageCounter)
        {
          this.NotifyProgressBar(currentPercentage);
          percentageCounter += 10;
        }
      }
    }

    #endregion


    #region PRIVATE

    private int CalculatePercentage(long totalIps, int counter)
    {
      double percentage = (double)100 / totalIps * counter;
      int roundedPercentage = (int)Math.Round(percentage, MidpointRounding.AwayFromZero);

      return roundedPercentage;
    }


    private bool VerifyAddressRange(long startIp, long endIp)
    {
      if (startIp > endIp)
      {
        throw new Exception("The start IP address is greater than the end address");
      }

      return true;
    }


    private Packet BuildArpWhoHasPacket(uint remoteIpInt)
    {
      // Build ethernet layer
      EthernetLayer ethernetPacket = new EthernetLayer();
      ethernetPacket.EtherType = EthernetType.Arp;
      ethernetPacket.Source = new MacAddress(this.config.LocalMac);
      ethernetPacket.Destination = new MacAddress("ff:ff:ff:ff:ff:ff");

      // Build ARP layer
      ArpLayer arpPacket = new ArpLayer();
      arpPacket.ProtocolType = EthernetType.IpV4;
      arpPacket.Operation = ArpOperation.Request;

      arpPacket.SenderHardwareAddress = this.localMacBytesCollection;
      arpPacket.SenderProtocolAddress = this.localIpBytesCollection;

      arpPacket.TargetHardwareAddress = new ReadOnlyCollection<byte>(new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });

var la = this.IpIntegerToByteArray(remoteIpInt);
arpPacket.TargetProtocolAddress = new ReadOnlyCollection<byte>(la);

      PacketBuilder packet = new PacketBuilder(ethernetPacket, arpPacket);
      return packet.Build(DateTime.Now);
    }


    private byte[] IpIntegerToByteArray(uint addressInt)
    {
      byte[] intBytes = BitConverter.GetBytes(addressInt);
      if (BitConverter.IsLittleEndian)
      {
        Array.Reverse(intBytes);
      }

      return intBytes;
    }


    private uint IpStringToInteger(string address)
    {
      uint intAddressHostOrder = (uint)System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(System.Net.IPAddress.Parse(address).GetAddressBytes(), 0));

      return intAddressHostOrder;
    }


    private long IpStringToLong(string address)
    {
      byte[] addressBytes = IPAddress.Parse(address).GetAddressBytes();
      long addressIntNetworkOrder = BitConverter.ToInt32(addressBytes, 0);
      long addressIntHostOrder = IPAddress.NetworkToHostOrder(addressIntNetworkOrder);

      return addressIntHostOrder;
    }


    private LivePacketDevice GetPcapDevice(string deviceId)
    {
      return LivePacketDevice.AllLocalMachine.Where(elem => elem.Name.Contains(deviceId)).First();
    }

    #endregion


    #region INTERFACE: IObservable

    public void AddObserver(IObserver observer)
    {
      this.observers.Add(observer);
    }


    public void NotifyProgressBar(int progress)
    {
      this.observers.ForEach(elem => elem.UpdateProgressbar(progress));
    }


    public void NotifyNewRecord(string inputData)
    {
      this.observers.ForEach(elem => elem.UpdateNewRecord(inputData));
    }

    #endregion

  }
}
