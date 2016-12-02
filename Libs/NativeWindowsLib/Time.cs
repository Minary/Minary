namespace NativeWindowsLib
{
  using System.Runtime.InteropServices;
  using TimeTypes = NativeWindowsLib.DataTypes.Time;


  public class Time
  {

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FileTimeToSystemTime(
        [In] ref long fileTime,
        out TimeTypes.SystemTime systemTime);

  }
}
