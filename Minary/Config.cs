namespace Minary
{
  using Minary.Common;
  using System;
  using System.IO;
  using System.Security.Principal;


  public class Config
  {

    #region MEMBERS

    public static readonly string ApplicationName = "Minary";

    //// TODO : Candidates to export the app.config file
public static string MinaryVersion { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); } set { } }

    public static readonly string LatestVersionOnGithub = "https://api.github.com/repos/Minary/Minary/releases/latest";
    public static readonly string ToolHomepage = "https://minary.io/download/";

    public static readonly string PipeName = "Minary";
    public static readonly string MinaryFileExtension = "mry";
    public static readonly int PipeInstances = 16;

    public static readonly string PluginsDir = "plugins";
    public static readonly string TemplatesDir = "templates";
    public static readonly string CustomTemplatesDir = Path.Combine(TemplatesDir, "MyTemplates");

    public static readonly string MinaryDllDir = "dll";
    public static readonly string PatternDir = "patterns";

    // Attack services
    public static readonly string AttackServicesPluginsDir = "attackservices";

// Service: APE
public static readonly string ApeServiceDir = Path.Combine(AttackServicesPluginsDir, "ArpPoisoning");
public static readonly string ApeBinaryPath = Path.Combine(ApeServiceDir, "Ape.exe");
public static readonly string ApeProcessName = "Ape";
    
// Service: DnsPoisoning
public static readonly string DnsPoisoningServiceDir = Path.Combine(AttackServicesPluginsDir, "DnsPoisoning");
public static readonly string DnsPoisoningBinaryPath = Path.Combine(DnsPoisoningServiceDir, "DnsPoisoning.exe");
public static readonly string DnsPoisoningProcessName = "DnsPoisoning";

// Service: Sniffer
public static readonly string SnifferServiceDir = Path.Combine(AttackServicesPluginsDir, "Sniffer");
public static readonly string SnifferBinaryPath = Path.Combine(SnifferServiceDir, "Sniffer.exe");
public static readonly string SnifferProcessName = "Sniffer";

// Service: HttpReverseProxy
public static readonly string HttpReverseProxyServiceDir = Path.Combine(AttackServicesPluginsDir, "HttpReverseProxy");
public static readonly string HttpReverseProxyBinaryPath = Path.Combine(HttpReverseProxyServiceDir, "HttpReverseProxy.exe");
public static readonly string HttpReverseProxyName = "HttpReverseProxy";
public static readonly string HttpReverseProxyCertrifcateDir = Path.Combine(HttpReverseProxyServiceDir, "Certificates");

// Registry
public static readonly string RegistrySoftwareName = "Minary";
public static readonly string BasisKey = $@"Software\{Config.RegistrySoftwareName}";

    #endregion


    #region PROPERTIES

    public static string OS { get; set; }

    public static string Architecture { get; set; }

    public static string Language { get; set; }

    public static string Processor { get; set; }

    public static string NumProcessors { get; set; }

    public static string DotNetVersion { get; set; }

    public static string CommonLanguateRuntime { get; set; }

    public static string WinPcap { get; set; }

    #endregion


    #region PUBLIC

    /// <summary>
    ///
    /// </summary>
    public static void CollectSystemInformation()
    {
      Config.OS = Utils.TryExecute(string.Format("{0}.{1}", Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor).ToString);
      Config.Architecture = Utils.TryExecute(() => { return Environment.Is64BitOperatingSystem ? "x64" : "x86"; });
      Config.Language = Utils.TryExecute(System.Globalization.CultureInfo.CurrentCulture.ToString);
      Config.Processor = Utils.TryExecute(System.Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER").ToString);
      Config.NumProcessors = Utils.TryExecute(System.Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS").ToString);
      Config.DotNetVersion = Utils.TryExecute(System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion);
      Config.CommonLanguateRuntime = Utils.TryExecute(Environment.Version.ToString);
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static bool IsAdministrator()
    {
      bool hasUserElevatedPermissions;
      WindowsIdentity currentUserIdentity = WindowsIdentity.GetCurrent();
      WindowsPrincipal principal = new WindowsPrincipal(currentUserIdentity);
      hasUserElevatedPermissions = principal.IsInRole(WindowsBuiltInRole.Administrator);

      return hasUserElevatedPermissions;
    }

    #endregion

  }
}