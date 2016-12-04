namespace NativeWindowsLib.Crypto
{
  using System;
  using System.Runtime.InteropServices;
  using CryptoTypes = DataTypes.Crypto;
  using TimeTypes = DataTypes.Time;


  public class Native
  {
    [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptAcquireContextW(
     out IntPtr providerContext,
     [MarshalAs(UnmanagedType.LPWStr)] string container,
     [MarshalAs(UnmanagedType.LPWStr)] string provider,
     int providerType,
     int flags);

    [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptReleaseContext(
        IntPtr providerContext,
        int flags);

    [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptGenKey(
        IntPtr providerContext,
        int algorithmId,
        int flags,
        out IntPtr cryptKeyHandle);

    [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptDestroyKey(
        IntPtr cryptKeyHandle);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CertStrToNameW(
        int certificateEncodingType,
        IntPtr x500,
        int strType,
        IntPtr reserved,
        [MarshalAs(UnmanagedType.LPArray)] [Out] byte[] encoded,
        ref int encodedLength,
        out IntPtr errorstring);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr CertCreateSelfSignCertificate(
        IntPtr providerHandle,
        [In] ref NativeWindowsLib.DataTypes.Crypto.CryptoApiBlob subjectIssuerBlob,
        int flags,
        [In] ref CryptoTypes.CryptKeyProviderInformation keyProviderInformation,
        IntPtr signatureAlgorithm,
        [In] ref TimeTypes.SystemTime startTime,
        [In] ref TimeTypes.SystemTime endTime,
        IntPtr extensions);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CertFreeCertificateContext(
        IntPtr certificateContext);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr CertOpenStore(
        [MarshalAs(UnmanagedType.LPStr)] string storeProvider,
        int messageAndCertificateEncodingType,
        IntPtr cryptProvHandle,
        int flags,
        IntPtr parameters);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CertCloseStore(
        IntPtr certificateStoreHandle,
        int flags);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CertAddCertificateContextToStore(
        IntPtr certificateStoreHandle,
        IntPtr certificateContext,
        int addDisposition,
        out IntPtr storeContextPtr);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CertSetCertificateContextProperty(
        IntPtr certificateContext,
        int propertyId,
        int flags,
        [In] ref CryptoTypes.CryptKeyProviderInformation data);

    [DllImport("Crypt32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PFXExportCertStoreEx(
        IntPtr certificateStoreHandle,
        ref CryptoTypes.CryptoApiBlob pfxBlob,
        IntPtr password,
        IntPtr reserved,
        int flags);



    [DllImport("crypt32.dll", SetLastError = true)]
    private static extern IntPtr PFXImportCertStore(
        ref CryptoTypes.CRYPT_DATA_BLOB pPfx,
        [MarshalAs(UnmanagedType.LPWStr)] string szPassword,
        uint dwFlags = 0);

    [DllImport("crypt32.dll")]
    private static extern bool PFXIsPFXBlob(
                 ref CryptoTypes.CRYPT_DATA_BLOB pPfx);

    [DllImport("crypt32.dll")]
    private static extern bool PFXVerifyPassword(
        ref CryptoTypes.CRYPT_DATA_BLOB pPfx,
        [MarshalAs(UnmanagedType.LPWStr)] string szPassword,
         uint dwFlags = 0);
  }
}
