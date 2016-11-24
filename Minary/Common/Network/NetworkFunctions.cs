namespace Minary.Common.Network
{
  using System;
  using System.Net;
  using System.Runtime.InteropServices;

  public static class NetworkFunctions
  {

    #region IMPORTS

    [DllImport("iphlpapi.dll", ExactSpelling = true)]
    private static extern int SendARP(uint destIP, uint srcIP, byte[] macAddress, ref uint macAddressLength);

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
    public static string GetMACFromIP(string ipAddress)
    {
      string retVal = string.Empty;
      IPAddress remoteIpAddress;
      byte[] mac = new byte[6];
      uint len = (uint)mac.Length;
      byte[] addressBytes;
      uint dest;

      if (string.IsNullOrEmpty(ipAddress) || ipAddress == "0.0.0.0")
      {
        throw new Exception(string.Format("GetMACFromIP() : \"{0}\" is an invalid IpAddress address", ipAddress));
      }

      remoteIpAddress = IPAddress.Parse(ipAddress);

      addressBytes = remoteIpAddress.GetAddressBytes();
      dest = ((uint)addressBytes[3] << 24)
           + ((uint)addressBytes[2] << 16)
           + ((uint)addressBytes[1] << 8)
           + ((uint)addressBytes[0]);

      if (SendARP(dest, 0, mac, ref len) != 0)
      {
        throw new Exception(string.Format("GetMACFromIP() : The ARP request for {0} failed.", ipAddress));
      }

      retVal = string.Format("{0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}", mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);

      return retVal;
    }

    #endregion

  }
}