namespace Minary.Form.Updates.Config
{
  using System.Collections.Generic;


  public class UpdateData
  {

    #region PROPERTIES

    public bool IsUpdateAvailable { get; set; } = false;

    public string AvailableVersionStr { get; set; } = string.Empty;

    public List<string> Messages { get; set; } = new List<string>();

    public string SourceDownloadUrl { get; set; } = string.Empty;

    public string WinBinaryDownloadUrl { get; set; } = string.Empty;

    #endregion


    #region PUBLIC

    public UpdateData()
    {
    }

    #endregion

  }
}
