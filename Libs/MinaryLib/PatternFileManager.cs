namespace Minary.PatternFileManager
{
  using LibGit2Sharp;
  using System;
  using System.Collections.Generic;
  using System.Configuration;
  using System.IO;
  using System.Linq;


  public class GitHubPatternFileMgr
  {

    #region PUBLIC METHODS

    public static void LoadParametersFromConfig(string configFileFullPath, Dictionary<string, string> configElements)
    {
      // Raise an exception if config file does not exist
      if (string.IsNullOrEmpty(configFileFullPath) ||
          !File.Exists(configFileFullPath))
      {
        throw new Exception("The application config file does not exist");
      }

      // Load config file
      ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
      configMap.ExeConfigFilename = configFileFullPath;
      System.Configuration.Configuration pluginConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
      AppSettingsSection section = (pluginConfiguration.GetSection("appSettings") as AppSettingsSection);

      // Load parameters
      string[] configElementKeys = configElements.Keys.ToArray();
      foreach (string tmpKey in configElementKeys)
      {
        if (section.Settings.AllKeys.Contains(tmpKey))
        {
          configElements[tmpKey] = section.Settings[tmpKey].Value.ToString();
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="repositoryLocal"></param>
    /// <param name="repositoryRemote"></param>
    public static void InitializeRepository(string repositoryLocal, string repositoryRemote)
    {
      for (int i = 0; i < 3; i++)
      {
        if (Repository.IsValid(repositoryLocal))
        {
          break;
        }

        try
        {
          Repository.Clone(repositoryRemote, repositoryLocal);
        }
        catch (LibGit2Sharp.NameConflictException)
        {
          DirectoryCleanup(repositoryLocal);
        }
      }

      if (!Repository.IsValid(repositoryLocal))
      {
        throw new Exception("Could not initialize repository");
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="directory"></param>
    private static void DirectoryCleanup(string directory)
    {
      if (string.IsNullOrEmpty(directory))
      {
        return;
      }

      try
      {
        FileAttributes fileAttributes = File.GetAttributes(directory);
        if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
        {
          Directory.Delete(directory, true);
        }
      }
      catch (Exception)
      {
      }
    }


    /// <summary>
    /// Fetch pattern file updates from remote GIT repository.
    /// </summary>
    /// <param name="repositoryLocal"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    public static void SyncRepository(string repositoryLocal, string username, string email)
    {
      using (var repo = new Repository(repositoryLocal))
      {
        var merger = new Signature(username, email, DateTimeOffset.UtcNow);
        var options = new PullOptions { FetchOptions = new FetchOptions() };

        MergeResult pullResult = repo.Network.Pull(merger, options);
// TODO: Make it more seksi. It's not done yet!
//// pullResult.Status == MergeStatus.
      }
    }

    #endregion

  }
}
