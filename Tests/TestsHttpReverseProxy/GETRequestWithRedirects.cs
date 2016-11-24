namespace TestsHttpReverseProxy
{
  using System;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class GETRequestWithRedirects
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
    public void HTTP_GET_redirects_http_to_http()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/Redirect.php?type=301&location=http%3A%2F%2Fredirect.ruben.zhdk.ch%2F", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.That(serverResponse, Does.Contain("Location: http://redirect.ruben.zhdk.ch/"));
    }


    [Test]
    public void HTTP_GET_redirects_http_to_http_subpath()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/Redirect.php?type=301&location=http%3A%2F%2Fredirect.ruben.zhdk.ch%2Ftests%2Findex.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.That(serverResponse, Does.Contain("Location: http://redirect.ruben.zhdk.ch/tests/index.php"));
    }



    [Test]
    public void HTTP_GET_redirects_http_to_https_subpath()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/Redirect.php?type=301&location=https%3A%2F%2Fredirect.ruben.zhdk.ch%2Ftests%2Findex.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.That(serverResponse, Does.Contain("Location: https://redirect.ruben.zhdk.ch/tests/index.php"));
    }

    #endregion

  }
}
