namespace Minary.Common.Associations
{
  using System.Collections.Generic;
  using System.IO;

  /// <summary>
  /// Reference to an list of executable files used by AF_FileAssociator.
  /// </summary>
  public class OpenWithList
  {
    public OpenWithList(string[] openWithPaths)
    {
      List<string> toReturn = new List<string>();
      FileInfo getInfo;

      foreach (string file in openWithPaths)
      {
        getInfo = new FileInfo(file);
        toReturn.Add(getInfo.Name);
      }

      List = toReturn.ToArray();
    }

    public readonly string[] List;
  }

}
