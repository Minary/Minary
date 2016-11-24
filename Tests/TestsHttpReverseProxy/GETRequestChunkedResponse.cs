namespace TestsHttpReverseProxy
{
  using System;
  using System.IO;
  using System.Text.RegularExpressions;
  using System.Threading;
  using NUnit.Framework;
  using TestsHttpReverseProxy.Lib;


  [TestFixture]
  public class GETRequestChunkedResponse
  {

    #region THREAD METHOD

    public static void ThreadDownload(object paramObj)
    {
      string outputFile = paramObj.ToString();
      string[] headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };

      try
      {
        TestLib.Instance.CancelDownload = false;
        TestLib.Instance.RawHttpRequestSaveOutputData("GET", "ruben.zhdk.ch", "/tests/EndlessStream.php", headers, null, outputFile, "\n");
      }
      catch (Exception)
      {
      }
    }

    #endregion


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
    public void Chunked_response_zero_chunks()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/Chunked.php?size=0&rounds=0", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);
 
      Assert.IsFalse(Regex.Match(serverResponse, @"Content-Length", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success);
      Assert.That(serverResponse, Does.Match(@"Transfer-Encoding\s*:\s*chunked"));
      Assert.That(serverResponse, Does.Match(@"(\r\n|\n){2}0(\r\n\r\n|\n\n)$"));
    }


    [Test]
    public void Chunked_response_chunksize_padded_with_zeros()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/ChunkedEx.php?size=128&rounds=2&padding=true", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.IsFalse(Regex.Match(serverResponse, @"Content-Length", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success);
      Assert.That(serverResponse, Does.Match(@"Transfer-Encoding\s*:\s*chunked"));
      Assert.That(serverResponse, Does.Match(@"(\r\n|\n){2}0(\r\n\r\n|\n\n)$"));
    }



    [Test]
    public void Chunked_response_1kb()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/Chunked.php?size=1024", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.IsTrue(serverResponse.Length > 1024);
      Assert.IsFalse(Regex.Match(serverResponse, @"Content-Length", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success);
      Assert.That(serverResponse, Does.Match(@"Transfer-Encoding\s*:\s*chunked"));
      Assert.That(serverResponse, Does.Match(@"(\r\n|\n)0(\r\n\r\n|\n\n)$"));
    }


    [Test]
    public void Chunked_response_1kb_chunklen_with_leading_zeroes()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/Chunked.php?size=1024", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: |{0}|", serverResponse);

      Assert.IsTrue(serverResponse.Length > 1024);
      Assert.IsFalse(Regex.Match(serverResponse, @"Content-Length", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success);
      Assert.That(serverResponse, Does.Match(@"Transfer-Encoding\s*:\s*chunked"));
      Assert.That(serverResponse, Does.Match(@"(\r\n|\n)0(\r\n\r\n|\n\n)$"));
    }


    [Test]
    public void Chunked_response_1mb()
    {
      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = null;
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch", "Accept-Encoding: ", "Connection: close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/Chunked.php?size=1024&rounds=1024", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);

      Assert.IsTrue(serverResponse.Length > (1024 * 1024));
      Assert.IsFalse(Regex.Match(serverResponse, @"Content-Length", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success);
      Assert.That(serverResponse, Does.Match(@"Transfer-Encoding\s*:\s*chunked"));
      Assert.That(serverResponse, Does.Match(@"(\r\n|\n)0(\r\n\r\n|\n\n)$"));
    }


    [Test]
    public void Endless_octet_stream_is_directly_forwarded_to_client()
    {
      int sleep = 0;
      string outputFile = Path.GetTempFileName();
      Random random = new Random();

      // 1. Start thread
      Thread downloadThread = new Thread(ThreadDownload);
      downloadThread.Start(outputFile);

      // 2. Sleep %RANDOM% seconds
      sleep = random.Next(1000, 10000);
      Console.WriteLine("sleeping for {0} milliseconds", sleep);
      Thread.Sleep(sleep);

      // 3. Memorize current file size
      long currentFileSize1 = (new FileInfo(outputFile)).Length;





      // 4. Sleep %RANDOM% seconds
      sleep = random.Next(1000, 10000);
      Console.WriteLine("sleeping for {0} milliseconds", sleep);
      Thread.Sleep(sleep);

      // 5. Memorize current file size
      long currentFileSize2 = (new FileInfo(outputFile)).Length;

      // 6. Stop download thread
      TestLib.Instance.CancelDownload = true;
      downloadThread.Join();

      Console.WriteLine("File size 1: {0} bytes", currentFileSize1);
      Console.WriteLine("File size 2: {0} bytes", currentFileSize2);

      Assert.IsTrue(currentFileSize1 > 0);
      Assert.IsTrue(currentFileSize1 < currentFileSize2);
    }

    #endregion

  }
}
