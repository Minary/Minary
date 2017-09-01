namespace Minary
{
  using Minary.DataTypes.Enum;
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
      Directory.SetCurrentDirectory(Application.StartupPath);
      DirectoryChecks(Application.StartupPath);

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
