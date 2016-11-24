namespace TestsSslStrip
{
  using HttpReverseProxy.Plugin.SslStrip;
  using HttpReverseProxy.Plugin.SslStrip.DataTypes;
  using HttpReverseProxyLib.Exceptions;
  using NUnit.Framework;
  using System.IO;
  using System.Text.RegularExpressions;

  [TestFixture]
  public class Configuration : Config
  {

    #region MEMBERS

    private string temporaryInputFile;

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
      try
      {
        if (File.Exists(this.temporaryInputFile))
        {
          File.Delete(this.temporaryInputFile);
        }
      }
      catch
      {
      }
    }


    [TearDown]
    public void SingleTestTearDown()
    {
      try
      {
        if (File.Exists(this.temporaryInputFile))
        {
          File.Delete(this.temporaryInputFile);
        }
      }
      catch
      {
      }
    }


    #endregion


    #region TESTS : Config file

    [Test]
    public void Config_file_null()
    {
      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.ParseConfigurationFile(null));
      Assert.That(pwex.Message, Does.Contain("Config file path is invalid"));
    }

    [Test]
    public void Config_file_empty()
    {
      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.ParseConfigurationFile(string.Empty));
      Assert.That(pwex.Message, Does.Contain("Config file path is invalid"));
    }

    [Test]
    public void Config_file_does_not_exist()
    {
      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.ParseConfigurationFile(@"c:\erjthygfrhjekwah.txt"));
      Assert.That(pwex.Message, Does.Contain("Config file does not exist"));
    }

    #endregion


    #region TESTS : Input data validation

    // Line is null/empty
    [Test]
    public void Config_file_line_is_null()
    {
      string inputLine = null;
      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(inputLine));
      Assert.That(pwex.Message, Does.Contain("Configuration line is invalid"));
    }

    [Test]
    public void Config_file_line_is_empty()
    {
      string inputLine = string.Empty;
      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(inputLine));
      Assert.That(pwex.Message, Does.Contain("Configuration line is invalid"));
    }


    [Test]
    public void Input_structure_has_too_many_elements()
    {
      string inputLine = "ruben.zhdk.ch:text/html:INVALIDELEMENT";

      SslStripConfigRecord configRecord = this.VerifyRecordParameters(inputLine);
      Assert.IsTrue(configRecord.Host == "ruben.zhdk.ch");
      Assert.IsTrue(configRecord.ContentType == "text/html:INVALIDELEMENT");
    }

    [Test]
    public void Input_structure_has_too_few_elements()
    {
      string inputLine = "ruben.zhdk.ch";

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(inputLine));
      Assert.That(pwex.Message, Does.Contain("Configuration is invalid"));
    }




    [Test]
    public void Host_parameter_is_invalid()
    {
      string inputLine = ":text/html";

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(inputLine));
      Assert.That(pwex.Message, Does.Contain("Host parameter is invalid:"));
    }

    [Test]
    public void Host_parameter_has_white_spaces()
    {
      string inputLine = " :text/html";

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(inputLine));
      Assert.That(pwex.Message, Does.Contain("Host parameter is invalid:"));
    }




    [Test]
    public void ContentType_parameter_is_invalid()
    {
      string inputLine = "ruben.zhdk.ch:";

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(inputLine));
      Assert.That(pwex.Message, Does.Contain("MIME-Type parameter is invalid:"));
    }

    [Test]
    public void ContentType_parameter_has_white_spaces()
    {
      string inputLine = "ruben.zhdk.ch: ";

      ProxyWarningException pwex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(inputLine));
      Assert.That(pwex.Message, Does.Contain("MIME-Type parameter is invalid:"));
    }

    #endregion


    #region TESTS : Configuration parameter structure

    [Test]
    public void Input_structure_add_single_rule()
    {
      string inputLine = "ruben.zhdk.ch:text/html";
      File.AppendAllText(this.temporaryInputFile, inputLine);

      this.ParseConfigurationFile(this.temporaryInputFile);

      Assert.IsTrue(this.SearchPatterns.ContainsKey("text/html"));
      Assert.IsTrue(this.SearchPatterns["text/html"].Count > 0);
      foreach (string tmpRegex in this.SearchPatterns["text/html"])
      {
        Assert.IsTrue(tmpRegex.Contains(@Regex.Escape("https://ruben.zhdk.ch")));
      }
    }

    #endregion

  }
}
