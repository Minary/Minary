namespace TestsHttpReverseProxy
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  public class HttpServerErrors
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
    public void Http_request_http_contains_unsupported_method()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest(string.Empty, "ruben.zhdk.ch", this.serverPort, "/", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 405 Method Not Allowed"));
    }

    [Test]
    public void Http_request_http_path_missing()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, string.Empty, "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 400 Bad Request"));
    }

    [Test]
    public void Http_request_http_version_missing()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/", string.Empty, httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 400 Bad Request"));
    }

    [Test]
    public void Http_request_remote_host_does_not_exist()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: honkhonk.ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 400 Bad Request"));
    }

    #endregion

  }
}
