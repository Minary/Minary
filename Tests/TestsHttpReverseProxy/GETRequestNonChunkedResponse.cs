namespace TestsHttpReverseProxy
{
  using System;
  using System.Text.RegularExpressions;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class GETRequestNonChunkedResponse
  {

    #region MEMBERS

    private int serverPort = 80;

    #endregion


    #region NUNIT

    [SetUp]
    public void TestCaseSetup()
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
    public void Nonchunked_response_1kb()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/nonChunked.php?size=1024", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);

      Assert.IsTrue(serverResponse.Length > 1024);
      Assert.That(serverResponse, Does.Contain("Content-Length: "));
      Assert.IsFalse(Regex.Match(serverResponse, @"Transfer-Encoding\s*:\s*chunked", RegexOptions.IgnoreCase | RegexOptions.Multiline).Success);
    }

    [Test]
    public void Nonchunked_response_1mb()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/nonChunked.php?size=1024&rounds=1024", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);

      Assert.IsTrue(serverResponse.Length > (1024 * 1024));
      Assert.That(serverResponse, Does.Contain("Content-Length: "));
      Assert.IsFalse(Regex.Match(serverResponse, @"Transfer-Encoding\s*:\s*chunked", RegexOptions.IgnoreCase | RegexOptions.Multiline).Success);
    }

    #endregion

  }
}
