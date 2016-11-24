namespace TestsHttpReverseProxy.Lib
{
  using System.Collections.Generic;

  public class ServerResponseData
  {

    #region MEMBERS

    private Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
    private string serverOutputData;
    private int serverResponseStatusCode;

    #endregion


    #region PROPERTIES

    public Dictionary<string, string> Headers
    {
      get { return this.requestHeaders; }
      set { this.requestHeaders = value; }
    }

    public string ServerOutputData
    {
      get { return this.serverOutputData; }
      set { this.serverOutputData = value; }
    }

    public int ServerResponseStatusCode
    {
      get { return this.serverResponseStatusCode; }
      set { this.serverResponseStatusCode = value; }
    }

    #endregion

  }
}
