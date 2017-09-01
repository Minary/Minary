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
      string minaryFullPath = Application.ExecutablePath;
      string cwd = Directory.GetCurrentDirectory();
      string iconPath = Path.Combine(cwd, "images", "Minary.ico");

      Minary.Common.Associations.AF_FileAssociator assoc = new Minary.Common.Associations.AF_FileAssociator(string.Format(".{0}", Minary.Config.MinaryFileExtension));
      try
      {
        if (assoc.Exists)
        {
          assoc.Delete();
        }
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "Minary.MainForm_Start(EXCEPTION): {0}", ex.Message);
      }

      try
      {
        assoc.Create(
                     "Minary_250",
                     string.Format("Minary .{0} file association", Minary.Config.MinaryFileExtension),
                     new Minary.Common.Associations.ProgramIcon(iconPath),
                     new Minary.Common.Associations.ExecApplication(minaryFullPath),
                     new Minary.Common.Associations.OpenWithList(new string[] { "Minary_250" }));
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "Minary.MainForm_Start(EXCEPTION): {0}", ex.Message);
      }

      // Tell explorer the file association has been changed
      SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
    }

    #endregion

  }
}
