namespace TestsHttpReverseProxy
{
  using System;
  using System.Text.RegularExpressions;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class POSTRequestChunked
  {

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
    public void POST_request_with_chunked_data_upload_sepparated_by_LF()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "5", "msg=P", "8", "OSTHello", "0", string.Empty };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: application/x-www-form-urlencoded",
                                               "Transfer-Encoding: chunked",
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/MethodPOST.php?GETHello", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.IsTrue(Regex.Match(serverResponse, "HTTP/1.1 200", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Success);
      Assert.IsTrue(Regex.Match(serverResponse, "POSTHello", RegexOptions.Multiline | RegexOptions.Singleline).Success);
    }


    [Test]
    public void POST_request_with_chunked_data_upload_sepparated_by_CRLF()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "5", "msg=P", "8", "OSTHello", "0", string.Empty };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: application/x-www-form-urlencoded",
                                               "Transfer-Encoding: chunked",
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/MethodPOST.php?GETHello", httpRequestData, "\r\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.IsTrue(Regex.Match(serverResponse, "HTTP/1.1 200", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Success);
      Assert.IsTrue(Regex.Match(serverResponse, "POSTHello", RegexOptions.Multiline | RegexOptions.Singleline).Success);
    }



    [Test]
    public void POST_request_with_nonchunked_data_upload()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=POSTHello", string.Empty };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: application/x-www-form-urlencoded",
                                               "Content-Length: 13",
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/MethodPOST.php?GETHello", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.IsTrue(Regex.Match(serverResponse, "HTTP/1.1 200", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Success);
      Assert.IsTrue(Regex.Match(serverResponse, "POSTHello", RegexOptions.Multiline | RegexOptions.Singleline).Success);
    }

    #endregion

  }
}
