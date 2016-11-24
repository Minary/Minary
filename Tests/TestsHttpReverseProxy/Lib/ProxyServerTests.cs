namespace TestsHttpReverseProxy
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Net;
  using System.Net.Sockets;
  using System.Reflection;
  using System.Text;
  using TestsHttpReverseProxy.Lib;

  public class TestLib
  {

    #region MEMBERS

    private static TestLib instance;
    private HttpReverseProxy.ProxyServer proxyServer;
    private StringBuilder serverResponse;

    #endregion


    #region PROPERTIES

    public static TestLib Instance { get { return instance ?? (instance = new TestLib()); } set { } }
    public HttpReverseProxy.ProxyServer ProxyServerInst { get { return this.proxyServer; } set { } }
    public string ServerResponse { get { return serverResponse.ToString(); } set { } }

    #endregion


    #region PUBLIC

    public string BuildHttpRequestString(string requestMethod, string host, string path, string[] headers, string[] data, string newLine = "\n")
    {
      string requestString = string.Empty;
      string requestHeaders = string.Join(newLine, headers);

      requestString = string.Format("{1} {2} HTTP/1.1{0}{3}{0}{0}", newLine, requestMethod, path, requestHeaders);

      if (data != null && data.Length > 0)
      {

      }

      Console.WriteLine("RawHttpRequest(): Requeststring:|{0}|", requestString);


      return requestString;
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="host</param>
    /// <param name="path</param>
    /// <returns></returns>
    public string RawHttpPostRequest(string requestMethod, string host, string path, HttpTestRequest httpRequest, string newLine = "\n")
    {
      byte[] requestString = null;
      byte[] buffer = new byte[4096];

      httpRequest.Data = httpRequest.Data ?? (new string[] { });

      this.serverResponse = new StringBuilder();
      TcpClient tcpClient = new TcpClient("localhost", 80);
      NetworkStream clientStream = tcpClient.GetStream();
      string requestHeaders = string.Join(newLine, httpRequest.Headers);

      requestString = Encoding.UTF8.GetBytes(string.Format("{1} {2} HTTP/1.1{0}{3}{0}{0}", newLine, requestMethod, path, requestHeaders));
      Console.WriteLine("RawHttpRequest(): Requeststring:|{0}|", Encoding.UTF8.GetString(requestString));
      clientStream.Write(requestString, 0, requestString.Length);
      clientStream.Flush();

      // Send client data
      foreach (string tmpData in httpRequest.Data)
      {
        string realData = tmpData + newLine;
        Console.WriteLine("RawHttpRequest(): data:|{0}|", realData);
        byte[] tmpDataArray = Encoding.UTF8.GetBytes(realData);

        clientStream.Write(tmpDataArray, 0, tmpDataArray.Length);
        clientStream.Flush();
      }


      // Read cRemoteSocket response
      int bytesRead = 0;
      int totalBytesRead = 0;
      int timeOutPrevious = clientStream.ReadTimeout = 5000;

      while ((bytesRead = clientStream.Read(buffer, 0, buffer.Length)) > 0)
      {
        totalBytesRead += bytesRead;
        this.serverResponse.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
      }

      clientStream.ReadTimeout = timeOutPrevious;
      clientStream.Close();
      tcpClient.Close();

      return this.serverResponse.ToString();
    }


    public string RawHttpGetRequest(string requestMethod, string host, int serverPort, string path, string httpVersion, HttpTestRequest httpRequestData, string newLine = "\n")
    {
      byte[] requestString = null;
      byte[] buffer = new byte[4096];

      httpRequestData.Data = httpRequestData.Data ?? (new string[] { });

      this.serverResponse = new StringBuilder();
      TcpClient tcpClient = new TcpClient("localhost", serverPort);
      NetworkStream clientStream = tcpClient.GetStream();
      string requestHeaders = string.Join(newLine, httpRequestData.Headers);

      requestString = Encoding.UTF8.GetBytes(string.Format("{1} {2} {3}{0}{4}{0}{0}", newLine, requestMethod, path, httpVersion, requestHeaders));
      Console.WriteLine("RawHttpRequest(): Requeststring:|{0}|", Encoding.UTF8.GetString(requestString));
      clientStream.Write(requestString, 0, requestString.Length);
      clientStream.Flush();

      // Send client data
      if (httpRequestData.Data != null && httpRequestData.Data.Count() > 0)
      {
        string realData = string.Join("", httpRequestData.Data);
        Console.WriteLine("RawHttpRequest(): data:|{0}|", realData);
        byte[] tmpDataArray = Encoding.UTF8.GetBytes(realData);

        clientStream.Write(tmpDataArray, 0, tmpDataArray.Length);
        clientStream.Flush();
      }

      // Read cRemoteSocket response
      int bytesRead = 0;
      int totalBytesRead = 0;
      int timeOutPrevious = clientStream.ReadTimeout = 5000;

      while ((bytesRead = clientStream.Read(buffer, 0, buffer.Length)) > 0)
      {
        totalBytesRead += bytesRead;
        this.serverResponse.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
      }

      clientStream.ReadTimeout = timeOutPrevious;
      clientStream.Close();
      tcpClient.Close();

      return this.serverResponse.ToString();
    }



    public List<string> RawHttpGetRequestKeepAlive(string requestMethod, string host, string path, string httpVersion, List<HttpTestRequest> httpRequestData, string newLine = "\n")
    {
      byte[] requestString = null;
      byte[] buffer = new byte[4096];
      StringBuilder serverResponse = new StringBuilder();
      List<string> returnBuffer = new List<string>();
      TcpClient tcpClient = new TcpClient("localhost", 80);
      NetworkStream clientStream = tcpClient.GetStream();

      foreach (HttpTestRequest tmpRequest in httpRequestData)
      {
        serverResponse.Clear();
        tmpRequest.Data = tmpRequest.Data ?? (new string[] { });
        string requestHeaders = string.Join(newLine, tmpRequest.Headers);

        requestString = Encoding.UTF8.GetBytes(string.Format("{1} {2} {3}{0}{4}{0}{0}", newLine, requestMethod, path, httpVersion, requestHeaders));
        Console.WriteLine("RawHttpRequest(): Requeststring:|{0}|", Encoding.UTF8.GetString(requestString));
        clientStream.Write(requestString, 0, requestString.Length);
        clientStream.Flush();

        // Send client data
        if (tmpRequest.Data != null && tmpRequest.Data.Count() > 0)
        {
          string realData = string.Join("", tmpRequest.Data);
          Console.WriteLine("RawHttpRequest(): data:|{0}|", realData);
          byte[] tmpDataArray = Encoding.UTF8.GetBytes(realData);

          clientStream.Write(tmpDataArray, 0, tmpDataArray.Length);
          clientStream.Flush();
        }

        // Read cRemoteSocket response
        int bytesRead = 0;
        int totalBytesRead = 0;
        int timeOutPrevious = clientStream.ReadTimeout = 5000;

        try
        {
          while ((bytesRead = clientStream.Read(buffer, 0, buffer.Length)) > 0)
          {
            totalBytesRead += bytesRead;
            serverResponse.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            Console.WriteLine(">Bytes received: {0}/{1}/{2}", bytesRead, totalBytesRead, serverResponse.ToString());
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Exception: {0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
        }

        returnBuffer.Add(serverResponse.ToString());
        clientStream.ReadTimeout = timeOutPrevious;
      }

      clientStream.Close();
      tcpClient.Close();

      return returnBuffer;
    }







    /// <summary>
    ///
    /// </summary>
    /// <param name="host</param>
    /// <param name="path</param>
    /// <param name="outputFilePath"></param>
    private bool cancelDownload = false;
    public bool CancelDownload { get { return cancelDownload; } set { cancelDownload = value; } }
    public void RawHttpRequestSaveOutputData(string requestMethod, string host, string path, string[] headers, string[] data, string outputFilePath, string newLine = "\n")
    {
      byte[] requestString = null;
      byte[] buffer = new byte[4096];

      data = data ?? (new string[] { });

      this.serverResponse = new StringBuilder();
      TcpClient tcpClient = new TcpClient("localhost", 80);
      NetworkStream clientStream = tcpClient.GetStream();
      string requestHeaders = string.Join(newLine, headers);

      requestString = Encoding.UTF8.GetBytes(string.Format("{1} {2} HTTP/1.1{0}{3}{0}{0}", newLine, requestMethod, path, requestHeaders));
      Console.WriteLine("RawHttpRequest(): Requeststring:|{0}|", Encoding.UTF8.GetString(requestString));
      clientStream.Write(requestString, 0, requestString.Length);
      clientStream.Flush();






      int bytesRead = 0;
      int totalBytesRead = 0;
      string outputData = string.Empty;

      Console.WriteLine("RawHttpRequestSaveOutputData(): Start");

      while ((bytesRead = clientStream.Read(buffer, 0, buffer.Length)) > 0)
      {
        totalBytesRead += bytesRead;
        outputData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("RawHttpRequestSaveOutputData(): Read/Saved {0} bytes to {1}", bytesRead, outputFilePath);

        try
        {
          File.AppendAllText(outputFilePath, outputData);
        }
        catch (Exception ex)
        {
          Console.WriteLine("RawHttpRequestSaveOutputData(EXCEPTION): {0}", ex.Message);
        }

        if (CancelDownload == true)
        {
          Console.WriteLine("RawHttpRequestSaveOutputData() : Thread was cancelled");
          break;
        }
      }

      clientStream.Close();
      tcpClient.Close();
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public ServerResponseData SendHttpRequest(RequestData requestData)
    {
      ServerResponseData serverOutputData = new ServerResponseData();
      Stream serverRequestDataStream = null;
      string requestUrl = string.Format("{0}://localhost{1}", requestData.Scheme, requestData.Path);
      byte[] byteArray;

      WebRequest request = WebRequest.Create(requestUrl);

      if (!string.IsNullOrEmpty(requestData.ContentType))
      {
        request.ContentType = requestData.ContentType;
      }

      request.Method = requestData.Method;
      request.Timeout = 10000;
      ((HttpWebRequest)request).Host = requestData.RemoteServer;
      ((HttpWebRequest)request).ServicePoint.Expect100Continue = false;
      ((HttpWebRequest)request).AllowAutoRedirect = false;
      ((HttpWebRequest)request).ServicePoint.MaxIdleTime = 20 * 1000;
////      ((HttpWebRequest)request).Connection = "close";
      ((HttpWebRequest)request).KeepAlive = false;


      // 1. Send request data to cRemoteSocket
      if (!string.IsNullOrEmpty(requestData.Data) && requestData.Method == "POST")
      {
        byteArray = Encoding.UTF8.GetBytes(requestData.Data);
        request.ContentLength = byteArray.Length;

        serverRequestDataStream = request.GetRequestStream();
        serverRequestDataStream.Write(byteArray, 0, byteArray.Length);
//        serverRequestDataStream.Close();
      }

      WebResponse response = request.GetResponse();
      serverOutputData.ServerResponseStatusCode = (int)((HttpWebResponse)response).StatusCode;
      WebHeaderCollection header = response.Headers;
      for (int i = 0; i < header.Count; i++)
      {
        serverOutputData.Headers.Add(header.GetKey(i), header[i]);
      }

      StreamReader input = new StreamReader(response.GetResponseStream());
      serverOutputData.ServerOutputData = input.ReadToEnd();
      input.Close();

      return serverOutputData;
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// Initializes a new instance of the <see cref="TestLib"/> class.
    ///
    /// </summary>
    private TestLib()
    {
      Console.WriteLine("Initiating TestLib environment ...");

      SetAllowUnsafeHeaderParsing();
      this.proxyServer = new HttpReverseProxy.ProxyServer();
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private static bool SetAllowUnsafeHeaderParsing()
    {
      //// Get the assembly that contains the internal class
      Assembly netAssembly = Assembly.GetAssembly(
        typeof(System.Net.Configuration.SettingsSection));
      if (netAssembly != null)
      {
        // Use the assembly in order to get the internal type for
        // the internal class
        Type settingsType = netAssembly.GetType(
          "System.Net.Configuration.SettingsSectionInternal");
        if (settingsType != null)
        {
          // Use the internal static property to get an instance
          // of the internal settings class. If the static instance
          // isn't created allready the property will create it for us.
          object instance = settingsType.InvokeMember("Section", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
          if (instance != null)
          {
            // Locate the private bool field that tells the
            // framework is unsafe headerByteArray parsing should be
            // allowed or not
            FieldInfo useUnsafeHeaderParsing = settingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
            if (useUnsafeHeaderParsing != null)
            {
              useUnsafeHeaderParsing.SetValue(instance, true);
              return true;
            }
          }
        }
      }

      return false;
    }


    #endregion

  }
}
