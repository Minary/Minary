namespace Minary.Domain.ArpScan
{
  using Minary.DataTypes.ArpScan;
  using Minary.DataTypes.Enum;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
//  using SharpPcap.Packets;
//  using SharpPcap;
  using SharpPcap.WinPcap; //Packets.Ethernet;
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Net;


  public class ArpScanner : IObservableArpRequest, IObservableArpCurrentIp
  {

    #region MEMBERS

// private static ArpScanner inst;
    private ArpScanConfig config;

    private byte[] localMacBytes = new byte[6];
    private byte[] localIpBytes = new byte[4];

    private ReadOnlyCollection<byte> localMacBytesCollection;
    private ReadOnlyCollection<byte> localIpBytesCollection;

    private List<IObserverArpRequest> observersArpRequest = new List<IObserverArpRequest>();
    private List<IObserverArpCurrentIp> observersCurrentIp = new List<IObserverArpCurrentIp>();

    #endregion


    #region PROPERTIES
    
    public ArpScanConfig Config { get; set; }
    //   private static ArpScanner Inst { get { return inst ?? (inst = new ArpScanner()); } set { } }

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
        // If ARP scan has stopped break out of the loop
        if (this.observersArpRequest.Any(elem => elem.IsCancellationPending == true))
        {
          LogCons.Inst.Write(LogLevel.Info, $"ArpScanner.StartScanning(): ARP scan process has stopped");
          break;
        }

        // Build and send ARP WhoHas packet
        uint tmpIpInt = (uint)(startIpInt + counter);

        try
        {
          Packet arpPacket = this.BuildArpWhoHasPacket(tmpIpInt);
          System.Threading.Thread.Sleep(13);
          this.config.Communicator.SendPacket(arpPacket);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"ArpScanner.StartScanning(): Exception occurred: {ex.Message}");
          System.Threading.Thread.Sleep(5);
        }

        // Notify observers about the progress
        int currentPercentage = this.CalculatePercentage(totalIps, counter);
        if (currentPercentage >= percentageCounter)
        {
          this.NotifyProgressBarArpRequest(currentPercentage);
          percentageCounter += 10;
        }

        // Notify observer about current IP
        if (tmpIpInt % 5 == 0)
        {
          var currIpStr = this.IpLongToString(tmpIpInt);
          this.NotifyProgressCurrentIp(currIpStr);
        }
      }
    }

    #endregion


    #region PRIVATE

    private int CalculatePercentage(long totalIps, int counter)
    {
      var percentage = (double)100 / totalIps * counter;
      var roundedPercentage = (int)Math.Round(percentage, MidpointRounding.AwayFromZero);

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
      var ethernetPacket = new EthernetLayer();
      ethernetPacket.EtherType = EthernetType.Arp;
      ethernetPacket.Source = new MacAddress(this.config.LocalMac);
      ethernetPacket.Destination = new MacAddress("ff:ff:ff:ff:ff:ff");

      // Build ARP layer
      var arpPacket = new ArpLayer();
      arpPacket.ProtocolType = EthernetType.IpV4;
      arpPacket.Operation = ArpOperation.Request;

      arpPacket.SenderHardwareAddress = this.localMacBytesCollection;
      arpPacket.SenderProtocolAddress = this.localIpBytesCollection;
      arpPacket.TargetHardwareAddress = new ReadOnlyCollection<byte>(new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });

      byte[] ipAddresBytes = this.IpIntegerToByteArray(remoteIpInt);
      arpPacket.TargetProtocolAddress = new ReadOnlyCollection<byte>(ipAddresBytes);

      var packet = new PacketBuilder(ethernetPacket, arpPacket);
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
      var intAddressHostOrder = (uint)System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(System.Net.IPAddress.Parse(address).GetAddressBytes(), 0));

      return intAddressHostOrder;
    }


    private long IpStringToLong(string address)
    {
      byte[] addressBytes = IPAddress.Parse(address).GetAddressBytes();
      long addressIntNetworkOrder = BitConverter.ToInt32(addressBytes, 0);
      long addressIntHostOrder = IPAddress.NetworkToHostOrder(addressIntNetworkOrder);

      return addressIntHostOrder;
    }


    public string IpLongToString(long longIP)
    {
      string ip = string.Empty;
      for (int i = 0; i< 4; i++)
      {
        int num = (int)(longIP / Math.Pow(256, (3 - i)));
        longIP = longIP - (long) (num* Math.Pow(256, (3 - i)));

        if (i == 0)
          ip = num.ToString();
        else
          ip  = ip + "." + num.ToString();
      }

      return ip;
    }

    #endregion


    #region INTERFACE: IObservableArpRequest

    public void AddObserverArpRequest(IObserverArpRequest observer)
    {
      this.observersArpRequest.Add(observer);
    }


    public void NotifyProgressBarArpRequest(int progress)
    {
      this.observersArpRequest.ForEach(elem => elem.UpdateProgressbar(progress));
    }

    #endregion


    #region INTERFACE: IObservableCurrentIP

    public void AddObserverCurrentIp(IObserverArpCurrentIp observer)
    {
      this.observersCurrentIp.Add(observer);
    }


    public void NotifyProgressCurrentIp(string currentIp)
    {
      this.observersCurrentIp.ForEach(elem => elem.UpdateCurrentIp(currentIp));
    }

    #endregion

  }
}
