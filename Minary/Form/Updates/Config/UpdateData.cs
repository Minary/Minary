namespace Minary.Form.Updates.Config
{
  using System.Collections.Generic;


  public class UpdateData
  {

    #region PROPERTIES

    public bool IsUpdateAvailable { get; set; }

    public string AvailableVersionStr { get; set; }

    public List<string> Messages { get; set; }

    public string SourceDownloadUrl { get; set; }

    public string WinBinaryDownloadUrl { get; set; }

    #endregion


    #region PUBLIC

    public UpdateData()
    {
      this.IsUpdateAvailable = false;
      this.AvailableVersionStr = string.Empty;
      this.Messages = new List<string>();
      this.SourceDownloadUrl = string.Empty;
      this.WinBinaryDownloadUrl = string.Empty;
    }

    #endregion

  }
}
