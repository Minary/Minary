namespace Minary.DataTypes.Enum
{
  using System;


  [Flags]
  public enum MinaryState
  {
    StateOk = 0x0,
    NotAdmin = 0x1,
    WinPcapMissing = 0x2,
    NetworkMissing = 0x3
  }
}
