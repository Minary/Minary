namespace NativeWindowsLib.DataTypes
{
  using System.Runtime.InteropServices;


  public class Time
  {

    #region DATA TYPES

    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
      public short Year;
      public short Month;
      public short DayOfWeek;
      public short Day;
      public short Hour;
      public short Minute;
      public short Second;
      public short Milliseconds;
    }

    #endregion

  }
}
