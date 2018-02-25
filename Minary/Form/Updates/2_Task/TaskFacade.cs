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
  using System.Collections.Generic;
  using System.Text.RegularExpressions;
  using System.Threading;


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
        UpdateData updateMetaData = new UpdateData();

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
      UpdateData updateData = new UpdateData();

      // Ignore all https certificat errors
      // Reason: Reasons
      System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

      // Fetch latest release data from Github
      var client = new RestClient(Config.LatestVersionOnGithub);
      var response = client.Execute<Release>(new RestRequest());

      if (response.ErrorException.Message.ToLower().Contains("ssl/tls"))
      {
        throw new Exception("Cannot crate a secure SSL/TLS connection to GitHub");
      }

      var release = response.Data;

      // Verify if version structure is correct
      if (string.IsNullOrEmpty(release?.TagName) ||
          Regex.Match(release.TagName, @"v?\d+\.\d+\.\d+", RegexOptions.IgnoreCase).Success == false)
      {
        throw new Exception($"Version number has an invalid structure: {release.TagName}");
      }

      // Remove leading "v" from the release tag
      if (Regex.Match(release.TagName, @"v\d+\.\d+\.\d+", RegexOptions.IgnoreCase).Success == true)
      {
        release.TagName = release.TagName.TrimStart(new char[] { 'v', 'V' });
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
      int[] versionArray = new int[3];
      MatchCollection versionMatches = Regex.Matches(versionString, @"^(\d+)\.(\d+)\.(\d+)");

      if (versionMatches.Count != 1)
      {
        throw new Exception("Pattern not found");
      }

      foreach (int i in Enumerable.Range(0, 3))
      {
        versionArray[i] = Convert.ToInt32(versionMatches[0].Groups[i + 1].Value);
      }

      long versionInt = (versionArray[0] + 1) * 1000000 + (versionArray[1] + 1) * 1000 + (versionArray[2] + 1) * 1;

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
