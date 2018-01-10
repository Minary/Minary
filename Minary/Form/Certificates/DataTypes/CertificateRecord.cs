namespace Minary.Certificates.DataTypes
{
  using System;
  using System.ComponentModel;


  public class CertificateRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private string fileName = string.Empty;
    private string serverName = string.Empty;
    private string issuer = string.Empty;
    private string subject = string.Empty;
    private DateTime startDate = DateTime.MinValue;
    private DateTime expirationDate = DateTime.MinValue;
    private string serialNumber = string.Empty;
    private string signatureAlgorithm = string.Empty;
    private string thumbprint = string.Empty;
    private int version = -1;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region PUBLIC

    public CertificateRecord()
    {
    }


    public CertificateRecord(string fileName, string serverName, string issuer, string subject, DateTime startDate, DateTime expirationDate, string serialNumber, string signatureAlgorithm, string thumbprint, int version)
    {
      this.fileName = fileName;
      this.serverName = serverName;
      this.issuer = issuer;
      this.subject = subject;
      this.startDate = startDate;
      this.expirationDate = expirationDate;
      this.serialNumber = serialNumber;
      this.signatureAlgorithm = signatureAlgorithm;
      this.thumbprint = thumbprint;
      this.version = version;
    }

    #endregion


    #region PROPERTIES

    [Browsable(true)]
    public string ServerName
    {
      get
      {
        return this.serverName;
      }

      set
      {
        this.serverName = value;
        this.NotifyPropertyChanged("ServerName");
      }
    }


    [Browsable(false)]
    public string Issuer
    {
      get
      {
        return this.issuer;
      }

      set
      {
        this.issuer = value;
        this.NotifyPropertyChanged("Issuer");
      }
    }


    [Browsable(false)]
    public string Subject
    {
      get
      {
        return this.subject;
      }

      set
      {
        this.subject = value;
        this.NotifyPropertyChanged("Subject");
      }
    }

 
    [Browsable(true)]
    public DateTime StartDate
    {
      get
      {
        return this.startDate;
      }

      set
      {
        this.startDate = value;
        this.NotifyPropertyChanged("StartDate");
      }
    }


    [Browsable(true)]
    public DateTime ExpirationDate
    {
      get
      {
        return this.expirationDate;
      }

      set
      {
        this.expirationDate = value;
        this.NotifyPropertyChanged("ExpirationDate");
      }
    }


    [Browsable(false)]
    public string SerialNumber
    {
      get
      {
        return this.serialNumber;
      }

      set
      {
        this.serialNumber = value;
        this.NotifyPropertyChanged("SerialNumber");
      }
    }


    [Browsable(false)]
    public string SignatureAlgorithm
    {
      get
      {
        return this.signatureAlgorithm;
      }

      set
      {
        this.signatureAlgorithm = value;
        this.NotifyPropertyChanged("SignatureAlgorithm");
      }
    }


    [Browsable(false)]
    public string Thumbprint
    {
      get
      {
        return this.thumbprint;
      }

      set
      {
        this.thumbprint = value;
        this.NotifyPropertyChanged("Thumbprint");
      }
    }


    [Browsable(false)]
    public int Version
    {
      get
      {
        return this.version;
      }

      set
      {
        this.version = value;
        this.NotifyPropertyChanged("Version");
      }
    }

    [Browsable(false)]
    public string FileName
    {
      get
      {
        return this.fileName;
      }

      set
      {
        this.fileName = value;
        this.NotifyPropertyChanged("FileName");
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    private void NotifyPropertyChanged(string name)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }

    #endregion

  }
}
