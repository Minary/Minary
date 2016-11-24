namespace TestsHttpReverseProxy
{
  using System;
  using System.Text.RegularExpressions;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class ClientRequestMethods
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
    public void GET_request()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", serverPort, "/tests/MethodGET.php?msg=GETHello", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.IsTrue(Regex.Match(serverResponse, @"Content-Length\s*:\s*8", RegexOptions.Multiline | RegexOptions.Singleline).Success);
      Assert.IsTrue(serverResponse.EndsWith("GETHello"));
    }


    [Test]
    public void POST_request_with_content_type()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      string serverResponse = string.Empty;
      httpRequestData.Data = new string[] { "msg=POSTHello" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: application/x-www-form-urlencoded",
                                               string.Format("Content-Length: {0}", httpRequestData.Data[0].Length),
                                               "Connection: close" };
      serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/MethodPOST.php?GETHello", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);

      Assert.IsTrue(Regex.Match(serverResponse, "HTTP/1.1 200", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Success);
      Assert.IsTrue(serverResponse.EndsWith("POSTHello"));
    }


    [Test]
    public void PUT_request()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("PUT", "ruben.zhdk.ch", this.serverPort, " /tests/MethodPUT.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 405 Method Not Allowed"));
    }

    [Test]
    public void OPTIONS_request()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("OPTIONS", "ruben.zhdk.ch", this.serverPort, " / tests/MethodPUT.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 405 Method Not Allowed"));
    }

    [Test]
    public void DELETE_request()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("DELETE", "ruben.zhdk.ch", this.serverPort, " / tests/MethodPUT.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 405 Method Not Allowed"));
    }

    [Test]
    public void HEAD_request()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("HEAD", "ruben.zhdk.ch", this.serverPort, "/tests/MethodHEAD.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);

      Assert.IsTrue(Regex.Match(serverResponse, "HTTP/1.1 200", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Success);
      Assert.IsFalse(Regex.Match(serverResponse, @"Content-Length\s*:", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Success);
      Assert.IsFalse(Regex.Match(serverResponse, @"Transfer-Encoding\s*:\s*chunked", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Success);
    }

    #endregion

  }
}
