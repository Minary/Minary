namespace TestsSslStrip
{
  using HttpReverseProxy.Plugin.SslStrip;
  using HttpReverseProxyLib.DataTypes;
  using NUnit.Framework;
  using System;
  using System.Collections;

  public class OnPostServerHeadersResponse
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

      // Cache was updated by SslStrip plugin
      HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.ResetCache();
      HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.ResetCache();
      HttpReverseProxy.Plugin.SslStrip.Cache.CacheHsts.Instance.ResetCache();
    }

    #endregion


    #region TESTS : 302/301 Location redirect

    [Test]
    public void ProcessServerResponseHeaders_no_redirect()
    {
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/";
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders = new Hashtable();

      // SslStrip plugin instruction: No cache record, procede
      RedirectType redir = this.sslStripObj.DetermineRedirectType(this.requestObj);
      Assert.IsTrue(redir == RedirectType.Http2http2XX);

      // Caches are empty
      this.sslStripObj.ProcessServerResponseHeaders(this.requestObj);
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache.ContainsKey("http://ruben.zhdk.ch/"));
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache.ContainsKey("http://ruben.zhdk.ch/"));
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheHsts.Instance.HstsCache.ContainsKey("http://ruben.zhdk.ch/"));
    }


    [Test]
    public void ProcessServerResponseHeaders_redirect_to_http()
    {
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/test.php";
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders = new Hashtable();
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Location", "http://ruben.zhdk.ch/test.php");

      // SslStrip plugin instruction: No cache record, procede
      RedirectType redir = this.sslStripObj.DetermineRedirectType(this.requestObj);
      Assert.IsTrue(redir == RedirectType.Http2Http3XX);

      // Caches are empty
      this.sslStripObj.ProcessServerResponseHeaders(this.requestObj);
      this.DumpSslStripCaches();
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheHsts.Instance.HstsCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
    }


    [Test]
    public void ProcessServerResponseHeaders_redirect_to_different_https_url()
    {
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/test.php";
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders = new Hashtable();
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Location", "https://nebur.zhdk.ch/test.php");

      // SslStrip plugin instruction: Cache http2https redirect
      RedirectType redir = this.sslStripObj.DetermineRedirectType(this.requestObj);
      Assert.IsTrue(redir == RedirectType.Http2Https3XXDifferentUrl);

      // Redirect cache contains record
      this.sslStripObj.ProcessServerResponseHeaders(this.requestObj);
      this.DumpSslStripCaches();
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
      Assert.IsTrue(HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache.ContainsKey("http://nebur.zhdk.ch/test.php"));
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheHsts.Instance.HstsCache.ContainsKey("http://ruben.zhdk.ch/test.php"));

    }


    [Test]
    public void ProcessServerResponseHeaders_redirect_to_same_https_url()
    {
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/test.php";
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders = new Hashtable();
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Location", string.Format("https://{0}{1}", this.requestObj.ClientRequestObj.Host, this.requestObj.ClientRequestObj.Path));

      // SslStrip plugin instruction: Reload current URL with HTTPS instead of HTTP
      RedirectType redir = this.sslStripObj.DetermineRedirectType(this.requestObj);
      Assert.IsTrue(redir == RedirectType.Http2Https3XXSameUrl);

      // Redirect cache contains record
      this.sslStripObj.ProcessServerResponseHeaders(this.requestObj);
      this.DumpSslStripCaches();
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
      Assert.IsTrue(HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheHsts.Instance.HstsCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
    }

    #endregion


    #region TESTS : HSTS

    [Test]
    public void Hsts_header_discovered()
    {
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      this.requestObj.ClientRequestObj.Path = "/test.php";
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders = new Hashtable();
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

      // No 302/301 location redirection, procede
      PluginInstruction pluginInstruction = this.sslStripObj.ProcessServerResponseHeaders(this.requestObj);
      Assert.IsTrue(pluginInstruction.Instruction == Instruction.DoNothing);

      // Redirect cache contains record
      this.DumpSslStripCaches();
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
      Assert.IsFalse(HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache.ContainsKey("http://nebur.zhdk.ch/test.php"));
      Assert.IsTrue(HttpReverseProxy.Plugin.SslStrip.Cache.CacheHsts.Instance.HstsCache.ContainsKey("http://ruben.zhdk.ch/test.php"));
    }

    #endregion


    #region TESTS : Errors

    [Test]
    public void ProcessServerResponseHeaders_Erroneous_redirect_location_scheme_ftps()
    {
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "buglist.io";
      this.requestObj.ClientRequestObj.Path = "/test.php";
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders = new Hashtable();
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Location", string.Format("ftps://{0}{1}", this.requestObj.ClientRequestObj.Host, this.requestObj.ClientRequestObj.Path));

      RedirectType redir = this.sslStripObj.DetermineRedirectType(this.requestObj);
      Assert.IsTrue(redir == RedirectType.Error);

    }


    [Test]
    public void ProcessServerResponseHeaders_Erroneous_redirect_location_broken_scheme()
    {
      this.requestObj.ClientRequestObj.Scheme = "http";
      this.requestObj.ClientRequestObj.Host = "ruben.zhdk";
      this.requestObj.ClientRequestObj.Path = "/test.php";
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders = new Hashtable();
      this.requestObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Location", string.Format("ftps:\\{0}{1}", this.requestObj.ClientRequestObj.Host, this.requestObj.ClientRequestObj.Path));

      RedirectType redir = this.sslStripObj.DetermineRedirectType(this.requestObj);
      Assert.IsTrue(redir == RedirectType.Error);
    }

    #endregion


    #region PRIVATE

    private void DumpSslStripCaches()
    {
      foreach (var tmpRecord in HttpReverseProxy.Plugin.SslStrip.Cache.CacheSslStrip.Instance.SslStripCache)
      {
        Console.WriteLine("SslStrip cache key:{0} Value:{1}", tmpRecord.Key, tmpRecord.Value);
      }

      foreach (var tmpRecord in HttpReverseProxy.Plugin.SslStrip.Cache.CacheRedirect.Instance.RedirectCache)
      {
        Console.WriteLine("Redirect cache key:{0} Value:{1}", tmpRecord.Key, tmpRecord.Value);
      }

      foreach (var tmpRecord in HttpReverseProxy.Plugin.SslStrip.Cache.CacheHsts.Instance.HstsCache)
      {
        Console.WriteLine("Hsts cache key:{0} Value:{1}", tmpRecord.Key, tmpRecord.Value);
      }
    }

    #endregion

  }
}