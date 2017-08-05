namespace Minary
{
  using Minary.Common.Network;
  using System;
  using System.Collections.Generic;
  using System.Net.NetworkInformation;


  public partial class Config
  {

    #region PROPERTIES

    public static NetworkInterface[] Interfaces { get; set; }

    #endregion


    #region PUBLIC

    public struct NetworkInterface
    {
      public string Id;
      public string Name;
      public string Description;
      public string IpAddress;
      public string NetworkAddr;
      public string BroadcastAddr;
      public string DefaultGw;
      public string GatewayMac;
      public bool IsUp;
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
      Interfaces = new NetworkInterface[32];

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

      foreach (NetworkInterface tmpInterface in Interfaces)
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
    public static NetworkInterface GetIfcByID(string interfaceId)
    {
      NetworkInterface retVal = new NetworkInterface();

      foreach (NetworkInterface tmpInterface in Interfaces)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("/" + tmpInterface.Id + "/" + interfaceId + "/");
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
    public static string GetNetworkInterfaceIDByIndexNumber(int index)
    {
      string retVal = string.Empty;

      if (index >= 0 && index < Interfaces.Length)
      {
        retVal = Interfaces[index].Id;
      }

      return retVal;
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="availableNetworkInterfaces"></param>
    /// <returns></returns>
    public static System.Net.NetworkInformation.NetworkInterface[] DetermineActiveInterfaces(System.Net.NetworkInformation.NetworkInterface[] availableNetworkInterfaces)
    {
      List<System.Net.NetworkInformation.NetworkInterface> activeInterfaces = new List<System.Net.NetworkInformation.NetworkInterface>();

      foreach (System.Net.NetworkInformation.NetworkInterface tmpInterface in availableNetworkInterfaces)
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
          Config.SetInterfaceInstance(ifcId, ifcName, ifcDescription, ifcIPAddress, ifcNetworkAddress, ifcBroadcastAddress, defaultGateway, gatewayMac);
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.Message);
        }
      }

      return activeInterfaces.ToArray();
    }

    #endregion

  }
}