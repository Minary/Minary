﻿namespace Minary.Common.Associations
{
  using System.IO;

  /// <summary>
  /// Reference to a executable file used by AF_FileAssociator.
  /// </summary>
  public class ExecApplication
  {

    #region MEMBERS

    public readonly string Path;

    #endregion


    #region PROPERTIES

    /// <summary>
    /// Gets a value indicating whether this Executable Application is an .exe, and that it exists.
    /// </summary>
    public bool IsValid
    {
      get
      {
        var getInfo = new FileInfo(this.Path);
        return getInfo.Exists;
      }
    }

    #endregion


    #region PUBLIC

    public ExecApplication(string appPath)
    {
      this.Path = appPath.Trim();
    }

    #endregion

  }
}
