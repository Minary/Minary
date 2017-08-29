namespace Minary.Domain.ArpScan
{
  using Minary.DataTypes.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using PcapDotNet.Packets;
  using PcapDotNet.Packets.Arp;
  using PcapDotNet.Packets.Ethernet;
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Net;


  public class ArpScanner : IObservableArpRequest
  {

    #region MEMBERS

    private ArpScanConfig config;

    private byte[] localMacBytes = new byte[6];
    private byte[] localIpBytes = new byte[4];

    private ReadOnlyCollection<byte> localMacBytesCollection;
    private ReadOnlyCollection<byte> localIpBytesCollection;

    private List<IObserverArpRequest> observers = new List<IObserverArpRequest>();

    #endregion


    #region PUBLIC

    public ArpScanner(ArpScanConfig arpScanConfig)
    {
      char[] separators = { '-', ':', ' ', '.' };
      this.config = arpScanConfig;

      // Byte arrays
      this.localMacBytes = arpScanConfig.LocalMac.Split(separators).Select(s => Convert.ToByte(s, 16)).ToArray();
      this.localIpBytes = IPAddress.Parse(arpScanConfig.LocalIp).GetAddressBytes();

      // Byte collections
      this.localMacBytesCollection = new ReadOnlyCollection<byte>(this.localMacBytes);
      this.localIpBytesCollection = new ReadOnlyCollection<byte>(this.localIpBytes);
    }


    public void StartScanning()
    {
      int percentageCounter = 10;
      uint startIpInt = this.IpStringToInteger(this.config.NetworkStartIp);
      uint stopIpInt = this.IpStringToInteger(this.config.NetworkStopIp);
      uint totalIps = stopIpInt - startIpInt;

      if (this.VerifyAddressRange(startIpInt, stopIpInt) == false)
      {
        throw new Exception("Something is wrong with the start/stop addresses");
      }

      for (int counter = 0; counter < totalIps; counter++)
      {
        // If ARP scan was cancelled break out of the loop
        if (this.observers.Any(elem => elem.IsCancellationPending == true))
        {
          LogCons.Inst.Write($"ArpScanner.StartScanning(): Cancellation detected");
          break;
        }

        // If ARP scan has stopped break out of the loop
        if (this.observers.Any(elem => elem.IsCancellationPending == true))
        {
          LogCons.Inst.Write($"ArpScanner.StartScanning(): ARP scan process has stopped");
          break;
        }

        // Build and send ARP WhoHas packet
        uint tmpIpInt = (uint)(startIpInt + counter);

        try
        {
          Packet arpPacket = this.BuildArpWhoHasPacket(tmpIpInt);
          System.Threading.Thread.Sleep(5);
          this.config.Communicator.SendPacket(arpPacket);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write($"ArpScanner.StartScanning(): Exception occurred: {ex.Message}");
          System.Threading.Thread.Sleep(5);
        }

        // Notify observers about the progress
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

      byte[] ipAddresBytes = this.IpIntegerToByteArray(remoteIpInt);
      arpPacket.TargetProtocolAddress = new ReadOnlyCollection<byte>(ipAddresBytes);

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

    #endregion


    #region INTERFACE: IObservable

    public void AddObserver(IObserverArpRequest observer)
    {
      this.observers.Add(observer);
    }


    public void NotifyProgressBar(int progress)
    {
      this.observers.ForEach(elem => elem.UpdateProgressbar(progress));
    }

    #endregion

  }
}
