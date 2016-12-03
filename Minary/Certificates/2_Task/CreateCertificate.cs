namespace Minary.Certificates.Task
{
  using Minary.Certificates.DataTypes;
  using NativeWindowsLib;
  using System;
  using System.IO;
  using System.Runtime.InteropServices;
  using System.Security.Cryptography.X509Certificates;
  using System.Text.RegularExpressions;
  using CryptoTypes = NativeWindowsLib.DataTypes.Crypto;
  using TimeTypes = NativeWindowsLib.DataTypes.Time;
  using RTHelpers = System.Runtime.CompilerServices.RuntimeHelpers;
  using Securestring = System.Security.SecureString;


  public class CreateCertificate
  {

    #region PUBLIC

    public CertificateRecord ReadCertificateFile(string certificateFilePath, string password)
    {
      CertificateRecord certificate = new CertificateRecord();

      // Create a collection object and populate it using the PFX file
      X509Certificate2Collection certificateCollection = new X509Certificate2Collection();
      certificateCollection.Import(certificateFilePath, password, X509KeyStorageFlags.PersistKeySet);

      if (certificateCollection.Count <= 0)
      {
        throw new Exception("No valid certificate found in store");
      }

      if (certificateCollection.Count > 1)
      {
        throw new Exception("More than one certificate in store found");
      }

      X509Certificate2 theCertificate = certificateCollection[0];
      certificate.Issuer = string.IsNullOrEmpty(theCertificate.Issuer) ? string.Empty : theCertificate.Issuer;
      certificate.StartDate = theCertificate.NotBefore == DateTime.MinValue ? DateTime.MinValue : theCertificate.NotBefore;
      certificate.ExpirationDate = theCertificate.NotAfter == DateTime.MinValue ? DateTime.MinValue : theCertificate.NotAfter;
      certificate.SerialNumber = string.IsNullOrEmpty(theCertificate.SerialNumber) ? string.Empty : theCertificate.SerialNumber;
      certificate.SignatureAlgorithm = theCertificate.SignatureAlgorithm != null ? theCertificate.SignatureAlgorithm.FriendlyName : string.Empty;
      certificate.Subject = string.IsNullOrEmpty(theCertificate.Subject) ? string.Empty : theCertificate.Subject;
      certificate.Thumbprint = string.IsNullOrEmpty(theCertificate.Thumbprint) ? string.Empty : theCertificate.Thumbprint;
      certificate.Version = theCertificate.Version;

      if (certificate.Subject.Contains("="))
      {
        certificate.ServerName = certificate.Subject.Split(new char[] { '=' }, 2)[1];
      }
      else
      {
        certificate.ServerName = certificate.Subject;
      }

      return certificate;
    }


    public void CreateNewCertificate(string hostName, DateTime validityStartDate, DateTime validityEndDate)
    {
      if (string.IsNullOrEmpty(hostName) || string.IsNullOrWhiteSpace(hostName))
      {
        throw new Exception("The certificate output path is invalid");
      }

      string certificateFileName = Regex.Replace(hostName, @"[^\d\w_]", "_");
      string certificateOutputPath = Path.Combine(Config.HttpReverseProxyCertrifcateDir, string.Format("{0}.pfx", certificateFileName));
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

      byte[] c = this.CreateSelfSignCertificatePfx(
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
    private byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime, string insecurePassword)
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

        pfxData = this.CreateSelfSignCertificatePfx(x500, startTime, endTime, password);
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
    private byte[] CreateSelfSignCertificatePfx(string x500, DateTime startTime, DateTime endTime, Securestring password)
    {
      byte[] pfxData;

      if (x500 == null)
      {
        x500 = string.Empty;
      }

      TimeTypes.SystemTime startSystemTime = this.ToSystemTime(startTime);
      TimeTypes.SystemTime endSystemTime = this.ToSystemTime(endTime);
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
        this.Check(Crypto.CryptAcquireContextW(
            out providerContext,
            containerName,
            null,
            1, // PROV_RSA_FULL
            8)); // CRYPT_NEWKEYSET

        this.Check(Crypto.CryptGenKey(
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

        if (!Crypto.CertStrToNameW(
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

        if (!Crypto.CertStrToNameW(
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

        certContext = Crypto.CertCreateSelfSignCertificate(
            providerContext,
            ref nameBlob,
            0,
            ref kpi,
            IntPtr.Zero, // default = SHA1RSA
            ref startSystemTime,
            ref endSystemTime,
            IntPtr.Zero);
        this.Check(certContext != IntPtr.Zero);
        dataHandle.Free();

        certStore = Crypto.CertOpenStore(
            "Memory", // sz_CERT_STORE_PROV_MEMORY
            0,
            IntPtr.Zero,
            0x2000, // CERT_STORE_CREATE_NEW_FLAG
            IntPtr.Zero);
        this.Check(certStore != IntPtr.Zero);

        this.Check(Crypto.CertAddCertificateContextToStore(
            certStore,
            certContext,
            1, // CERT_STORE_ADD_NEW
            out storeCertContext));

        Crypto.CertSetCertificateContextProperty(
            storeCertContext,
            2, // CERT_KEY_PROV_INFO_PROP_ID
            0,
            ref kpi);

        if (password != null)
        {
          passwordPtr = Marshal.SecureStringToCoTaskMemUnicode(password);
        }

        CryptoTypes.CryptoApiBlob pfxBlob = new CryptoTypes.CryptoApiBlob();
        this.Check(Crypto.PFXExportCertStoreEx(certStore, ref pfxBlob, passwordPtr, IntPtr.Zero, 7)); // EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY

        pfxData = new byte[pfxBlob.DataLength];
        dataHandle = GCHandle.Alloc(pfxData, GCHandleType.Pinned);
        pfxBlob.Data = dataHandle.AddrOfPinnedObject();
        this.Check(Crypto.PFXExportCertStoreEx(certStore, ref pfxBlob, passwordPtr, IntPtr.Zero, 7)); // EXPORT_PRIVATE_KEYS | REPORT_NO_PRIVATE_KEY | REPORT_NOT_ABLE_TO_EXPORT_PRIVATE_KEY
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
          Crypto.CertFreeCertificateContext(certContext);
        }

        if (storeCertContext != IntPtr.Zero)
        {
          Crypto.CertFreeCertificateContext(storeCertContext);
        }

        if (certStore != IntPtr.Zero)
        {
          Crypto.CertCloseStore(certStore, 0);
        }

        if (cryptKey != IntPtr.Zero)
        {
          Crypto.CryptDestroyKey(cryptKey);
        }

        if (providerContext != IntPtr.Zero)
        {
          Crypto.CryptReleaseContext(providerContext, 0);
          Crypto.CryptAcquireContextW(
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
    private TimeTypes.SystemTime ToSystemTime(DateTime dateTime)
    {
      long fileTime = dateTime.ToFileTime();
      TimeTypes.SystemTime systemTime;
      this.Check(Time.FileTimeToSystemTime(ref fileTime, out systemTime));
      return systemTime;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="nativeCallSucceeded"></param>
    private void Check(bool nativeCallSucceeded)
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