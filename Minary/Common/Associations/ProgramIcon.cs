namespace Minary.Common.Associations
{
  using System.IO;


  /// <summary>
  /// Reference to an .ico file used by AF_FileAssociator.
  /// </summary>
  public class ProgramIcon
  {
    public ProgramIcon(string iconPath)
    {
      IconPath = iconPath.Trim();
    }

    public readonly string IconPath;

    public bool IsValid
    {
      get
      {
        FileInfo getInfo = new FileInfo(IconPath);

        if (getInfo.Exists && getInfo.Extension == ".ico")
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }
  }
}
