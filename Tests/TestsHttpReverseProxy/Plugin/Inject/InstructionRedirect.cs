using HttpReverseProxy;
using HttpReverseProxyLib.DataTypes;
using NUnit.Framework;
using System;
using System.IO;
using TestsHttpReverseProxy.Lib;


namespace TestsHttpReverseProxy.Plugin.Inject
{

  [TestFixture]
  public class InstructionRedirect
  {

    #region MEMBERS

    private static string workingDir = Directory.GetCurrentDirectory();
    private static string pluginsDir = "plugins";

    private string pluginInjectDir = Path.Combine(workingDir, pluginsDir, "Inject");
    private string pluginSrcInjectDll = Path.Combine(workingDir, @"Inject\bin\debug\Inject.dll");
    private string pluginDstInjectDll = Path.Combine(workingDir, pluginsDir, "Inject", "Inject.dll");
    private string pluginConfigFileInject = Path.Combine(workingDir, pluginsDir, "Inject", "plugin.config");
    private string pluginConfigInject = @"File:www.deep.ch:/index.php:c:\temp\honk.log\r\n" +
                                        "URL:www.spin.ch:/index.php:http://www.google.com/\r\n" +
                                        "URL:ruben.zhdk.ch:/index.php:http://heise.de/";

    private string pluginSslStripDir = Path.Combine(workingDir, pluginsDir, "SslStrip");
    private string pluginSrcSslStripDll = Path.Combine(workingDir, @"SslStrip\bin\debug\SslStrip.dll");
    private string pluginDstSslStripDll = Path.Combine(workingDir, pluginsDir, "SslStrip", "SslStrip.dll");
    private string pluginConfigFileSslStrip = Path.Combine(workingDir, pluginsDir, "SslStrip", "plugin.config");
    private string pluginConfigSslStrip = string.Empty;

    private string pluginWeakenDir = Path.Combine(workingDir, pluginsDir, "Weaken");
    private string pluginSrcWeakenDll = Path.Combine(workingDir, @"Weaken\bin\debug\Weaken.dll");
    private string pluginDstWeakenDll = Path.Combine(workingDir, pluginsDir, "Weaken", "Weaken.dll");
    private string pluginConfigFileWeaken = Path.Combine(workingDir, pluginsDir, "Weaken", "plugin.config");
    private string pluginConfigWeaken = string.Empty;

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
      FileAction.WriteToFile(this.pluginConfigFileSslStrip, this.pluginConfigFileSslStrip);

      FileAction.CreateDirectoryIfNotExists(this.pluginWeakenDir);
      FileAction.CopyFileIfNotExists(this.pluginSrcWeakenDll, this.pluginDstWeakenDll);

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
    }

    #endregion


    #region TESTS : Inject redirect

    [Test]
    public void Injection_unsuccessful_because_host_matches_and_path_doesnt_match()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      RequestHandler requestHandler = new RequestHandler(requestObj);

      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/hotindex.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);
      Assert.That(serverResponse, Does.Not.Contain("Location: http://heise.de/"));
    }


    [Test]
    public void Injection_unsuccessful_because_host_does_not_match_and_path_matches()
    {
      RequestObj requestObj = new RequestObj("www.zhdk.ch");
      RequestHandler requestHandler = new RequestHandler(requestObj);

      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: www.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "www.zhdk.ch", this.serverPort, "/index.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);
      Assert.That(serverResponse, Does.Not.Contain("Location: http://heise.de/"));
    }


    [Test]
    public void Injection_successful_because_host_matches_and_path_matches()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      RequestHandler requestHandler = new RequestHandler(requestObj);

      HttpTestRequest httpRequestData = new HttpTestRequest();
      httpRequestData.Data = new string[] { };
      httpRequestData.Headers = new string[] { "Host: ruben.zhdk.ch",
                                        "Accept-Encoding: ",
                                        "Content-Type: text/html",
                                        "Connection: Close" };
      string serverResponse = TestLib.Instance.RawHttpGetRequest("GET", "ruben.zhdk.ch", this.serverPort, "/index.php", "HTTP/1.1", httpRequestData, "\n");
      Console.WriteLine("serverResponse: {0}", serverResponse);
      Assert.That(serverResponse, Does.Contain("HTTP/1.1 302 Found"));
      Assert.That(serverResponse, Does.Contain("Location: http://heise.de/"));
    }

    #endregion

  }
}
