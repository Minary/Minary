namespace TestsHttpReverseProxy.Plugin.SslStrip
{
  using HttpReverseProxy;
  using HttpReverseProxyLib.DataTypes;
  using NUnit.Framework;
  using System;
  using System.IO;
  using TestsHttpReverseProxy.Lib;


  [TestFixture]
  public class SslStrip
  {

    #region MEMBERS

    private static string workingDir = Directory.GetCurrentDirectory();
    private static string pluginsDir = "plugins";
    private static string dataInjectFileName = Path.GetTempFileName();

    private string pluginInjectDir = Path.Combine(workingDir, pluginsDir, "Inject");
    private string pluginSrcInjectDll = Path.Combine(workingDir, @"Inject\bin\debug\Inject.dll");
    private string pluginDstInjectDll = Path.Combine(workingDir, pluginsDir, "Inject", "Inject.dll");
    private string pluginConfigFileInject = Path.Combine(workingDir, pluginsDir, "Inject", "plugin.config");
    private string pluginConfigInject = string.Empty;

    private string pluginSslStripDir = Path.Combine(workingDir, pluginsDir, "SslStrip");
    private string pluginSrcSslStripDll = Path.Combine(workingDir, @"SslStrip\bin\debug\SslStrip.dll");
    private string pluginDstSslStripDll = Path.Combine(workingDir, pluginsDir, "SslStrip", "SslStrip.dll");
    private string pluginConfigFileSslStrip = Path.Combine(workingDir, pluginsDir, "SslStrip", "plugin.config");
    private string pluginConfigSslStrip = "ruben.zhdk.ch:text/html\r\n" +
                                           "www.ruben.zhdk.ch:text/html";

    private string pluginWeakenDir = Path.Combine(workingDir, pluginsDir, "Weaken");
    private string pluginSrcWeakenDll = Path.Combine(workingDir, @"Weaken\bin\debug\Weaken.dll");
    private string pluginDstWeakenDll = Path.Combine(workingDir, pluginsDir, "Weaken", "Weaken.dll");
    private string pluginConfigFileWeaken = Path.Combine(workingDir, pluginsDir, "Weaken", "plugin.config");
    private string pluginConfigWeaken = string.Empty;

    private string dataInjectFileContent = "Randomdata Randomdata Randomdata Randomdata Randomdata Randomdata Randomdata Randomdata Randomdata ...";
    private int serverPort = 6789;

    #endregion


    #region NUNIT

    [OneTimeSetUp]
    public void EnvironmentSetup()
    {
    }


    [OneTimeTearDown]
    public void EnvironmentTearDown()
    {
    }


    [SetUp]
    public void SingleTestSetup()
    {
      FileAction.CreateDirectoryIfNotExists(this.pluginInjectDir);
      FileAction.CopyFileIfNotExists(this.pluginSrcInjectDll, this.pluginDstInjectDll);
      FileAction.WriteToFile(this.pluginConfigFileInject, this.pluginConfigInject);

      FileAction.CreateDirectoryIfNotExists(this.pluginSslStripDir);
      FileAction.CopyFileIfNotExists(this.pluginSrcSslStripDll, this.pluginDstSslStripDll);
      FileAction.WriteToFile(this.pluginConfigFileSslStrip, this.pluginConfigSslStrip);

      FileAction.CreateDirectoryIfNotExists(this.pluginWeakenDir);
      FileAction.CopyFileIfNotExists(this.pluginSrcWeakenDll, this.pluginDstWeakenDll);
      FileAction.WriteToFile(dataInjectFileName, this.dataInjectFileContent);

      TestLib.Instance.ProxyServerInst.Start(this.serverPort);
    }

    [TearDown]
    public void SingleTestTearDown()
    {
      TestLib.Instance.ProxyServerInst.Stop();

      FileAction.RemoveFileIfExists(this.pluginDstInjectDll);
      FileAction.RemoveFileIfExists(this.pluginConfigInject);
      FileAction.RemoveDirectoryIfExists(this.pluginInjectDir);

      FileAction.RemoveFileIfExists(this.pluginDstSslStripDll);
      FileAction.RemoveFileIfExists(this.pluginConfigFileSslStrip);
      FileAction.RemoveDirectoryIfExists(this.pluginSslStripDir);

      FileAction.RemoveFileIfExists(this.pluginDstWeakenDll);
      FileAction.RemoveFileIfExists(this.pluginConfigFileWeaken);
      FileAction.RemoveDirectoryIfExists(this.pluginWeakenDir);

      string pluginsPath = Path.Combine(workingDir, pluginsDir);
      FileAction.RemoveDirectoryIfExists(pluginsPath);

      FileAction.RemoveFileIfExists(dataInjectFileName);
    }

    #endregion


    #region TESTS : SslStrip

    [Test]
    public void SslStrip_all_tags()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      RequestHandler requestHandler = new RequestHandler(requestObj);

      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/SSLStripping.php", "HTTP/1.1", httpRequestData, "\n");
      Assert.That(serverResponse, Does.Contain(@"<a href=""http://ruben.zhdk.ch/Anchor1.html"" >"));
      Console.WriteLine("serverResponse: {0}", serverResponse);
    }


    [Test]
    public void SslStrip_anchor_tags_containing_a_newline()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      RequestHandler requestHandler = new RequestHandler(requestObj);

      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/SSLStripping.php", "HTTP/1.1", httpRequestData, "\n");
      Assert.That(serverResponse, Does.Contain(@"<a href=""http://ruben.zhdk.ch/Anchor1.html"" >"));
      Assert.That(serverResponse, Does.Contain(@"<a href=""http://www.ruben.zhdk.ch/Anchor2.html"" >"));
      Assert.That(serverResponse, Does.Contain(@"<a href=""http://www.ruben.zhdk.ch/Anchor5.html"""));
      Console.WriteLine("serverResponse: {0}", serverResponse);
    }


    [Test]
    public void SslStrip_all_other_tags()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      RequestHandler requestHandler = new RequestHandler(requestObj);

      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/tests/SSLStripping.php", "HTTP/1.1", httpRequestData, "\n");
      Assert.That(serverResponse, Does.Contain(@"<form action=""http://ruben.zhdk.ch/"" >"));
      Assert.That(serverResponse, Does.Contain(@"<img src=""http://ruben.zhdk.ch/"" >"));
      Assert.That(serverResponse, Does.Contain(@"<base href=""http://ruben.zhdk.ch/"" >"));
      Assert.That(serverResponse, Does.Contain(@"<link href=""http://ruben.zhdk.ch/"" >"));
      Assert.That(serverResponse, Does.Contain(@"<script src=""http://ruben.zhdk.ch/"" >"));
      Console.WriteLine("serverResponse: {0}", serverResponse);
    }

    #endregion

  }


}
