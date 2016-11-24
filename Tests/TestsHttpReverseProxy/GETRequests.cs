namespace TestsHttpReverseProxy
{
  using System;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;

  [TestFixture]
  public class GETRequests
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
    public void HTTP_GET_Existing_directory()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 200"));
      Assert.That(serverResponse, Does.Match(@".*\<\/html\>.*"));
    }



    [Test]
    public void HTTP_GET_Existing_URL()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/InputParameters.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 200"));
      Assert.IsTrue(serverResponse.EndsWith("TESTOK"));
    }


    [Test]
    public void HTTP_GET_Non_existing_URL()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/asfasdfasdfa.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 404"));
    }


    [Test]
    public void HTTP_GET_Existing_URL_and_parameters_URLEncoded()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: application/x-www-form-urlencoded",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/InputParameters.php?msg=hello+world", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 200"));
      Assert.That(serverResponse, Does.EndWith("hello world"));
    }


    [Test]
    public void HTTP_GET_Existing_URL_and_parameters_textHTML()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/InputParameters.php?msg=hello+world", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 200"));
      Assert.That(serverResponse, Does.EndWith("hello world"));
    }



    [Test]
    public void HTTP_GET_deepCh_redirect()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: deep.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "deep.ch", this.serverPort, "/", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.That(serverResponse, Does.StartWith("HTTP/1.1 301 Moved "));
      Assert.That(serverResponse, Does.Contain("Location: "));
    }

    #endregion

  }
}
