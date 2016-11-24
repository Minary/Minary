namespace TestsMinaryAttackServices
{
  using Minary.AttackService;
  using Minary.AttackService.DataTypes;
  using NUnit.Framework;
  using System.Linq;


  [TestFixture]
  public class TestsAttackServiceParameters
  {

    #region MEMBERS

    private AttackServiceHandler attackServiceHandler;

    #endregion


    #region NUNIT

    [OneTimeSetUp]
    public void TestSetup()
    {
      this.attackServiceHandler = new AttackServiceHandler(null);
    }

    [SetUp]
    public void SingleTestSetup()
    {
    }

    [TearDown]
    public void SingleTestTearDown()
    {
    }

    #endregion


    #region TESTS

    [Test]
    public void Attack_services_are_registered()
    {
      Assert.IsTrue(this.attackServiceHandler.AttackServices.ContainsKey(AttackService.ArpPoisoning.Name));
      Assert.IsTrue(this.attackServiceHandler.AttackServices.ContainsKey(AttackService.DataSniffer.Name));
      Assert.IsTrue(this.attackServiceHandler.AttackServices.ContainsKey(AttackService.HttpReverseProxyServer.Name));
    }

    [Test]
    public void ArpPoisoning_submodule_DnsPoisoning_is_registered()
    {
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules.ContainsKey(AttackService.ArpPoisoning.SubModule.DnsPoisoning));

      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules[AttackService.ArpPoisoning.SubModule.DnsPoisoning].WorkingDirectory.ToLower().StartsWith(@"c:\"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules[AttackService.ArpPoisoning.SubModule.DnsPoisoning].WorkingDirectory.ToLower().EndsWith(@"minary\services\ape"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules[AttackService.ArpPoisoning.SubModule.DnsPoisoning].ConfigFilePath.ToLower().EndsWith(@".dnshosts"));
   }

    [Test]
    public void ArpPoisoning_submodule_Firewall_is_registered()
    {
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules.ContainsKey(AttackService.ArpPoisoning.SubModule.Firewall));

      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules[AttackService.ArpPoisoning.SubModule.Firewall].WorkingDirectory.ToLower().StartsWith(@"c:\"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules[AttackService.ArpPoisoning.SubModule.Firewall].WorkingDirectory.ToLower().EndsWith(@"minary\services\ape"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.ArpPoisoning.Name].SubModules[AttackService.ArpPoisoning.SubModule.Firewall].ConfigFilePath.ToLower().EndsWith(@".fwrules"));
    }




    [Test]
    public void HttpReverseProxy_submodule_SslStrip_is_registered()
    {
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules.ContainsKey(AttackService.HttpReverseProxyServer.SubModule.SslStrip));

      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules[AttackService.HttpReverseProxyServer.SubModule.SslStrip].WorkingDirectory.ToLower().StartsWith(@"c:\"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules[AttackService.HttpReverseProxyServer.SubModule.SslStrip].WorkingDirectory.ToLower().EndsWith(@"minary\services\httpreverseproxy\plugins\sslstrip"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules[AttackService.HttpReverseProxyServer.SubModule.SslStrip].ConfigFilePath.ToLower().EndsWith(@"plugin.config"));
    }

    [Test]
    public void HttpReverseProxy_submodule_DataSniffer_is_registered()
    {
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules.ContainsKey(AttackService.HttpReverseProxyServer.SubModule.DataSniffer));

      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules[AttackService.HttpReverseProxyServer.SubModule.DataSniffer].WorkingDirectory.ToLower().StartsWith(@"c:\"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules[AttackService.HttpReverseProxyServer.SubModule.DataSniffer].WorkingDirectory.ToLower().EndsWith(@"minary\services\httpreverseproxy\plugins\datasniffer"));
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.HttpReverseProxyServer.Name].SubModules[AttackService.HttpReverseProxyServer.SubModule.DataSniffer].ConfigFilePath.ToLower().EndsWith(@"plugin.config"));
    }



    [Test]
    public void DataSniffer_submodules()
    {
      Assert.IsTrue(this.attackServiceHandler.AttackServices[AttackService.DataSniffer.Name].SubModules == null ||
                    this.attackServiceHandler.AttackServices[AttackService.DataSniffer.Name].SubModules.Count() == 0);
    }

    #endregion

  }
}
