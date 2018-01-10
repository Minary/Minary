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
      var toReturn = new List<string>();

      foreach (string file in openWithPaths)
      {
        var getInfo = new FileInfo(file);
        toReturn.Add(getInfo.Name);
      }

      List = toReturn.ToArray();
    }

    public readonly string[] List;
  }

}
