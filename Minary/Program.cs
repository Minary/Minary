namespace Minary
{
  using Minary.Form;
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Minary.LogConsole.Main;


  public static class Program
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    [STAThread]
    public static void Main(string[] args)
    {
      OperatingSystem operatingSystem = Environment.OSVersion;
      Version operatingSystemVersion = operatingSystem.Version;

      Application.SetCompatibleTextRenderingDefault(false);

      // Initialization
      Directory.SetCurrentDirectory(System.Windows.Forms.Application.StartupPath);
      DirectoryChecks(System.Windows.Forms.Application.StartupPath);
      AttackServiceChecks(System.Windows.Forms.Application.StartupPath);

      Application.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
      Application.EnableVisualStyles();

      // Load GUI
      try
      {
        MinaryMain minaryGuiObj = new MinaryMain(args);
        minaryGuiObj.LoadAllFormElements();
        minaryGuiObj.StartAllHandlers();
        minaryGuiObj.StartBackgroundThreads();
        minaryGuiObj.PreRun();
        minaryGuiObj.SetMinaryState();
        Application.Run(minaryGuiObj);
      }
      catch (Exception ex)
      {
        string message = string.Format("Minary: The following error occured: {0}\r\n\r\nStacktrace: {1}", ex.Message, ex.StackTrace);
        MessageDialog.Inst.ShowError(string.Empty, message, null);
      }
    }


    private static void AttackServiceChecks(string baseDir)
    {
      string missingAttackServiceBinaries = string.Empty;
      string[] attackServiceBinaries = new string[]
      {
        Config.ApeBinaryPath,
        Config.ArpScanBinaryPath,
        Config.HttpReverseProxyBinaryPath
      };

      foreach (string tmpBinPath in attackServiceBinaries)
      {
        string attackServiceBinaryFullPath = string.Format(@"{0}\{1}", baseDir, tmpBinPath);
        if (!File.Exists(attackServiceBinaryFullPath))
        {
          missingAttackServiceBinaries += tmpBinPath + "\r\n";
        }
      }

      if (missingAttackServiceBinaries.Length > 0)
      {
        string message = string.Format("The following attack services are missing: {0}", missingAttackServiceBinaries);
        LogCons.Inst.Write(message);
        MessageDialog.Inst.ShowWarning(string.Empty, message, null);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="baseDir"></param>
    private static void DirectoryChecks(string baseDir)
    {
      string[] directoryList = new string[]
                            {
                              Config.AttackServicesDir,
                              Config.ApeServiceDir,
                              Config.HttpReverseProxyServiceDir,
                              Config.HttpReverseProxyCertrifcateDir,
                              Config.ArpScanServiceDir,
                              Config.PluginsDir,
                              Config.TemplatesDir,
                              Config.CustomTemplatesDir,
                              Config.DataDir,
                              Config.MinaryDllDir
                            };

      foreach (string tmpDirectoryPath in directoryList)
      {
        try
        {
          string tmpAttackServiceDirectory = string.Format(@"{0}\{1}", baseDir, tmpDirectoryPath);
          if (!Directory.Exists(tmpAttackServiceDirectory))
          {
            Directory.CreateDirectory(tmpAttackServiceDirectory);
          }
        }
        catch (Exception ex)
        {
          string message = string.Format("Error occurred while creating \"{0}\"\r\nMessage: {1}", tmpDirectoryPath, ex.Message);
          MessageDialog.Inst.ShowError(string.Empty, message, null);
        }
      }
    }
  }
}
