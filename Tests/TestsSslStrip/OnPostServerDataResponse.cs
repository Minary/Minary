namespace TestsSslStrip
{
  using HttpReverseProxy.Plugin.SslStrip;
  using HttpReverseProxy.Plugin.SslStrip.Cache;
  using HttpReverseProxy.Plugin.SslStrip.DataTypes.Configuration;
  using HttpReverseProxyLib.DataTypes;
  using HttpReverseProxyLib.Exceptions;
  using NUnit.Framework;
  using System.IO;
  using System.Text;

  [TestFixture]
  public class OnPostServerDataResponse
  {

    #region MEMBERS

    private SslStrip sslStripObj;
    private string temporaryInputFile;
    //// private Config sslStripConfig;

    #endregion


    #region NUNIT

    [OneTimeSetUp]
    public void TestFixtureSetup()
    {
      this.temporaryInputFile = Path.GetTempFileName();
    }

    [SetUp]
    public void SingleTestSetup()
    {
      this.sslStripObj = new SslStrip();

      if (File.Exists(this.temporaryInputFile))
      {
        File.Delete(this.temporaryInputFile);
      }
    }

    [TearDown]
    public void SingleTestTearDown()
    {
      CacheHsts.Instance.ResetCache();
      CacheRedirect.Instance.ResetCache();
      CacheSslStrip.Instance.ResetCache();

      if (File.Exists(this.temporaryInputFile))
      {
        File.Delete(this.temporaryInputFile);
      }
    }

    #endregion


    #region TESTS : Preconditions

    [Test]
    public void Requestobject_is_null()
    {
      RequestObj requestObj = null;
      Encoding encoding = Encoding.UTF8;
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");

      DataPacket dataPacket = new DataPacket(data, encoding);
      dataPacket.DataEncoding = Encoding.UTF8;

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket));
      Assert.That(pwex.Message, Does.Contain("The request object is invalid"));
    }

    [Test]
    public void ServerResponseMetaDataObj_is_null()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj = null;

      Encoding encoding = Encoding.UTF8;
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");

      DataPacket dataPacket = new DataPacket(data, encoding);
      dataPacket.DataEncoding = Encoding.UTF8;

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => sslStripObj.OnPostServerDataResponse(requestObj, dataPacket));
      Assert.That(pwex.Message, Does.Contain("The meta data object is invalid"));
    }

    [Test]
    public void DataPacket_is_null()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/html";
      Encoding encoding = Encoding.UTF8;
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");
      DataPacket dataPacket = null;

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket));
      Assert.That(pwex.Message, Does.Contain("The data packet is invalid"));
    }

    [Test]
    public void ContentType_is_null()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = null;
      Encoding encoding = Encoding.UTF8;
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");

      DataPacket dataPacket = new DataPacket(data, encoding);
      dataPacket.DataEncoding = Encoding.UTF8;

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket));
      Assert.That(pwex.Message, Does.Contain("The server response content type is invalid"));
    }

    #endregion


    #region TESTS

    [Test]
    public void SslStrip_skipped_because_of_contenttype_missmatch()
    {
      // Initialize plugin configuration
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);
      this.sslStripObj.PluginConfig.ParseConfigurationFile(this.temporaryInputFile);

      // Initialize request object
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/plain";
      HtmlTagConfig htmlTagConfig1 = new HtmlTagConfig();
      htmlTagConfig1.TagList.TryAdd("a", true);

      // Initialize data packet
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");
      Encoding encoding = Encoding.UTF8;
      DataPacket dataPacket = new DataPacket(data, encoding);

      // Call target methodString and verify result
      this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket);

      Assert.IsTrue(dataPacket.ContentData.Length == data.Length);
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData) == Encoding.ASCII.GetString(data));

    }

    [Test]
    public void SslStrip_skipped_because_of_tag_missmatch()
    {
      // Initialize plugin configuration
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);
      this.sslStripObj.PluginConfig.ParseConfigurationFile(this.temporaryInputFile);

      // Initialize request object
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/html";
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentCharsetEncoding = Encoding.UTF8;

      // Initialize data packet
      byte[] data = Encoding.ASCII.GetBytes("<anchor href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");
      Encoding encoding = Encoding.UTF8;
      DataPacket dataPacket = new DataPacket(data, encoding);

      // Call target methodString and verify result
      this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket);

      Assert.IsTrue(dataPacket.ContentData.Length == data.Length);
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData) == Encoding.ASCII.GetString(data));
    }


    [Test]
    public void SslStrip_single_anchor_record()
    {
      // Initialize plugin configuration
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);
      this.sslStripObj.PluginConfig.ParseConfigurationFile(this.temporaryInputFile);

      // Initialize request object
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/html";
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentCharsetEncoding = Encoding.UTF8;

      // Initialize data packet
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");
      Encoding encoding = Encoding.UTF8;
      DataPacket dataPacket = new DataPacket(data, encoding);

      // Call target methodString and verify result
      this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket);

      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData) != Encoding.ASCII.GetString(data));
      Assert.IsTrue(dataPacket.ContentData.Length != data.Length);
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData), Does.Contain("<a href=\"http://"));
      Assert.IsFalse(Encoding.ASCII.GetString(dataPacket.ContentData).Contains("<a href=\"https://"));
    }



    [Test]
    public void SslStrip_two_identical_anchor_records()
    {
      // Initialize plugin configuration
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);
      this.sslStripObj.PluginConfig.ParseConfigurationFile(this.temporaryInputFile);

      // Initialize request object
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/html";
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentCharsetEncoding = Encoding.UTF8;

      // Initialize data packet
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a> " +
                                            "<body>hello <b>world</b> " +
                                            "<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a> " +
                                            "</body></html>");
      Encoding encoding = Encoding.UTF8;
      DataPacket dataPacket = new DataPacket(data, encoding);

      // Call target methodString and verify result
      this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket);

      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData) != Encoding.ASCII.GetString(data));
      Assert.IsTrue(dataPacket.ContentData.Length != data.Length);
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData), Does.Contain("<a href=\"http://ruben.zhdk.ch"));
      Assert.IsFalse(Encoding.ASCII.GetString(dataPacket.ContentData).Contains("<a href=\"https://ruben.zhdk.ch"));
    }


    [Test]
    public void SslStrip_multiple_different_anchor_records()
    {
      // Initialize plugin configuration
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);
      this.sslStripObj.PluginConfig.ParseConfigurationFile(this.temporaryInputFile);

      // Initialize request object
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/html";
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentCharsetEncoding = Encoding.UTF8;

      // Initialize data packet
      byte[] data = Encoding.ASCII.GetBytes("<a href=\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a> " +
                                            "<body>hello <b>world</b> " +
                                            "<a href=\"https://nebur.zhdk.ch/tests/input.php\">nebur.zhdk.ch</a> " +
                                            "</body></html>");
      Encoding encoding = Encoding.UTF8;
      DataPacket dataPacket = new DataPacket(data, encoding);

      // Call target methodString and verify result
      this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket);

      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData) != Encoding.ASCII.GetString(data));
      Assert.IsTrue(dataPacket.ContentData.Length != data.Length);
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData), Does.Contain("<a href=\"http://ruben.zhdk.ch"));
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData), Does.Contain("<a href=\"https://nebur.zhdk.ch"));
    }



    [Test]
    public void SslStrip_strip_tag_containting_a_newline()
    {
      // Initialize plugin configuration
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);
      this.sslStripObj.PluginConfig.ParseConfigurationFile(this.temporaryInputFile);

      // Initialize request object
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/html";
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentCharsetEncoding = Encoding.UTF8;

      // Initialize data packet
      byte[] data = Encoding.ASCII.GetBytes(string.Format("<a href={0}\"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>", System.Environment.NewLine));
      Encoding encoding = Encoding.UTF8;
      DataPacket dataPacket = new DataPacket(data, encoding);

      // Call target methodString and verify result
      this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket);

      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData) != Encoding.ASCII.GetString(data));
      Assert.IsTrue(dataPacket.ContentData.Length != data.Length);
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData), Does.Contain(string.Format("<a href={0}\"http://", System.Environment.NewLine)));
      Assert.IsFalse(Encoding.ASCII.GetString(dataPacket.ContentData).Contains(string.Format("<a href={0}\"https://", System.Environment.NewLine)));
    }


    [Test]
    public void SslStrip_strip_tag_containting_useless_whitespaces()
    {
      // Initialize plugin configuration
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);
      this.sslStripObj.PluginConfig.ParseConfigurationFile(this.temporaryInputFile);

      // Initialize request object
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentType = "text/html";
      requestObj.ServerResponseMetaDataObj.ContentTypeEncoding.ContentCharsetEncoding = Encoding.UTF8;

      // Initialize data packet
      byte[] data = Encoding.ASCII.GetBytes("<a href   = \"https://ruben.zhdk.ch/tests/input.php\">ruben.zhdk.ch</a>");
      Encoding encoding = Encoding.UTF8;
      DataPacket dataPacket = new DataPacket(data, encoding);

      // Call target methodString and verify result
      this.sslStripObj.OnPostServerDataResponse(requestObj, dataPacket);

      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData) != Encoding.ASCII.GetString(data));
      Assert.IsTrue(dataPacket.ContentData.Length != data.Length);
      Assert.That(Encoding.ASCII.GetString(dataPacket.ContentData), Does.Contain("<a href   = \"http://"));
      Assert.IsFalse(Encoding.ASCII.GetString(dataPacket.ContentData).Contains("<a href=\"https://"));
    }

    #endregion

  }
}
