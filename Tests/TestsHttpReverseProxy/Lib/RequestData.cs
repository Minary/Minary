namespace TestsHttpReverseProxy.Lib
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class RequestData
  {

    #region MEMBERS

    private string method;
    private string scheme;
    private string remoteServer;
    private string path;
    private string contentType;
    private string contentEncoding;
    private string data;

    #endregion


    #region PROPERTIES

    public string Method { get { return method; } set { method = value; } }
    public string Scheme { get { return scheme; } set { scheme = value; } }
    public string RemoteServer { get { return remoteServer; } set { remoteServer = value; } }
    public string Path { get { return path; } set { path = value; } }
    public string ContentType { get { return contentType; } set { contentType = value; } }
    public string ContentEncoding { get { return contentEncoding; } set { contentEncoding = value; } }
    public string Data { get { return data; } set { data = value; } }

    #endregion


    #region  PUBLIC

    public RequestData(string remoteMethod, string scheme, string remoteServer, string path, string requestData)
    {
      Method = remoteMethod;
      Scheme = scheme;
      RemoteServer = remoteServer;
      Path = path;
      Data = requestData;
      ContentEncoding = "UTF-8";
    }

    #endregion

  }
}
