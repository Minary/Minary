namespace Minary.DataTypes.Enum
{
  using System;


  [Flags]
  public enum MinaryState
  {
    StateOk = 0x0,
    NotAdmin = 0x1,
    NPcapMissing = 0x2,
    NetworkMissing = 0x4,
    ApeBinaryMissing = 0x8,
    DnsPoisoningBinaryMissing = 0x10,
    SnifferMissing = 0x20,
    HttpProxyMissing = 0x40,
    RouterIPv4BinaryMissing = 0x80,
  }
}
