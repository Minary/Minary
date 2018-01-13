namespace Minary.Common.Associations
{
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;
  using System;
  using System.IO;
  using System.Runtime.InteropServices;
  using System.Windows.Forms;


  public class MryFiles
  {

    #region IMPORTS

    [DllImport("Shell32.dll", ExactSpelling = true)]
    public static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

    #endregion


    #region PUBLIC METHODS

    public static void InstallMryFileAssociation()
    {
      // Set .mry file extension association
      var minaryFullPath = Application.ExecutablePath;
      var cwd = Directory.GetCurrentDirectory();
      var iconPath = Path.Combine(cwd, "images", "Minary.ico");
      var assoc = new AF_FileAssociator($".{Config.MinaryFileExtension}");

      try
      {
        if (assoc.Exists)
        {
          assoc.Delete();
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"Minary.MainForm_Start(EXCEPTION.Delete): {ex.Message}");
      }

      try
      {
        assoc.Create(
                     "Minary_250",
                     $"Minary .{Config.MinaryFileExtension} file association",
                     new ProgramIcon(iconPath),
                     new ExecApplication(minaryFullPath),
                     new OpenWithList(new string[] { "Minary_250" }));
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"Minary.MainForm_Start(EXCEPTION.Create): {ex.Message}");
      }

      // Tell explorer the file association has been changed
      SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
    }

    #endregion

  }
}
