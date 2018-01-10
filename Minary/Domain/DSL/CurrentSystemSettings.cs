namespace Minary.Domain.DSL
{
  using System.Text.RegularExpressions;


  public class CurrentSystemSettings
  {

    #region PROPERTIES

    public string LocalIpAddress { get; private set; }

    public string DefaultGatewayAddress { get; private set; }

    public string DnsIpAddress { get; private set; }

    #endregion


    #region PUBLIC

    public CurrentSystemSettings(string localIpAddress, string defaultGatewayAddress, string dnsIpAddress)
    {
      this.LocalIpAddress = localIpAddress;
      this.DefaultGatewayAddress = defaultGatewayAddress;
      this.DnsIpAddress = dnsIpAddress;
    }


    public string SetCurrentConfiguration(string templateData)
    {
      if (string.IsNullOrEmpty(templateData))
      {
        throw new System.Exception("No template data was defined");
      }

      var configuredTemplateData = templateData;
      configuredTemplateData = Regex.Replace(configuredTemplateData, Config.CONSTANT_LOCAL_IP, this.LocalIpAddress);
      configuredTemplateData = Regex.Replace(configuredTemplateData, Config.CONSTANT_LOCAL_GW, this.DefaultGatewayAddress);

      return configuredTemplateData;
    }

    #endregion

  }
}
