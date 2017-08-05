namespace Minary.Domain.DSL
{
  using System.Text.RegularExpressions;


  public class CurrentSystemSettings
  {

    #region MEMBERS

    private string localIpAddress;
    private string defaultGatewayAddress;
    private string dnsIpAddress;

    #endregion


    #region PROPERTIES

    public string LocalIpAddress { get { return this.localIpAddress; } set { } }

    public string DefaultGatewayAddress { get { return this.defaultGatewayAddress; } set { } }

    public string DnsIpAddress { get { return this.dnsIpAddress; } set { } }

    #endregion


    #region PUBLIC

    public CurrentSystemSettings(string localIpAddress, string defaultGatewayAddress, string dnsIpAddress)
    {
      this.localIpAddress = localIpAddress;
      this.defaultGatewayAddress = defaultGatewayAddress;
      this.dnsIpAddress = dnsIpAddress;
    }


    public string SetCurrentConfiguration(string templateData)
    {
      if (string.IsNullOrEmpty(templateData))
      {
        throw new System.Exception("No template data was defined");
      }

      string configuredTemplateData = templateData;

      configuredTemplateData = Regex.Replace(configuredTemplateData, Config.CONSTANT_LOCAL_IP, this.localIpAddress);
      configuredTemplateData = Regex.Replace(configuredTemplateData, Config.CONSTANT_LOCAL_GW, this.defaultGatewayAddress);

      return configuredTemplateData;
    }

    #endregion

  }
}
