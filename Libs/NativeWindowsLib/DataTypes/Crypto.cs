namespace NativeWindowsLib.DataTypes
{
  using System;
  using System.Runtime.InteropServices;


  public class Crypto
  {

    #region DATA TYPES

    [StructLayout(LayoutKind.Sequential)]
    public struct CryptoApiBlob
    {
      public int DataLength;
      public IntPtr Data;

      public CryptoApiBlob(int dataLength, IntPtr data)
      {
        this.DataLength = dataLength;
        this.Data = data;
      }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CryptKeyProviderInformation
    {
      [MarshalAs(UnmanagedType.LPWStr)]
      public string ContainerName;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string ProviderName;
      public int ProviderType;
      public int Flags;
      public int ProviderParameterCount;
      public IntPtr ProviderParameters; // PCRYPT_KEY_PROV_PARAM
      public int KeySpec;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_DATA_BLOB
    {
      [MarshalAs(UnmanagedType.U4)]
      public uint cbData;
      public IntPtr pbData;
    }

    #endregion

  }
}
