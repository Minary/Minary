namespace Minary.Common
{
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;
  using System;
  using System.Configuration;
  using System.IO;
  using System.Linq;
  using System.Net.NetworkInformation;
  using System.Windows.Forms;


  public class Updates : Control
  {

    #region PUBLIC METHODS

    /// <summary>
    ///
    /// </summary>
    public static void SyncAttackPatterns()
    {
      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up) == false)
      {
        return;
      }

      string repositoryLocal = Path.Combine(Directory.GetCurrentDirectory(), Config.TemplatesDir);
      string repositoryRemote = ConfigurationManager.AppSettings.Get("AttackPatternGitRepositoryRemote");

      if (string.IsNullOrEmpty(repositoryRemote))
      {
        LogCons.Inst.Write(LogLevel.Warning, "Minary SyncAttackPatterns: Can't sync attack pattern files because no remote repository is defined in the configuration file");
        return;
      }

      try
      {
        PatternFileManager.GitHubPatternFileMgr.InitializeRepository(repositoryLocal, repositoryRemote);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "Minary SyncAttackPatterns: Initializing local attack pattern directory ({0}) failed: {1}", repositoryLocal, ex.Message);
      }

      try
      {
        PatternFileManager.GitHubPatternFileMgr.SyncRepository(repositoryLocal, Config.GitUser, Config.GitEmail);
        LogCons.Inst.Write(LogLevel.Info, "Minary SyncAttackPatterns: Attack pattern sync finished.");
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "Minary SyncAttackPatterns: Syncing attack pattern failed: {0}", ex.Message);
      }
    }

    #endregion

  }
}
