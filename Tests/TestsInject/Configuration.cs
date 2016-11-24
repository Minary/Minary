using HttpReverseProxy.Plugin.Inject;
using HttpReverseProxy.Plugin.Inject.DataTypes;
using HttpReverseProxyLib.Exceptions;
using NUnit.Framework;

namespace TestsInject
{

  [TestFixture]
  public class Configuration : Config
  {

    #region MEMBERS
    

    #endregion


    #region NUNIT


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
    
    [Test]
    public void Config_record_line_is_null()
    {
      ProxyWarningException ex = Assert.Throws<ProxyWarningException>(() => { InjectConfigRecord recordParams = this.VerifyRecordParameters(null); });
      Assert.That(ex.Message, Does.Contain("Configuration line is invalid"));
    }

    [Test]
    public void Config_record_line_is_empty()
    {
      ProxyWarningException ex = Assert.Throws<ProxyWarningException>(() => { InjectConfigRecord recordParams = this.VerifyRecordParameters(string.Empty); });
      Assert.That(ex.Message, Does.Contain("Configuration line is invalid"));
    }


    [Test]
    public void No_colon_in_record()
    {
      string configRecordLine = "an invalid config record line";
      ProxyWarningException ex = Assert.Throws<ProxyWarningException>(() => { InjectConfigRecord recordParams = this.VerifyRecordParameters(configRecordLine); });
      Assert.That(ex.Message, Does.Contain("Wrong numbers of configuration parameters"));
    }

    [Test]
    public void Configuration_parameters_missing()
    {
      string configRecordLine = "URL:www.spin.ch:/index.html";
      ProxyWarningException ex = Assert.Throws<ProxyWarningException>(() => { InjectConfigRecord recordParams = this.VerifyRecordParameters(configRecordLine); });
      Assert.That(ex.Message, Does.Contain("Wrong numbers of configuration parameters"));
    }

    [Test]
    public void Too_many_configuration_parameters()
    {
      string configRecordLine = "URL:www.spin.ch:/index.html:www.test.ch/";
      dynamic recordParams = this.VerifyRecordParameters(configRecordLine);

      Assert.IsTrue(recordParams.Type == InjectType.URL);
      Assert.IsTrue(recordParams.Host == "www.spin.ch");
      Assert.IsTrue(recordParams.Path == "/index.html");
      Assert.IsTrue(recordParams.ReplacementResource == "www.test.ch/");
    }

    [Test]
    public void Wrong_resource_replacement_type()
    {
      string configRecordLine = "Wire:www.spin.ch:/index.html:www.test.ch/";
      ProxyWarningException ex = Assert.Throws<ProxyWarningException>(() => this.VerifyRecordParameters(configRecordLine));
      Assert.That(ex.Message, Does.Contain("Replacement type parameter is invalid"));
    }

    [Test]
    public void Correct_record()
    {
      string configRecordLine = "URL:www.spin.ch:/index.html:www.test.ch/";
      InjectConfigRecord recordParams = this.VerifyRecordParameters(configRecordLine);

      Assert.IsTrue(recordParams.Type == InjectType.URL);
      Assert.IsTrue(recordParams.Host == "www.spin.ch");
      Assert.IsTrue(recordParams.Path == "/index.html");
      Assert.IsTrue(recordParams.ReplacementResource == "www.test.ch/");
    }
    #endregion


    #region TESTS : 


    #endregion 

  }
}
