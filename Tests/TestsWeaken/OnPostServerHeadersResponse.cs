namespace TestsWeaken
{
  using HttpReverseProxy.Plugin.Weaken;
  using HttpReverseProxyLib.DataTypes;
  using HttpReverseProxyLib.Exceptions;
  using NUnit.Framework;


  [TestFixture]
  public class OnPostServerHeadersResponse
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

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => weakenObj.OnPostServerHeadersResponse(null));
      Assert.That(pwex.Message, Does.Contain("The request object is invalid"));
    }

    [Test]
    public void RequestObject_serverresponse_headers_null()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = null;

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);
    }

    [Test]
    public void RequestObject_serverresponse_headers_empty()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = new System.Collections.Hashtable();

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);
    }

    #endregion


    #region TESTS : HSTS

    [Test]
    public void Ignore_nonHSTS_headers()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = new System.Collections.Hashtable();
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Server", "Apache/2.4.10 (Debian)");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);

      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.ContainsKey("Server"));
    }


    [Test]
    public void Remove_HSTS_header()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = new System.Collections.Hashtable();
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("strict-transport-security", "max-age=31536000");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);

      Assert.IsFalse(reqObj.ServerResponseMetaDataObj.ResponseHeaders.ContainsKey("strict-transport-security"));
    }

    #endregion


    #region TESTS : Set-Cookie

    [Test]
    public void Remove_secure_cookie_attribute()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = new System.Collections.Hashtable();
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Server", "Apache/2.4.10 (Debian)");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Set-Cookie", "_sessid=laskdflaksjflaksjf; path=/; secure; HttpOnly");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);

      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Count == 2);
      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Contains("Set-Cookie"));
      Assert.IsFalse(reqObj.ServerResponseMetaDataObj.ResponseHeaders["Set-Cookie"].ToString().Contains("secure"));
    }

    [Test]
    public void Remove_multiple_secure_cookie_attributes()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = new System.Collections.Hashtable();
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Server", "Apache/2.4.10 (Debian)");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Set-Cookie", "_sessid=laskdflaksjflaksjf; path=/; secure; HttpOnly; secure");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);

      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Count == 2);
      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Contains("Set-Cookie"));
      Assert.IsFalse(reqObj.ServerResponseMetaDataObj.ResponseHeaders["Set-Cookie"].ToString().Contains("secure"));
    }


    [Test]
    public void Remove_httponly_cookie_attribute()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = new System.Collections.Hashtable();
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Server", "Apache/2.4.10 (Debian)");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Set-Cookie", "_sessid=laskdflaksjflaksjf; path=/; secure; HttpOnly");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);

      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Count == 2);
      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Contains("Set-Cookie"));
      Assert.IsFalse(reqObj.ServerResponseMetaDataObj.ResponseHeaders["Set-Cookie"].ToString().Contains("HttpOnly"));
    }

    [Test]
    public void Remove_multiple_httponly_cookie_attributes()
    {
      RequestObj reqObj = new RequestObj("ruben.zhdk.ch");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders = new System.Collections.Hashtable();
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Server", "Apache/2.4.10 (Debian)");
      reqObj.ServerResponseMetaDataObj.ResponseHeaders.Add("Set-Cookie", "_sessid=laskdflaksjflaksjf; path=/; secure; HttpOnly; HttpOnly; secure");

      Weaken weakenObj = new HttpReverseProxy.Plugin.Weaken.Weaken();
      weakenObj.OnPostServerHeadersResponse(reqObj);

      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Count == 2);
      Assert.IsTrue(reqObj.ServerResponseMetaDataObj.ResponseHeaders.Contains("Set-Cookie"));
      Assert.IsFalse(reqObj.ServerResponseMetaDataObj.ResponseHeaders["Set-Cookie"].ToString().Contains("HttpOnly"));
    }
    #endregion

  }
}
