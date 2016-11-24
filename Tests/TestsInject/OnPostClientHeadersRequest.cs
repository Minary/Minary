namespace TestsInject
{
  using HttpReverseProxy.Plugin.Inject;
  using HttpReverseProxyLib.DataTypes;
  using HttpReverseProxyLib.Exceptions;
  using NUnit.Framework;
  using System;
  using System.Collections.Generic;

  [TestFixture]
  public class OnPostClientHeadersRequest : Inject
  {

    #region MEMBERS



    #endregion


    #region NUNIT

    [SetUp]
    public void TestSetup()
    {
      HttpReverseProxy.Plugin.Inject.Config.InjectRecords = new List<HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord>();
      HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Clear();
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
    public void Injection_record_list_is_null()
    {
      // Inject plugin setup
      HttpReverseProxy.Plugin.Inject.Config.InjectRecords = null;


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
      // Inject plugin setup
      HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord record = new HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord();
      record.Type = HttpReverseProxy.Plugin.Inject.DataTypes.InjectType.URL;
      record.Host = "ruben.zhdk.ch";
      record.Path = "/index.html";
      record.ReplacementResource = "http://www.zhdk.ch/";

      HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Add(record);


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
    public void Host_and_path_do_not_match()
    {
      // Inject plugin setup
      HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord record = new HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord();
      record.Type = HttpReverseProxy.Plugin.Inject.DataTypes.InjectType.URL;
      record.Host = "ruben.zhdk.ch";
      record.Path = "/index.html";
      record.ReplacementResource = "http://www.zhdk.ch/";

      HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Add(record);

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "notruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Path = "/notindex.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.DoNothing); 
    }

    [Test]
    public void Host_matches_path_does_not()
    {
      // Inject plugin setup
      HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord record = new HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord();
      record.Type = HttpReverseProxy.Plugin.Inject.DataTypes.InjectType.URL;
      record.Host = "ruben.zhdk.ch";
      record.Path = "/index.html";
      record.ReplacementResource = "http://www.zhdk.ch/";

      HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Add(record);

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Path = "/notindex.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.DoNothing);
    }


    [Test]
    public void Path_matches_host_does_not()
    {
      // Inject plugin setup
      HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord record = new HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord();
      record.Type = HttpReverseProxy.Plugin.Inject.DataTypes.InjectType.URL;
      record.Host = @"ruben.zhdk.ch";
      record.Path = @"/index.html";
      record.ReplacementResource = "http://www.zhdk.ch/";

      HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Add(record);

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "notruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Path = "/index.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.DoNothing);
    }

    #endregion


    #region TESTS : Successful, Types

    [Test]
    public void Redirect_resource_url()
    {
      // Inject plugin setup
      HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord record = new HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord();
      record.Type = HttpReverseProxy.Plugin.Inject.DataTypes.InjectType.URL;
      record.Host = "ruben.zhdk.ch";
      record.Path = "/index.html";
      record.ReplacementResource = "http://www.zhdk.ch/";

      HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Add(record);

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Path = "/index.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.RedirectToNewUrl);
      Assert.IsTrue(instruction.InstructionParameters.Data == record.ReplacementResource);
    }


    [Test]
    public void Redirect_resource_file()
    {
      // Inject plugin setup
      HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord record = new HttpReverseProxy.Plugin.Inject.DataTypes.InjectConfigRecord();
      record.Type = HttpReverseProxy.Plugin.Inject.DataTypes.InjectType.File;
      record.Host = "ruben.zhdk.ch";
      record.Path = "/index.html";
      record.ReplacementResource = @"c:\temp\replacementResource.txt";
      Console.WriteLine("recordCount:{0}", HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Count);
      HttpReverseProxy.Plugin.Inject.Config.InjectRecords.Add(record);

      // Request object setup
      RequestObj requestObj = new RequestObj("ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Clear();
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Host", "ruben.zhdk.ch");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("User-Agent", "A-Random-User-Agent");
      requestObj.ClientRequestObj.ClientRequestHeaders.Add("Referer", "http://random.referer.ch/");
      requestObj.ClientRequestObj.Path = "/index.html";
      PluginInstruction instruction = this.OnPostClientHeadersRequest(requestObj);

      Assert.IsTrue(instruction.Instruction == Instruction.SendBackLocalFile);
      Assert.IsTrue(instruction.InstructionParameters.Data == record.ReplacementResource);
    }

    #endregion

  }
}
