namespace Minary.Updates.Config
{
  using System.Collections.Generic;

  public class UpdateData
  {

    #region PROPERTIES

    public bool IsUpdateAvaliable { get; set; }

    public string AvailableVersionStr { get; set; }

    public List<string> Messages { get; set; }

    #endregion


    #region PUBLIC

    public UpdateData()
    {
      IsUpdateAvaliable = false;
      AvailableVersionStr = string.Empty;
      Messages = new List<string>();
    }

    #endregion

  }
}
