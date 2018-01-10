namespace Minary.Common
{
  using System;
  using System.Net;
  using System.Net.NetworkInformation;
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
      for (var i = 0; i < broadcastAddress.Length; i++)
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

      var broadcastAddress = new byte[ipAdressBytes.Length];
      for (var i = 0; i < broadcastAddress.Length; i++)
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
      var retVal = string.Empty;
      IPAddress remoteIpAddress;
      var mac = new byte[6];
      var len = (uint)mac.Length;
      byte[] addressBytes;
      uint dest;

      if (string.IsNullOrEmpty(ipAddress) || 
          ipAddress == "0.0.0.0")
      {
        throw new Exception($"GetMacFromIp(): \"{ipAddress}\" is an invalid IpAddress address");
      }

      remoteIpAddress = IPAddress.Parse(ipAddress);
      addressBytes = remoteIpAddress.GetAddressBytes();
      dest = ((uint)addressBytes[3] << 24)
           + ((uint)addressBytes[2] << 16)
           + ((uint)addressBytes[1] << 8)
           + ((uint)addressBytes[0]);

      if (SendARP(dest, 0, mac, ref len) != 0)
      {
        throw new Exception($"GetMacFromIp(): The ARP request for {ipAddress} failed.");
      }

      return $"{mac[0]:x2}-{mac[1]:x2}-{mac[2]:x2}-{mac[3]:x2}-{mac[4]:x2}-{mac[5]:x2}";
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

      var isPortAvailable = true;
      IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
      IPEndPoint[] ipEndPoints = ipGlobalProperties.GetActiveTcpListeners();

      foreach (var endPoint in ipEndPoints)
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
    /// <param name="buf"></param>
    /// <returns></returns>
    public static string MacByteArrayToString(byte[] buf)
    {
      return $"{buf[0]:X2}:{buf[1]:X2}:{buf[2]:X2}:{buf[3]:X2}:{buf[4]:X2}:{buf[5]:X2}";
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="buf"></param>
    /// <returns></returns>
    public static string IpByteArrayToString(byte[] buf)
    {
      return $"{buf[0]}.{buf[1]}.{buf[2]}.{buf[3]}";
    }

    #endregion

  }
}