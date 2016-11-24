namespace TestsHttpReverseProxy
{
  using System;
  using System.Net;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class POSTRequestNonChunked
  {

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


    #region POST REQUEST TESTS

    [Test]
    public void HTTP_POST_Existing_URL_no_data_textPlain()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: text/plain",
                                               "Content-Length: 0",
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }


    public void HTTP_POST_Non_existing_URL()
    {
      RequestData requestDataObj = new RequestData("POST", "http", "ruben.zhdk.ch", "/tests/helloasdf.php", string.Empty);
      requestDataObj.ContentType = "text/plain";

      var ex = Assert.Throws<System.Net.WebException>(() => TestLib.Instance.SendHttpRequest(requestDataObj));
      var response = ex.Response as HttpWebResponse;

      Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
    }


    [Test]
    public void HTTP_POST_Existing_URL_data_textHtml()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=hello+world" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: text/html",
                                               string.Format("Content-Length: {0}", httpRequestData.Data[0].Length),
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }

    [Test]
    public void HTTP_POST_Existing_URL_data_textPlain()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=hello+world" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: text/plain",
                                               string.Format("Content-Length: {0}", httpRequestData.Data[0].Length),
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }

    [Test]
    public void HTTP_POST_Existing_URL_data_applicationXWwwFormUrlncoded()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=hello+world" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: application/x-www-form-urlencoded",
                                               string.Format("Content-Length: {0}", httpRequestData.Data[0].Length),
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("hello world"));
    }


    [Test]
    public void HTTP_POST_Existing_URL_data_applicationJson()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=hello+world" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: application/json",
                                               string.Format("Content-Length: {0}", httpRequestData.Data[0].Length),
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }


    [Test]
    public void HTTP_POST_Existing_URL_data_applicationOctetStream()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=hello+world" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: application/octet-clientStream",
                                               string.Format("Content-Length: {0}", httpRequestData.Data[0].Length),
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }



    [Test]
    public void HTTP_POST_Existing_URL_data_textPlain_no_contentlength()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=hello+world" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Content-Type: text/plain",
                                               "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }



    [Test]
    public void POST_request_without_content_type()
    {
      string serverResponse = string.Empty;
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { "msg=POSTHello" };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                               "Accept-Encoding: ",
                                               "Connection: close" };

      serverResponse = TestLib.Instance.RawHttpPostRequest("POST", "ruben.zhdk.ch", "/tests/InputParameters.php", httpRequestData, "\n");
      Console.WriteLine("serverResponse:{0}", serverResponse);
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }

    /*
    public void HTTP_POST_Upload_file_applicatonOctetStream()
    {
      RequestData requestDataObj = new RequestData("POST", "http", "ruben.zhdk.ch", "/fileupload.php", "msg=hello+world");
      requestDataObj.ContentType = "application/octet-clientStream";

////      "application/x-www-form-urlencoded";
      string file = @"C:\Users\Sergio\documents\visual studio 2010\Projects\WpfApplication1\WpfApplication1\Test\Avatar.png";
      string parameters = @"image=" + Convert.ToBase64string(File.ReceiveChunk(file));
    }
    */

    //// KeepAlive = true
    //// HTTPVersion = 1.1
    //// multipart/form-data


    #endregion

  }
}
