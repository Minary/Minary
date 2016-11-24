namespace TestsHttpReverseProxy
{
  using System;
  using System.Collections.Generic;
  using System.Text.RegularExpressions;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class ConnectionType
  {

    #region MEMBERS

    private int serverPort = 80;

    #endregion


    #region NUNIT

    [SetUp]
    public void SingleTestSetUp()
    {
      TestLib.Instance.ProxyServerInst.Start();
    }

    [TearDown]
    public void SingleTestTearDown()
    {
      TestLib.Instance.ProxyServerInst.Stop();
    }

    #endregion


    #region TESTS

    [Test]
    public void GET_request_Connection_close()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                           "Accept-Encoding: ",
                                           "Content-Type: application/x-www-form-urlencoded",
                                           "Content-Length: 0",
                                           "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/index.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 200"));
      Assert.That(serverResponse, Does.EndWith("</html>\n"));
    }




    [Test]
    public void GET_request_Connection_keepalive()
    {
      List<HttpTestRequest> requestList = new List<HttpTestRequest>();
      HttpTestRequest httpRequestData1 = new HttpTestRequest();
      httpRequestData1.Data = new string[] { };
      httpRequestData1.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: application/x-www-form-urlencoded",
                                        "Content-Length: 0",
                                        "Connection: keep-alive" };


      // Second request, close the connection after the request
      HttpTestRequest httpRequestData2 = new HttpTestRequest();
      httpRequestData2.Data = new string[] { };
      httpRequestData2.Headers = new string[] { "Host: ruben.zhdk.ch",
                               "Accept-Encoding: ",
                               "Content-Type: application/x-www-form-urlencoded",
                               "Content-Length: 0",
                               "Connection: Close" };

      requestList.Add(httpRequestData1);
      requestList.Add(httpRequestData1);
      requestList.Add(httpRequestData2);

      List<string> serverResponse = TestLib.Instance.RawHttpGetRequestKeepAlive("GET", "ruben.zhdk.ch", "/tests/index.php", "HTTP/1.1", requestList, "\n");
      Assert.IsTrue(serverResponse.Count == 3);
      foreach (string tmpServerResponse in serverResponse)
      {
        Assert.That(tmpServerResponse, Does.Contain("</html>\n"));
      }
    }

    #endregion

  }
}
