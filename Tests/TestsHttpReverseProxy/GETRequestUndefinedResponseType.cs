namespace TestsHttpReverseProxy
{
  using System;
  using System.IO;
  using System.Threading;
  using NUnit.Framework;

  [TestFixture]
  public class GETRequestUndefinedResponseType
  {

    #region THREAD METHOD

    public static void ThreadDownload(object paramObj)
    {
      string outputFile = paramObj.ToString();
      string[] headers = new string[] { "Host: rundfunk.ice.infomaniak.ch", "Accept-Encoding: ", "Connection: close" };

      try
      {
        TestLib.Instance.CancelDownload = false;
        TestLib.Instance.RawHttpRequestSaveOutputData("GET", "rundfunk.ice.infomaniak.ch", "/rundfunk-192.mp3", headers, null, outputFile, "\n");
      }
      catch (Exception)
      {
      }
    }

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
