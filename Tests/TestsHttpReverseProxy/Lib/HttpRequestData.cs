namespace TestsHttpReverseProxy.Lib
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class HttpTestRequest
  {

    #region MEMBERS

    private string[] data;
    private string[] headers;

    #endregion


    #region PROPERTIES

    public string[] Data { get { return data; } set { data = value; } }
    public string[] Headers { get { return headers; } set { headers = value; } }

    #endregion


    #region PUBLIC

    public HttpTestRequest()
    {
    }

    #endregion

  }
}
