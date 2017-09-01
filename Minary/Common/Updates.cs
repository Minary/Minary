namespace Minary.Common
{
  using Minary.DataTypes.Enum;
  using Minary.Form.Updates.Config;
  using Minary.LogConsole.Main;
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
      UpdateData updateData = null;

      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up) == false)
      {
        LogCons.Inst.Write(LogLevel.Warning, "Can't check for new updates as no internet connection is available.");
        return;
      }

      try
      {
        updateData = FetchUpdateInformationFromServer();
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "CheckForMinarygUpdates(Exception): {0}", ex.Message);
      }

      if (updateData == null || updateData.IsUpdateAvaliable == false)
      {
        LogCons.Inst.Write(LogLevel.Info, "No new updates available.");
        return;
      }

      Minary.Form.Updates.FormNewVersion newVersion = new Minary.Form.Updates.FormNewVersion();
      newVersion.TopMost = true;
      newVersion.ShowDialog();
    }


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

      if (NetworkInterface.GetAllNetworkInterfaces().Any(x => x.OperationalStatus == OperationalStatus.Up) == false)
      {
        goto END;
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
        if (string.IsNullOrEmpty(currentVersionXML) ||
            string.IsNullOrEmpty(Minary.Config.MinaryVersion))
        {
          goto END;
        }

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
        if (Regex.Match(Minary.Config.MinaryVersion, @"^\d+\.\d+\.\d+$").Success  == false ||
           Regex.Match(updateMetaData.AvailableVersionStr, @"^\d+\.\d+\.\d+$").Success == false)
        {
          goto END;
        }

        int availableVersionInt = int.Parse(Regex.Replace(updateMetaData.AvailableVersionStr, @"[^\d]+", string.Empty));
        int toolVersionInt = int.Parse(Regex.Replace(Minary.Config.MinaryVersion, @"[^\d]+", string.Empty));

        if (availableVersionInt.CompareTo(toolVersionInt) > 0)
        {
          updateMetaData.IsUpdateAvaliable = true;
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "FetchLatestReleaseMetaData(): {0}", ex.Message);
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

END:

      return updateMetaData;
    }

    #endregion

  }
}
