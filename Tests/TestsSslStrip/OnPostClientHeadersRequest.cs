namespace TestsSslStrip
{
  using HttpReverseProxy.Plugin.SslStrip;
  using HttpReverseProxy.Plugin.SslStrip.Cache;
  using HttpReverseProxy.Plugin.SslStrip.DataTypes;
  using HttpReverseProxyLib;
  using HttpReverseProxyLib.DataTypes;
  using NUnit.Framework;

  [TestFixture]
  public class OnPostClientHeadersRequest
  {

    #region MEMBERS

    private SslStrip sslStripObj;
    private RequestObj requestObj;

    #endregion


    #region NUNIT

    [OneTimeSetUp]
    public void TestFixtureSetup()
    {
    }

    [SetUp]
    public void SingleTestSetup()
    {
      this.sslStripObj = new SslStrip();
      this.requestObj = new RequestObj("ruben.zhdk.ch");
      Logging.Instance.ResetLogging();
    }

    public void SingleTestTearDown()
    {
      CacheHsts.Instance.ResetCache();
      HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.ResetCache();
    }

    #endregion


    #region TESTS

    [Test]
    public void Request_must_be_mapped_because_of_a_previously_detected_redirect()
    {
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";

      HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache.Clear();
      HostRecord tmpHost = new HostRecord("GET", "http", "ruben.zhdk.ch", "/tests/SslStrip/MustBeRedirected.php");
      HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache.Add("http://ruben.zhdk.ch/tests/SslStrip/MustBeRedirected.php", tmpHost);

      // SSLStripping Hosts config
      Logging.Instance.IsInTestingMode = true;

      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/tests/SslStrip/MustBeRedirected.php";
      this.sslStripObj.OnPostClientHeadersRequest(this.requestObj);

      Logging.Instance.DumpLogRecords();
      Assert.IsTrue(Logging.Instance.FindLogRecordByRegex(@".*SslStrip\.OnPostClientHeadersRequest\(\) : HTTP redirect\(301\/302\) from").Count > 0);
    }



    [Test]
    public void Request_must_be_mapped_because_of_a_previously_detected_server_hsts_response()
    {
      CacheHsts.Instance.AddElement("ruben.zhdk.ch");
      Logging.Instance.IsInTestingMode = true;

      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/tests/testUrl.php";
      this.sslStripObj.OnPostClientHeadersRequest(this.requestObj);

      Logging.Instance.DumpLogRecords();
      Assert.IsTrue(Logging.Instance.FindLogRecordByRegex(@".*SslStrip\.OnPostClientHeadersRequest\(\) : HSTS header set for .*").Count > 0);
      Assert.IsTrue(this.requestObj.ClientRequestObj.Scheme == "https");
    }




    [Test]
    public void Request_must_be_mapped_because_of_a_previously_ssl_strip()
    {
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      Logging.Instance.IsInTestingMode = true;

      HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache.Clear();
      HostRecord tmpHost = new HostRecord("GET", "http", "ruben.zhdk.ch", "/tests/SslStrip/MustBeRedirected.php");
      HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache.Add("http://ruben.zhdk.ch/tests/SslStrip/MustBeRedirected.php", tmpHost);

      // SSLStripping Hosts config
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/tests/SslStrip/MustBeRedirected.php";
      this.sslStripObj.OnPostClientHeadersRequest(this.requestObj);

      Logging.Instance.DumpLogRecords();
      Assert.IsTrue(this.requestObj.ClientRequestObj.Scheme == "https");
      Assert.IsTrue(Logging.Instance.FindLogRecordByRegex(@".*SslStrip\.OnPostClientHeadersRequest\(\) \: SslStripped from ").Count > 0);
    }


    #endregion

  }
}
