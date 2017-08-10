namespace Minary.Common
{
  using Minary.DataTypes.Struct;
  using Minary.LogConsole.Main;
  using System;
  using System.Collections.Generic;
  using System.Net;
  using System.Net.NetworkInformation;
  using System.Runtime.InteropServices;


  public static class NetworkFunctions
  {

    #region IMPORTS

    [DllImport("iphlpapi.dll", ExactSpelling = true)]
    private static extern int SendARP(uint destIP, uint srcIP, byte[] macAddress, ref uint macAddressLength);

    #endregion


    #region PROPERTIES

    public static NetworkInterfaceConfig[] Interfaces { get; set; }

    #endregion


    #region PUBLIC

    /// <summary>
    ///
    /// </summary>
    /// <param name="address"></param>
    /// <param name="subnetMask"></param>
    /// <returns></returns>
    public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
    {
      byte[] ipAdressBytes = address.GetAddressBytes();
      byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

      if (ipAdressBytes.Length != subnetMaskBytes.Length)
      {
        throw new ArgumentException("Lengths of IpAddress address and subnet mask do not match.");
      }

      byte[] broadcastAddress = new byte[ipAdressBytes.Length];
      for (int i = 0; i < broadcastAddress.Length; i++)
      {
        broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
      }

      return new IPAddress(broadcastAddress);
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="address"></param>
    /// <param name="subnetMask"></param>
    /// <returns></returns>
    public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
    {
      byte[] ipAdressBytes = address.GetAddressBytes();
      byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

      if (ipAdressBytes.Length != subnetMaskBytes.Length)
      {
        throw new ArgumentException("Lengths of IpAddress address and subnet mask do not match.");
      }

      byte[] broadcastAddress = new byte[ipAdressBytes.Length];
      for (int i = 0; i < broadcastAddress.Length; i++)
      {
        broadcastAddress[i] = (byte)(ipAdressBytes[i] & subnetMaskBytes[i]);
      }

      return new IPAddress(broadcastAddress);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    public static string GetMacByIp(string ipAddress)
    {
      string retVal = string.Empty;
      IPAddress remoteIpAddress;
      byte[] mac = new byte[6];
      uint len = (uint)mac.Length;
      byte[] addressBytes;
      uint dest;

      if (string.IsNullOrEmpty(ipAddress) || ipAddress == "0.0.0.0")
      {
        throw new Exception(string.Format("GetMACFromIP(): \"{0}\" is an invalid IpAddress address", ipAddress));
      }

      remoteIpAddress = IPAddress.Parse(ipAddress);

      addressBytes = remoteIpAddress.GetAddressBytes();
      dest = ((uint)addressBytes[3] << 24)
           + ((uint)addressBytes[2] << 16)
           + ((uint)addressBytes[1] << 8)
           + ((uint)addressBytes[0]);

      if (SendARP(dest, 0, mac, ref len) != 0)
      {
        throw new Exception(string.Format("GetMACFromIP(): The ARP request for {0} failed.", ipAddress));
      }

      retVal = string.Format("{0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}", mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);

      return retVal;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="portNo"></param>
    /// <returns></returns>
    public static bool IsPortAvailable(int portNo)
    {
      if (portNo <= 0 || portNo > 65535)
      {
        throw new Exception("The port is invalid");
      }

      bool isPortAvailable = true;
      IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
      IPEndPoint[] ipEndPoints = ipGlobalProperties.GetActiveTcpListeners();

      foreach (IPEndPoint endPoint in ipEndPoints)
      {
        if (endPoint.Port == portNo)
        {
          isPortAvailable = false;
          break;
        }
      }

      return isPortAvailable;
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="descr"></param>
    /// <param name="ipAddress"></param>
    /// <param name="neworkAddr"></param>
    /// <param name="broadcastAddress"></param>
    /// <param name="defaultGW"></param>
    /// <param name="gatewayMac"></param>
    public static void SetInterfaceInstance(string id, string name, string descr, string ipAddress, string neworkAddr, string broadcastAddress, string defaultGW, string gatewayMac)
    {
      Interfaces = new NetworkInterfaceConfig[32];

      for (int i = 0; i < Interfaces.Length; i++)
      {
        if (Interfaces[i].IsUp != true)
        {
          Interfaces[i].IsUp = true;
          Interfaces[i].Id = id;
          Interfaces[i].Name = name;
          Interfaces[i].Description = descr;
          Interfaces[i].IpAddress = ipAddress;
          Interfaces[i].BroadcastAddr = broadcastAddress;
          Interfaces[i].NetworkAddr = neworkAddr;
          Interfaces[i].DefaultGw = defaultGW;
          Interfaces[i].GatewayMac = gatewayMac;

          break;
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static int NumInterfaces()
    {
      int numberInterfaces = 0;

      if (Interfaces == null)
      {
        return 0;
      }

      foreach (NetworkInterfaceConfig tmpInterface in Interfaces)
      {
        if (tmpInterface.IsUp == false)
        {
          numberInterfaces++;
        }
      }

      return numberInterfaces;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="interfaceId"></param>
    /// <returns></returns>
    public static NetworkInterfaceConfig GetIfcById(string interfaceId)
    {
      NetworkInterfaceConfig retVal = new NetworkInterfaceConfig();

      foreach (NetworkInterfaceConfig tmpInterface in Interfaces)
      {
        LogCons.Inst.Write("/" + tmpInterface.Id + "/" + interfaceId + "/");
        if (tmpInterface.Id == interfaceId)
        {
          retVal = tmpInterface;
          break;
        }
      }

      return retVal;
    }
    

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetNetworkInterfaceIdByIndexNumber(int index)
    {
      string retVal = string.Empty;

      if (index >= 0 && index < Interfaces.Length)
      {
        retVal = Interfaces[index].Id;
      }

      return retVal;
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="availableNetworkInterfaces"></param>
    /// <returns></returns>
    public static NetworkInterface[] DetermineActiveInterfaces(NetworkInterface[] availableNetworkInterfaces)
    {
      List<NetworkInterface> activeInterfaces = new List<NetworkInterface>();

      foreach (NetworkInterface tmpInterface in availableNetworkInterfaces)
      {
        if (tmpInterface.OperationalStatus != OperationalStatus.Up)
        {
          continue;
        }

        if (tmpInterface.GetIPProperties() == null ||
            tmpInterface.GetIPProperties().UnicastAddresses.Count <= 0)
        {
          continue;
        }

        UnicastIPAddressInformation ipAddress = null;
        // Find entry with valid IPv4 address
        foreach (UnicastIPAddressInformation tmpIPaddr in tmpInterface.GetIPProperties().UnicastAddresses)
        {
          if (tmpIPaddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
          {
            ipAddress = tmpIPaddr;
            break;
          }
        }

        // Continue if no valid IP address and netmask is found
        if (ipAddress == null || ipAddress.IPv4Mask == null)
        {
          continue;
        }

        // Put found interface with details on the interface batch
        try
        {
          string ifcBroadcastAddress = NetworkFunctions.GetBroadcastAddress(ipAddress.Address, ipAddress.IPv4Mask).ToString();
          string ifcNetworkAddress = NetworkFunctions.GetNetworkAddress(ipAddress.Address, ipAddress.IPv4Mask).ToString();
          string ifcDescription = tmpInterface.Description;
          string ifcId = tmpInterface.Id;
          string ifcIPAddress = ipAddress.Address.ToString();
          string ifcName = tmpInterface.Name;

          string defaultGateway = string.Empty;

          if (tmpInterface.GetIPProperties().GatewayAddresses.Count > 0)
          {
            foreach (GatewayIPAddressInformation tmpAddress in tmpInterface.GetIPProperties().GatewayAddresses)
            {
              if (!tmpAddress.Address.IsIPv6LinkLocal)
              {
                defaultGateway = tmpAddress.Address.ToString();
                break;
              }
            }
          }

          string gatewayMac = NetworkFunctions.GetMacByIp(defaultGateway);
          SetInterfaceInstance(ifcId, ifcName, ifcDescription, ifcIPAddress, ifcNetworkAddress, ifcBroadcastAddress, defaultGateway, gatewayMac);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(ex.Message);
        }
      }

      return activeInterfaces.ToArray();
    }

    #endregion

  }
}