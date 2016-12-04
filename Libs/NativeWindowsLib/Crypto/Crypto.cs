namespace NativeWindowsLib.Crypto
{
  //using Minary.Certificates.DataTypes;
  //  using NativeWindowsLib;
  using System;
  using System.IO;
  using System.Runtime.InteropServices;
  using System.Security.Cryptography.X509Certificates;
  using System.Text.RegularExpressions;
  using CryptoTypes = NativeWindowsLib.DataTypes.Crypto;
  using TimeTypes = NativeWindowsLib.DataTypes.Time;
  using RTHelpers = System.Runtime.CompilerServices.RuntimeHelpers;
  using Securestring = System.Security.SecureString;


  public static class Crypto
  {

    #region PUBLIC

    public static X509Certificate2Collection GetCertificatesFromStoreFile(string certificateFilePath, string password)
    {

      // Create a collection object and populate it using the PFX file
      X509Certificate2Collection certificateCollection = new X509Certificate2Collection();
      certificateCollection.Import(certificateFilePath, password, X509KeyStorageFlags.PersistKeySet);

      if (certificateCollection.Count <= 0)
      {
        throw new Exception("No valid certificate found in store");
      }

      return certificateCollection;
    }


    public static void CreateNewCertificate(string certificateOutputPath, string hostName, DateTime validityStartDate, DateTime validityEndDate)
    {
      if (string.IsNullOrEmpty(certificateOutputPath) || string.IsNullOrWhiteSpace(certificateOutputPath))
      {
        throw new Exception("The certificate output path is invalid");
      }

      if (string.IsNullOrEmpty(hostName) || string.IsNullOrWhiteSpace(hostName))
      {
        throw new Exception("The hostName is invalid");
      }

      if (File.Exists(certificateOutputPath))
      {
        throw new Exception("A certificate for this server already exists");
      }

      if (validityStartDate == null)
      {
        throw new Exception("The validity start date is invalid");
      }

      if (validityEndDate == null)
      {
        throw new Exception("The validity end date is invalid");
      }

      if (validityEndDate < DateTime.Now)
      {
        throw new Exception("A validity end date that is in the past is invalid");
      }

      if (validityEndDate < validityStartDate)
      {
        throw new Exception("The validity end date must be greater than the validity start date");
      }

      byte[] c = CreateSelfSignCertificatePfx(
                              string.Format("CN={0}", hostName), //"CN=localhost", //host name
                              validityStartDate, //not valid before
                              validityEndDate, //not valid after
                              string.Empty); //password to encrypt key file
      using (BinaryWriter binWriter = new BinaryWriter(File.Open(certificateOutputPath, FileMode.Create)))
      {
        binWriter.Write(c);
      }

      if (!File.Exists(certificateOutputPath))
      {
        throw new Exception("The certificate file could not be created");
      }

      FileInfo certificateFileInfo = new FileInfo(certificateOutputPath);
      if (certificateFileInfo.Length < 10)
      {
        File.Delete(certificateOutputPath);
        throw new Exception("The content of the created certificate is invalid");
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="x500"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="insecurePassword"></param>
    /// <returns></returns>
    private static byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime, string insecurePassword)
    {
      byte[] pfxData;
      Securestring password = null;

      try
      {
        if (!string.IsNullOrEmpty(insecurePassword))
        {
          password = new Securestring();
          foreach (char ch in insecurePassword)
          {
            password.AppendChar(ch);
          }

          password.MakeReadOnly();
        }

        pfxData = CreateSelfSignCertificatePfx(x500, startTime, endTime, password);
      }
      finally
      {
        if (password != null)
        {
          password.Dispose();
        }
      }

      return pfxData;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="x500"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private static byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime, Securestring password)
    {
      byte[] pfxData;

      if (x500 == null)
      {
        x500 = string.Empty;
      }

      TimeTypes.SystemTime startSystemTime = ToSystemTime(startTime);
      TimeTypes.SystemTime endSystemTime = ToSystemTime(endTime);
      string containerName = Guid.NewGuid().ToString();

      GCHandle dataHandle = new GCHandle();
      IntPtr providerContext = IntPtr.Zero;
      IntPtr cryptKey = IntPtr.Zero;
      IntPtr certContext = IntPtr.Zero;
      IntPtr certStore = IntPtr.Zero;
      IntPtr storeCertContext = IntPtr.Zero;
      IntPtr passwordPtr = IntPtr.Zero;
      RTHelpers.PrepareConstrainedRegions();

      try
      {
        Check(Native.CryptAcquireContextW(
            out providerContext,
            containerName,
            null,
            1, // PROV_RSA_FULL
            8)); // CRYPT_NEWKEYSET

        Check(Native.CryptGenKey(
            providerContext,
            1, // AT_KEYEXCHANGE
            1, // CRYPT_EXPORTABLE
            out cryptKey));

        IntPtr errorstringPtr;
        int nameDataLength = 0;
        byte[] nameData;

        // errorstringPtr gets a pointer into the middle of the x500 string,
        // so x500 needs to be pinned until after we've copied the value
        // of errorstringPtr.
        dataHandle = GCHandle.Alloc(x500, GCHandleType.Pinned);

        if (!Native.CertStrToNameW(
            0x00010001, // X509_ASN_ENCODING | PKCS_7_ASN_ENCODING
            dataHandle.AddrOfPinnedObject(),
            3, // CERT_X500_NAME_STR = 3
            IntPtr.Zero,
            null,
            ref nameDataLength,
            out errorstringPtr))
        {
          string error = Marshal.PtrToStringUni(errorstringPtr);
          throw new ArgumentException(error);
        }

        nameData = new byte[nameDataLength];

        if (!Native.CertStrToNameW(
            0x00010001, // X509_ASN_ENCODING | PKCS_7_ASN_ENCODING
            dataHandle.AddrOfPinnedObject(),
            3, // CERT_X500_NAME_STR = 3
            IntPtr.Zero,
            nameData,
            ref nameDataLength,
            out errorstringPtr))
        {
          string error = Marshal.PtrToStringUni(errorstringPtr);
          throw new ArgumentException(error);
        }

        dataHandle.Free();

        dataHandle = GCHandle.Alloc(nameData, GCHandleType.Pinned);
        CryptoTypes.CryptoApiBlob nameBlob = new CryptoTypes.CryptoApiBlob(nameData.Length, dataHandle.AddrOfPinnedObject());

        CryptoTypes.CryptKeyProviderInformation kpi = new CryptoTypes.CryptKeyProviderInformation();
        kpi.ContainerName = containerName;
        kpi.ProviderType = 1; // PROV_RSA_FULL
        kpi.KeySpec = 1; // AT_KEYEXCHANGE

        certContext = Native.CertCreateSelfSignCertificate(
            providerContext,
            ref nameBlob,
            0,
            ref kpi,
            IntPtr.Zero, // default = SHA1RSA
            ref startSystemTime,
            ref endSystemTime,
            IntPtr.Zero);
        Check(certContext != IntPtr.Zero);
        dataHandle.Free();

        certStore = Native.CertOpenStore(
            "Memory", // sz_CERT_STORE_PROV_MEMORY
            0,
            IntPtr.Zero,
            0x2000, // CERT_STORE_CREATE_NEW_FLAG
            IntPtr.Zero);
        Check(certStore != IntPtr.Zero);

        Check(Native.CertAddCertificateContextToStore(
            certStore,
            certContext,
            1, // CERT_STORE_ADD_NEW
            out storeCertContext));

        Native.CertSetCertificateContextProperty(
            storeCertContext,
            2, // CERT_KEY_PROV_INFO_PROP_ID
            0,
            ref kpi);

        if (password != null)
        {
          passwordPtr = Marshal.SecureStringToCoTaskMemUnicode(password);
        }

        CryptoTypes.CryptoApiBlob pfxBlob = new CryptoTypes.CryptoApiBlob();
        Check(Native.PFXExportCertStoreEx(certStore, ref pfxBlob, passwordPtr, IntPtr.Zero, 7)); // EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY

        pfxData = new byte[pfxBlob.DataLength];
        dataHandle = GCHandle.Alloc(pfxData, GCHandleType.Pinned);
        pfxBlob.Data = dataHandle.AddrOfPinnedObject();
        Check(Native.PFXExportCertStoreEx(certStore, ref pfxBlob, passwordPtr, IntPtr.Zero, 7)); // EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY
        dataHandle.Free();
      }
      finally
      {
        if (passwordPtr != IntPtr.Zero)
        {
          Marshal.ZeroFreeCoTaskMemUnicode(passwordPtr);
        }

        if (dataHandle.IsAllocated)
        {
          dataHandle.Free();
        }

        if (certContext != IntPtr.Zero)
        {
          Native.CertFreeCertificateContext(certContext);
        }

        if (storeCertContext != IntPtr.Zero)
        {
          Native.CertFreeCertificateContext(storeCertContext);
        }

        if (certStore != IntPtr.Zero)
        {
          Native.CertCloseStore(certStore, 0);
        }

        if (cryptKey != IntPtr.Zero)
        {
          Native.CryptDestroyKey(cryptKey);
        }

        if (providerContext != IntPtr.Zero)
        {
          Native.CryptReleaseContext(providerContext, 0);
          Native.CryptAcquireContextW(
              out providerContext,
              containerName,
              null,
              1, // PROV_RSA_FULL
              0x10); // CRYPT_DELETEKEYSET
        }
      }

      return pfxData;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    private static TimeTypes.SystemTime ToSystemTime(DateTime dateTime)
    {
      long fileTime = dateTime.ToFileTime();
      TimeTypes.SystemTime systemTime;
      Check(Time.FileTimeToSystemTime(ref fileTime, out systemTime));
      return systemTime;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="nativeCallSucceeded"></param>
    private static void Check(bool nativeCallSucceeded)
    {
      if (!nativeCallSucceeded)
      {
        int error = Marshal.GetHRForLastWin32Error();
        Marshal.ThrowExceptionForHR(error);
      }
    }

    #endregion

  }
}
