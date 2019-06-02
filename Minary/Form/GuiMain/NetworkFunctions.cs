using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;



public static class IPCalc
{

  #region IMPORTS

  [DllImport("iphlpapi.dll", ExactSpelling = true)]
  private static extern int SendARP(uint destIP, uint srcIP, byte[] macAddress, ref uint macAddressLength);

  #endregion



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
      throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

    byte[] broadcastAddress = new byte[ipAdressBytes.Length];
    for (int i = 0; i < broadcastAddress.Length; i++)
      broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));

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
      throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

    byte[] broadcastAddress = new byte[ipAdressBytes.Length];
    for (int i = 0; i < broadcastAddress.Length; i++)
      broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));

    return new IPAddress(broadcastAddress);
  }


  /// <summary>
  /// 
  /// </summary>
  /// <param name="pIP"></param>
  /// <returns></returns>
  public static string GetMACFromIP(string pIP)
  {
    string lRetVal = String.Empty;
    IPAddress lRemoteIP;
    byte[] mac = new byte[6];
    uint len = (uint)mac.Length;
    byte[] addressBytes;
    uint dest;

    if (String.IsNullOrEmpty(pIP) || pIP == "0.0.0.0")
      throw new Exception(String.Format("GetMACFromIP() : \"{0}\" is an invalid IP address", pIP));


    lRemoteIP = IPAddress.Parse(pIP);

    addressBytes = lRemoteIP.GetAddressBytes();
    dest = ((uint)addressBytes[3] << 24)
      + ((uint)addressBytes[2] << 16)
      + ((uint)addressBytes[1] << 8)
      + ((uint)addressBytes[0]);

    if (SendARP(dest, 0, mac, ref len) != 0)
      throw new Exception(String.Format("GetMACFromIP() : The ARP request for {0} failed.", pIP));


    lRetVal = String.Format("{0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}", mac[0], mac[1], mac[2], mac[3], mac[4], mac[5]);

    return (lRetVal);
  }
}





public static class HTTPReq
{

  /// <summary>
  /// 
  /// </summary>
  /// <param name="pURL"></param>
  /// <returns></returns>
  public static bool HTTPPing(string pURL)
  {
    bool lRetVal = false;

    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pURL);
    request.Timeout = 4000;
    request.Method = "HEAD";
    try
    {
      using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
      {
        if (response.StatusCode == HttpStatusCode.OK)
          lRetVal = true;
      }
    }
    catch (WebException)
    {
    }

    return (lRetVal);
  }



  /// <summary>
  /// 
  /// </summary>
  /// <param name="pURL"></param>
  /// <param name="pPostData"></param>
  /// <param name="pUserAgent"></param>
  /// <returns></returns>
  public static string sendPostRequest(string pURL, string pPostData, string pUserAgent)
  {
    string lRetVal = String.Empty;
    HttpWebRequest lRequest;
    byte[] lByteArray;
    Stream lDataStream = null;
    HttpWebResponse lWebResponse = null;
    StreamReader lReader = null;


    lRequest = (HttpWebRequest)WebRequest.Create(pURL);
    lRequest.UserAgent = pUserAgent;
    lRequest.KeepAlive = false;
    lRequest.Method = "POST";
    lRequest.ServicePoint.Expect100Continue = false;
    //lRequest.CookieContainer = new CookieContainer();
    //lRequest.CookieContainer.



    lRequest.Timeout = 10000;

    // Build request
    lByteArray = Encoding.UTF8.GetBytes(pPostData);
    lRequest.ContentType = "application/x-www-form-urlencoded";
    lRequest.ContentLength = lByteArray.Length;

    try
    {
      lDataStream = lRequest.GetRequestStream();
      lDataStream.Write(lByteArray, 0, lByteArray.Length);

      lWebResponse = (HttpWebResponse)lRequest.GetResponse();
      lDataStream = lWebResponse.GetResponseStream();
      lReader = new StreamReader(lDataStream);
      lRetVal = lReader.ReadToEnd();
    }
    catch (WebException)
    {
    }
    finally
    {
      if (lReader != null)
        lReader.Close();

      if (lDataStream != null)
        lDataStream.Close();

      if (lWebResponse != null)
        lWebResponse.Close();
    }

    return (lRetVal);
  }




  /// <summary>
  /// 
  /// </summary>
  /// <param name="pURL"></param>
  /// <param name="pUserAgent"></param>
  /// <param name="pCookies"></param>
  /// <returns></returns>
  public static string sendGetRequest(string pURL, string pUserAgent, string pCookies)
  {
    string lRetVal = String.Empty;
    HttpWebRequest lRequest = null;
    HttpWebResponse lWebResponse = null;
    StreamReader lReader = null;

    try
    {
      lRequest = (HttpWebRequest)WebRequest.Create(pURL);
      if (pUserAgent.Length > 0)
        lRequest.UserAgent = pUserAgent;
      lRequest.Method = "GET";
      lRequest.KeepAlive = false;
      lRequest.Timeout = 10000;
      if (pCookies.Length > 0)
        lRequest.Headers.Add("Cookie", pCookies);

    }
    catch (Exception)
    {
    }


    try
    {
      lWebResponse = (HttpWebResponse)lRequest.GetResponse();
      lReader = new StreamReader(lWebResponse.GetResponseStream());
      lRetVal = lReader.ReadToEnd();
    }
    catch (WebException)
    {
    }
    finally
    {
      if (lReader != null)
        lReader.Close();

      if (lWebResponse != null)
        lWebResponse.Close();
    }
    return (lRetVal);
  }



  /// <summary>
  /// 
  /// </summary>
  /// <param name="pURL"></param>
  /// <param name="pLocalFilePath"></param>
  public static void DownloadFile(string pURL, string pLocalFilePath)
  {
    HttpWebRequest lWebRequest;
    HttpWebResponse lWebResponse = null;
    Stream lDownloadStream = null;
    FileStream lFSOutput = null;
    byte[] lReadBuf;
    int lCount;

    try
    {
      lWebRequest = (HttpWebRequest)WebRequest.Create(pURL);
      lWebRequest.Timeout = 5000;
      lWebRequest.AllowWriteStreamBuffering = false;
      lWebResponse = (HttpWebResponse)lWebRequest.GetResponse();
      lDownloadStream = lWebResponse.GetResponseStream();


      //Write to disk
      lFSOutput = new FileStream(pLocalFilePath, FileMode.Create);
      lReadBuf = new byte[256];

      while ((lCount = lDownloadStream.Read(lReadBuf, 0, lReadBuf.Length)) > 0)
        lFSOutput.Write(lReadBuf, 0, lCount);

      //Close everything
    }
    catch (Exception)
    {
      throw;
    }

    finally
    {
      if (lFSOutput != null)
        lFSOutput.Close();

      if (lDownloadStream != null)
        lDownloadStream.Close();

      if (lWebResponse != null)
        lWebResponse.Close();

    }
  }
}