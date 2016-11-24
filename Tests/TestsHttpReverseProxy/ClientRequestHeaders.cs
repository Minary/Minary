namespace TestsHttpReverseProxy
{
  using System;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;


  [TestFixture]
  public class ClientRequestHeaders
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
    public void Host_header_missing()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", serverPort, "/tests/", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 404 Not Found"));
    }

    #endregion

  }
}
