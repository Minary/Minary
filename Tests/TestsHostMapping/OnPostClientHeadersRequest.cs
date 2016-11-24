namespace TestsHttpMapping
{
  using HttpReverseProxy.Plugin.HostMapping;
  using HttpReverseProxyLib.DataTypes;
  using HttpReverseProxyLib.Exceptions;
  using NUnit.Framework;
  using System;
  using System.Collections.Generic;


  [TestFixture]
  public class OnPostClientHeadersRequest : HostMapping
  {

    #region MEMBERS



    #endregion


    #region NUNIT

    [SetUp]
    public void TestSetup()
    {
      HttpReverseProxy.Plugin.HostMapping.Config.Mappings = new Dictionary<string, Tuple<string, string>>();
      HttpReverseProxy.Plugin.HostMapping.Config.Mappings.Clear();
    }

    #endregion


    #region TESTS : Preconditions

    [Test]
    public void Request_object_is_null()
    {
      Exception ex = Assert.Throws<ProxyWarningException>(() => this.OnPostClientHeadersRequest(null));
      Assert.That(ex.Message, Does.Contain("The request object is invalid"));
    }


    [Test]
    public void Request_headers_is_null()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders = null;
      this.OnPostClientHeadersRequest(requestObj);
    }

    [Test]
    public void Request_headers_is_empty()
    {
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      this.OnPostClientHeadersRequest(requestObj);
    }

    [Test]
    public void HostMapping_record_list_is_null()
    {
      // Inject plugin setup
      HttpReverseProxy.Plugin.HostMapping.Config.Mappings = null;
      
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Connection", "close");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "Honk honk");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://ruben.zhdk.ch/");
      this.OnPostClientHeadersRequest(requestObj);
    }

    [Test]
    public void No_host_header_found()
    {
      // HostMapping plugin setup
      HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord record = new HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord();
      record.RequestedHost = "www.ruben.zhdk.ch";
      record.MappingHost = "zhdk.ch";
      HttpReverseProxy.Plugin.HostMapping.Config.Mappings.Add(record.RequestedHost, new Tuple<string, string>("http", record.MappingHost));

      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Connection", "close");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "Honk honk");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://ruben.zhdk.ch/");
      this.OnPostClientHeadersRequest(requestObj);
    }

    #endregion


    #region TESTS : Unsuccessful

    [Test]
    public void Requested_host_not_in_mapping_list()
    {
      // HostMapping plugin setup
      HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord record = new HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord();
      record.RequestedHost = "ruben.zhdk.ch";
      record.MappingHost = "zhdk.ch";
      HttpReverseProxy.Plugin.HostMapping.Config.Mappings.Add(record.RequestedHost, new Tuple<string, string>("http", record.MappingHost));

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "notruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Scheme = "http";
      requestObj.ClientRequestObj.Host = "notruben.zhdk.ch";
      requestObj.ClientRequestObj.Path = "/notindex.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.DoNothing);
      Assert.IsTrue(requestObj.ClientRequestObj.ClientRequestHeaders["Host"].ToString() == "notruben.zhdk.ch");
      Assert.IsTrue(requestObj.ClientRequestObj.Scheme == "http");
      Assert.IsTrue(requestObj.ClientRequestObj.Host == "notruben.zhdk.ch");
    }

    #endregion


    #region TESTS : Successful
    
    [Test]
    public void Requested_host_in_mapping_list_http()
    {
      // HostMapping plugin setup
      HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord record = new HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord();
      record.RequestedHost = "ruben.zhdk.ch";
      record.MappingHost = "zhdk.ch";
      HttpReverseProxy.Plugin.HostMapping.Config.Mappings.Add(record.RequestedHost, new Tuple<string, string>("http", record.MappingHost));

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Scheme = "http";
      requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      requestObj.ClientRequestObj.Path = "/notindex.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.DoNothing);
      Assert.IsTrue(requestObj.ClientRequestObj.ClientRequestHeaders["Host"].ToString() == "zhdk.ch");
      Assert.IsTrue(requestObj.ClientRequestObj.Scheme == "http");
      Assert.IsTrue(requestObj.ClientRequestObj.Host == "zhdk.ch");
    }


    [Test]
    public void Requested_host_in_mapping_list_https()
    {
      // HostMapping plugin setup
      HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord record = new HttpReverseProxy.Plugin.HostMapping.DataTypes.HostMappingConfigRecord();
      record.RequestedHost = "ruben.zhdk.ch";
      record.MappingHost = "zhdk.ch";
      HttpReverseProxy.Plugin.HostMapping.Config.Mappings.Add(record.RequestedHost, new Tuple<string, string>("https", record.MappingHost));

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Scheme = "http";
      requestObj.ClientRequestObj.Host = "ruben.zhdk.ch";
      requestObj.ClientRequestObj.Path = "/notindex.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.DoNothing);
      Assert.IsTrue(requestObj.ClientRequestObj.ClientRequestHeaders["Host"].ToString() == "zhdk.ch");
      Assert.IsTrue(requestObj.ClientRequestObj.Scheme == "https");
      Assert.IsTrue(requestObj.ClientRequestObj.Host == "zhdk.ch");
    }

    #endregion

  }
}
