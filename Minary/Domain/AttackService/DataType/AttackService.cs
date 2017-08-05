namespace Minary.Domain.AttackService.DataTypes
{
  public class AttackService
  {
    public static class ArpPoisoning
    {
      public static readonly string Name = "ArpPoisoning";

      public static class SubModule
      {
        public static string DnsPoisoning { get { return "ArpPoisoning.DnsPoisoning"; } set { } }

        public static string Firewall { get { return "ArpPoisoning.Firewall"; } set { } }
      }
    }

    public static class DataSniffer
    {
      public static readonly string Name = "DataSniffer";

      public static class SubModule
      {
      }
    }

    public static class HttpReverseProxyServer
    {
      public static readonly string Name = "HttpReverseProxyServer";

      public static class SubModule
      {
        public static string SslStrip { get { return "HttpReverseProxyServer.SslStrip"; } set { } }

        public static string DataSniffer { get { return "HttpReverseProxyServer.DataSniffer"; } set { } }

        public static string InjectCode { get { return "HttpReverseProxyServer.InjectCode"; } set { } }

        public static string InjectFile { get { return "HttpReverseProxyServer.InjectFile"; } set { } }

        public static string RequestRedirect { get { return "HttpReverseProxyServer.RequestRedirect"; } set { } }

        public static string HostMapping { get { return "HttpReverseProxyServer.HostMapping"; } set { } }
      }
    }
  }
}