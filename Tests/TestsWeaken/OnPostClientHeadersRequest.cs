namespace TestsWeaken
{
  using HttpReverseProxy.Plugin.Weaken;
  using HttpReverseProxyLib.DataTypes;
  using HttpReverseProxyLib.Exceptions;
  using NUnit.Framework;


  [TestFixture]
  public class OnPostClientHeadersRequest
  {

    #region MEMBERS


    #endregion


    #region NUNIT

    [OneTimeSetUp]
    public void TestFixtureSetup()
    {
    }


    [SetUp]
    public void SingleTestSetup()
    {
    }

    #endregion


    #region TESTS : Preconditions

    [Test]
    public void RequestObject_empty()
    {
      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => weakenObj.OnPostClientHeadersRequest(null));
      Assert.That(pwex.Message, Does.Contain("The request object is invalid"));
    }

    [Test]
    public void RequestObject_clientrequest_headers_null()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = null;

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);
    }

    [Test]
    public void RequestObject_clientrequest_headers_empty()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = new System.Collections.Hashtable();

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);
    }

    #endregion


    #region TESTS : Connection keep-alive
    /*
    [Test]
    public void RequestObject_ignore_connection_header_close()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = new System.Collections.Hashtable();
      reqObj.ClientRequestObj.ClientRequestHeaders.Add("Connection", "close");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);

      Assert.IsTrue(reqObj.ClientRequestObj.ClientRequestHeaders["Connection"].ToString() == "close");
    }

    [Test]
    public void RequestObject_replace_connection_header_keepalive()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = new System.Collections.Hashtable();
      reqObj.ClientRequestObj.ClientRequestHeaders.Add("Connection", "keep-alive");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);

      Assert.IsTrue(reqObj.ClientRequestObj.ClientRequestHeaders["Connection"].ToString() == "close");
    }
    */
    #endregion


    #region TESTS : Upgrade

    [Test]
    public void RequestObject_ignore_upgrade_nonTLS()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = new System.Collections.Hashtable();
      reqObj.ClientRequestObj.ClientRequestHeaders.Add("Upgrade", "SameSong");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);

      Assert.IsTrue(reqObj.ClientRequestObj.ClientRequestHeaders["Upgrade"].ToString() == "SameSong");
    }

    [Test]
    public void RequestObject_remove_upgrade_TLS()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = new System.Collections.Hashtable();
      reqObj.ClientRequestObj.ClientRequestHeaders.Add("Upgrade", "TLS.1.3");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);

      Assert.IsFalse(reqObj.ClientRequestObj.ClientRequestHeaders.ContainsKey("Connection"));
    }

    #endregion


    #region TESTS : Accept-Encoding

    [Test]
    public void RequestObject_ignore_noncompression_encoding()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = new System.Collections.Hashtable();
      reqObj.ClientRequestObj.ClientRequestHeaders.Add("Accept-Encoding", "SameSong");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);

      Assert.IsTrue(reqObj.ClientRequestObj.ClientRequestHeaders["Accept-Encoding"].ToString() == "SameSong");
    }

    [Test]
    public void RequestObject_remove_compression_encoding()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ClientRequestObj.ClientRequestHeaders = new System.Collections.Hashtable();
      reqObj.ClientRequestObj.ClientRequestHeaders.Add("Accept-Encoding", "gzip, deflate");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostClientHeadersRequest(reqObj);

      Assert.IsFalse(reqObj.ClientRequestObj.ClientRequestHeaders.ContainsKey("Accept-Encoding"));
    }

    #endregion

  }
}
