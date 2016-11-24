namespace Minary.Common
{
  using Minary.Updates.Config;
  using System;
  using System.Configuration;
  using System.IO;
  using System.Linq;
  using System.Net;
  using System.Net.NetworkInformation;
  using System.Text.RegularExpressions;
  using System.Windows.Forms;
  using System.Xml;

  public class Updates : Control
  {

    #region PUBLIC METHODS


    /// <summary>
    ///
    /// </summary>
    public static void CheckForMinaryUpdates()
    {
      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        UpdateData updateData = null;

        try
        {
          updateData = FetchUpdateInformationFromServer();
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("CheckForMinarygUpdates(Exception) : {0}", ex.Message);
        }

        if (updateData != null && updateData.IsUpdateAvaliable)
        {
          Minary.Updates.FormNewVersion newVersion = new Minary.Updates.FormNewVersion();
          newVersion.TopMost = true;
          newVersion.ShowDialog();
        }
        else
          LogConsole.Main.LogConsole.LogInstance.LogMessage("No new updates available.");
      }
      else
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("Can't check for new updates as no internet connection is available.");
      }
    }


    /// <summary>
    ///
    /// </summary>
    public static void SyncAttackPatterns()
    {
      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        string repositoryLocal = Path.Combine(Directory.GetCurrentDirectory(), Config.TemplatesDir);
        string repositoryRemote = ConfigurationManager.AppSettings.Get("AttackPatternGitRepositoryRemote");

        if (string.IsNullOrEmpty(repositoryRemote))
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary SyncAttackPatterns: Can't sync attack pattern files because no remote repository is defined in the configuration file");
          return;
        }

        try
        {
          Minary.PatternFileManager.GitHubPatternFileMgr.InitializeRepository(repositoryLocal, repositoryRemote);
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary SyncAttackPatterns: Initializing local attack pattern directory ({0}) failed: {1}", repositoryLocal, ex.Message);
        }

        try
        {
          Minary.PatternFileManager.GitHubPatternFileMgr.SyncRepository(repositoryLocal, Config.GitUser, Config.GitEmail);
          LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary SyncAttackPatterns: Attack pattern sync finished.");
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("Minary SyncAttackPatterns: Syncing attack pattern failed: {0}", ex.Message);
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    public static UpdateData FetchUpdateInformationFromServer()
    {
      HttpWebRequest webRequest = null;
      WebResponse webResponse = null;
      Stream dataStream = null;
      StreamReader reader = null;
      string currentVersionXML = string.Empty;
      UpdateData updateMetaData = new UpdateData();

      if (!NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up))
      {
        return updateMetaData;
      }

      try
      {
        // Fetch meta data from server
        webRequest = (HttpWebRequest)WebRequest.Create(Minary.Config.CurrentVersionURL);
        webResponse = webRequest.GetResponse();

        dataStream = webResponse.GetResponseStream();
        reader = new StreamReader(dataStream);
        currentVersionXML = reader.ReadToEnd();

        // Parse meta data
        if (!string.IsNullOrEmpty(currentVersionXML) && !string.IsNullOrEmpty(Minary.Config.MinaryVersion))
        {
          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc.LoadXml(currentVersionXML);

          // Parse latest version data
          var data = xmlDoc.SelectNodes("/minary");
          updateMetaData.AvailableVersionStr = data.Item(0)["version"].InnerText;

          // Parse messages from XML
          updateMetaData.Messages.Clear();
          XmlNodeList messages = xmlDoc.SelectNodes("/minary/message");
          foreach (XmlNode xn in messages)
          {
            updateMetaData.Messages.Add(xn.InnerText);
          }

          // Compare current and latest version.
          if (Regex.Match(Minary.Config.MinaryVersion, @"^\d+\.\d+\.\d+$").Success && Regex.Match(updateMetaData.AvailableVersionStr, @"^\d+\.\d+\.\d+$").Success)
          {
            int availableVersionInt = int.Parse(Regex.Replace(updateMetaData.AvailableVersionStr, @"[^\d]+", string.Empty));
            int toolVersionInt = int.Parse(Regex.Replace(Minary.Config.MinaryVersion, @"[^\d]+", string.Empty));

            if (availableVersionInt.CompareTo(toolVersionInt) > 0)
            {
              updateMetaData.IsUpdateAvaliable = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("FetchLatestReleaseMetaData(): {0}", ex.Message);
      }
      finally
      {
        if (reader != null)
        {
          reader.Close();
        }

        if (webResponse != null)
        {
          webResponse.Close();
        }
      }

      return updateMetaData;
    }


    #endregion
  }
}
