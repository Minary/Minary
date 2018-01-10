namespace Minary.Common.Associations
{
  using System.IO;


  /// <summary>
  /// Reference to an .ico file used by AF_FileAssociator.
  /// </summary>
  public class ProgramIcon
  {

    #region MEMBERS

    public readonly string IconPath;

    #endregion


    #region PROPERTIES

    public bool IsValid
    {
      get
      {
        FileInfo getInfo = new FileInfo(this.IconPath);

        if (getInfo.Exists && 
            getInfo.Extension == ".ico")
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    #endregion


    #region PUBLIC

    public ProgramIcon(string iconPath)
    {
      this.IconPath = iconPath.Trim();
    }

    #endregion

  }
}
