namespace TestsHttpReverseProxy
{
  using System;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class ServerResponseHeaders
  {

    #region MEMBERS

    private int serverPort = 80;

    #endregion


    #region NUNIT

    [SetUp]
    public void SingleTestSetup()
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
    public void GET_request_required_headers_declared()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Accept-Encoding: ",
                                               "Content-Type: text/html",
                                               "Connection: Close",
                                               "Host: ruben.zhdk.ch" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 200 OK"));
      Assert.That(serverResponse, Does.Match(@"\nServer: "));
      Assert.That(serverResponse, Does.Match(@"\nConnection: "));
      Assert.That(serverResponse, Does.Match(@"\nContent-Type: "));
      Assert.That(serverResponse, Does.Match(@"\nContent-Length: "));
      Assert.That(serverResponse, Does.Match(@"\nDate: "));
    }

    #endregion

  }
}
