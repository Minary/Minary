namespace Minary.Form.Updates.Task
{
  using Minary;
  using Minary.DataTypes.DTO;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface.Updates;
  using Minary.Form.Updates.Config;
  using Minary.LogConsole.Main;
  using RestSharp;
  using System;
  using System.Linq;
  using System.Net.Security;
  using System.Collections.Generic;
  using System.Text.RegularExpressions;


  public class TaskFacade : IObservable
  {

    #region MEMBERS

    private List<IObserver> observers = new List<IObserver>();

    #endregion


    #region PUBLIC

    public void StartSearchingForUpdates()
    {
      System.Threading.Tasks.Task.Run(() =>
      {
        var updateMetaData = new UpdateData();

        try
        {
          updateMetaData = this.FetchUpdateInformationFromServer();
        }
        catch (Exception ex)
        {
          var errorMsg = $"The following error occurred: {ex.Message}";
          LogCons.Inst.Write(LogLevel.Warning, errorMsg);

          updateMetaData.IsErrorOccurred = true;
          updateMetaData.ErrorMessage = ex.Message;
        }

        this.Notify(updateMetaData);
      });    
    }

    #endregion


    #region PRIVATE

    private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
    {
      return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public UpdateData FetchUpdateInformationFromServer()
    {
      var updateData = new UpdateData();

      // Ignore all https certificat errors
      // Reason: Reasons

      System.Net.ServicePointManager.ServerCertificateValidationCallback +=
        (sender, certificate, chain, sslPolicyErrors) => true;

      // Fetch latest release data from Github
      var client = new RestClient(Config.LatestVersionOnGithub);
      var response = client.Execute<Release>(new RestRequest());

      // Process errors
      if (response.ErrorException != null &&
          string.IsNullOrEmpty(response.ErrorException.Message) == false)
      {
        if (response.ErrorException.Message.ToLower().Contains("ssl/tls"))
        {
          throw new Exception("Cannot crate a secure SSL/TLS connection to GitHub");
        }
        else if (((System.Net.WebException)((RestSharp.RestResponseBase)response).ErrorException).Status == System.Net.WebExceptionStatus.NameResolutionFailure)
        {
          throw new Exception("Could not resolve Github server name");
        }
        else
        {
          throw new Exception($"Unknown error occurred: {response.ErrorException.Message}");
        }
      }

      var release = response.Data;

      // Verify if version structure is correct
      if (string.IsNullOrEmpty(release?.TagName) ||
          Regex.Match(release.TagName, @"v?\d+", RegexOptions.IgnoreCase).Success == false)
      {
        throw new Exception($"Version number has an invalid structure: {release.TagName}");
      }

      // Compare current and latest version.
      long currentVersionInt = DetermineVersion(Config.MinaryVersion);
      long availableVersionInt = DetermineVersion(release.TagName);
      if (availableVersionInt.CompareTo(currentVersionInt) > 0)
      {
        updateData.IsUpdateAvailable = true;
        updateData.AvailableVersionStr = release.TagName;
        updateData.Messages = Regex.Split(release.Body, @"(\r\n|\n)").ToList();
        updateData.SourceDownloadUrl = release.SourceZipBallUrl;
        updateData.WinBinaryDownloadUrl = release.Assets[0].BrowserDownloadUrl;
      }

      return updateData;
    }


    private static long DetermineVersion(string versionString)
    {
      int[] versionArray = new int[4];

      if (string.IsNullOrEmpty(versionString) == true)
      {
        throw new Exception($"Invalid version string: {versionString}");
      }

      versionString = versionString.Trim();
      if (Regex.Match(versionString, @"^v?\d+$").Success)
      {
        versionString += ".0";
      }

      versionString = versionString.Trim();
      if (Regex.Match(versionString, @"^v?\d+\.\d+$").Success)
      {
        versionString += ".0";
      }

      if (Regex.Match(versionString, @"^v?\d+\.\d+\.\d+$").Success)
      {
        versionString += ".0";
      }

      MatchCollection versionMatches = Regex.Matches(versionString, @"^v?(\d+)\.(\d+)\.(\d+)\.(\d+)");
      if (versionMatches.Count != 1)
      {
        throw new Exception("Pattern not found");
      }

      foreach (int i in Enumerable.Range(0, 4))
      {
        versionArray[i] = Convert.ToInt32(versionMatches[0].Groups[i + 1].Value);
      }

      long versionInt = (versionArray[0]) * 1000000000 + (versionArray[1]) * 1000000 + (versionArray[2]) * 1000 + (versionArray[3]) * 1;

      return versionInt;
    }
    #endregion


    #region INTERFACE: IObservable

    public void AddObserver(IObserver observer)
    {
      this.observers.Add(observer);
    }


    public void Notify(UpdateData updateData)
    {
      foreach (IObserver observer in this.observers)
      {
        observer.Update(updateData);
      }
    }

    #endregion

  }
}
