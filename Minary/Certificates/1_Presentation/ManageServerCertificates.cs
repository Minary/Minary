namespace Minary.Certificates.Presentation
{
  using Minary.Certificates.DataTypes;
  using System;
  using System.ComponentModel;
  using System.IO;
  using System.Linq;
  using System.Security.Cryptography.X509Certificates;
  using System.Windows.Forms;


  public partial class ManageServerCertificates : Form
  {

    #region MEMBERSS

    private static ManageServerCertificates instance;
    private BindingList<CertificateRecord> certificateRecords;
    private Minary.MinaryMain minaryMain;

    #endregion


    #region PUBLIC

    public static ManageServerCertificates GetInstance(Minary.MinaryMain minaryMain)
    {
      return instance ?? (instance = new ManageServerCertificates(minaryMain));
    }

    #endregion


    #region PRIVATE

    private ManageServerCertificates(Minary.MinaryMain minaryMain)
    {
      this.InitializeComponent();

      DataGridViewTextBoxColumn columnServerName = new DataGridViewTextBoxColumn();
      columnServerName.DataPropertyName = "ServerName";
      columnServerName.Name = "ServerName";
      columnServerName.HeaderText = "Server name";
      columnServerName.ReadOnly = true;
      columnServerName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgv_ServerCertificates.Columns.Add(columnServerName);

      DataGridViewTextBoxColumn columnStartDate = new DataGridViewTextBoxColumn();
      columnStartDate.DataPropertyName = "StartDate";
      columnStartDate.Name = "StartDate";
      columnStartDate.HeaderText = "Start date";
      columnStartDate.ReadOnly = true;
      columnStartDate.Width = 200;
      this.dgv_ServerCertificates.Columns.Add(columnStartDate);

      DataGridViewTextBoxColumn columnExpirationDate = new DataGridViewTextBoxColumn();
      columnExpirationDate.DataPropertyName = "ExpirationDate";
      columnExpirationDate.Name = "ExpirationDate";
      columnExpirationDate.HeaderText = "Expiration date";
      columnExpirationDate.ReadOnly = true;
      columnExpirationDate.Width = 200;
      this.dgv_ServerCertificates.Columns.Add(columnExpirationDate);

      DataGridViewTextBoxColumn columnFileName = new DataGridViewTextBoxColumn();
      columnFileName.DataPropertyName = "FileName";
      columnFileName.Name = "FileName";
      columnFileName.HeaderText = "File name";
      columnFileName.ReadOnly = true;
      columnFileName.Visible = false;
      columnFileName.Width = 0;
      this.dgv_ServerCertificates.Columns.Add(columnFileName);

      this.certificateRecords = new BindingList<CertificateRecord>();
      this.dgv_ServerCertificates.DataSource = this.certificateRecords;

      this.minaryMain = minaryMain;
      this.RefreshCertificateListing();
    }


    private void RefreshCertificateListing()
    {
      this.certificateRecords.Clear();

      if (Directory.Exists(Config.HttpReverseProxyCertrifcateDir))
      {
        foreach (string tmpFile in Directory.GetFiles(Config.HttpReverseProxyCertrifcateDir))
        {
          try
          {
            X509Certificate2Collection certificateCollection = NativeWindowsLib.Crypto.Crypto.GetCertificatesFromStoreFile(tmpFile, string.Empty);
            CertificateRecord tmpRecord = new CertificateRecord();
            X509Certificate2 theCertificate = certificateCollection[0];

            tmpRecord.Issuer = string.IsNullOrEmpty(theCertificate.Issuer) ? string.Empty : theCertificate.Issuer;
            tmpRecord.StartDate = theCertificate.NotBefore == DateTime.MinValue ? DateTime.MinValue : theCertificate.NotBefore;
            tmpRecord.ExpirationDate = theCertificate.NotAfter == DateTime.MinValue ? DateTime.MinValue : theCertificate.NotAfter;
            tmpRecord.SerialNumber = string.IsNullOrEmpty(theCertificate.SerialNumber) ? string.Empty : theCertificate.SerialNumber;
            tmpRecord.SignatureAlgorithm = theCertificate.SignatureAlgorithm != null ? theCertificate.SignatureAlgorithm.FriendlyName : string.Empty;
            tmpRecord.Subject = string.IsNullOrEmpty(theCertificate.Subject) ? string.Empty : theCertificate.Subject;
            tmpRecord.Thumbprint = string.IsNullOrEmpty(theCertificate.Thumbprint) ? string.Empty : theCertificate.Thumbprint;
            tmpRecord.Version = theCertificate.Version;

            if (tmpRecord.Subject.Contains("="))
            {
              tmpRecord.ServerName = tmpRecord.Subject.Split(new char[] { '=' }, 2)[1];
            }
            else
            {
              tmpRecord.ServerName = tmpRecord.Subject;
            }

            tmpRecord.FileName = Path.GetFileName(tmpFile);
            this.certificateRecords.Add(tmpRecord);
          }
          catch
          {
          }
        }
      }
    }


    private CertificateRecord FindCertificateByServerName(string serverName)
    {
      CertificateRecord foundCertificate = null;

      if (string.IsNullOrEmpty(serverName))
      {
        return foundCertificate;
      }

      try
      {
        int selectedIndex = this.dgv_ServerCertificates.CurrentCell.RowIndex;
        foundCertificate = (from record in this.certificateRecords where record.ServerName == serverName select record).First();
      }
      catch
      {
      }

      return foundCertificate;
    }

    #endregion


    #region EVENTS

    private void ManageServerCertificates_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
    }


    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Hide();
        return false;
      }
      else
      {
        return base.ProcessDialogKey(keyData);
      }
    }


    private void DGV_ServerCertificates_MouseDown(object sender, MouseEventArgs e)
    {
      // Select row
      try
      {
        DataGridView.HitTestInfo hti = this.dgv_ServerCertificates.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          this.dgv_ServerCertificates.ClearSelection();
          this.dgv_ServerCertificates.Rows[hti.RowIndex].Selected = true;
          this.dgv_ServerCertificates.CurrentCell = this.dgv_ServerCertificates.Rows[hti.RowIndex].Cells["ServerName"];
        }
      }
      catch (Exception)
      {
        this.dgv_ServerCertificates.ClearSelection();
      }
    }


    private void DGV_ServerCertificates_MouseUp(object sender, MouseEventArgs e)
    {
      // Select row
      try
      {
        DataGridView.HitTestInfo hti = this.dgv_ServerCertificates.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          this.dgv_ServerCertificates.ClearSelection();
          this.dgv_ServerCertificates.Rows[hti.RowIndex].Selected = true;
          this.dgv_ServerCertificates.CurrentCell = this.dgv_ServerCertificates.Rows[hti.RowIndex].Cells["ServerName"];
        }
      }
      catch (Exception ex)
      {
        this.dgv_ServerCertificates.ClearSelection();
      }

      // Show context menu strip
      if (e.Button == MouseButtons.Right)
      {
        try
        {
          DataGridView.HitTestInfo hti = this.dgv_ServerCertificates.HitTest(e.X, e.Y);
          this.cmsServerCertificates_Delete.Visible = (hti.RowIndex >= 0);
          this.cms_ManageCertificates.Show(this.dgv_ServerCertificates, e.Location);
        }
        catch (Exception ex)
        {
          //this.pluginProperties.HostApplication.LogMessage("ManageServerCertificate: {0}", ex.Message);
        }
      }
    }


    private void BT_Close_Click(object sender, EventArgs e)
    {
      this.Hide();
    }


    private void CmsServerCertificates_Add_Click(object sender, EventArgs e)
    {
      CreateCertificate certificateDialog = new CreateCertificate(this.minaryMain);
      certificateDialog.ShowDialog();
      this.RefreshCertificateListing();
    }


    private void CmsServerCertificates_Delete_Click(object sender, EventArgs e)
    {
      CertificateRecord certificateRecord = null;
      int selectedIndex = -1;
      string serverName = string.Empty;

      if (this.dgv_ServerCertificates.CurrentCell == null || this.dgv_ServerCertificates.CurrentCell.RowIndex < 0)
      {
        return;
      }

      try
      {
        selectedIndex = this.dgv_ServerCertificates.CurrentCell.RowIndex;
        serverName = this.dgv_ServerCertificates.Rows[selectedIndex].Cells["ServerName"].Value.ToString();
        certificateRecord = this.FindCertificateByServerName(serverName);
      }
      catch
      {
        return;
      }

      if (certificateRecord == null)
      {
        return;
      }

      if (string.IsNullOrEmpty(certificateRecord.FileName))
      {
        return;
      }

      string filePath = Path.Combine(Config.HttpReverseProxyCertrifcateDir, certificateRecord.FileName);
      if (!File.Exists(filePath))
      {
        return;
      }

      try
      {
        File.Delete(filePath);
      }
      catch (Exception)
      {
      }

      this.RefreshCertificateListing();
    }


    private void DGV_ServerCertificates_DoubleClick(object sender, EventArgs e)
    {
      CertificateRecord certificateRecord = null;
      int selectedIndex = -1;
      string serverName = string.Empty;

      if (this.dgv_ServerCertificates.CurrentCell == null || this.dgv_ServerCertificates.CurrentCell.RowIndex < 0)
      {
        return;
      }

      try
      {
        selectedIndex = this.dgv_ServerCertificates.CurrentCell.RowIndex;
        serverName = this.dgv_ServerCertificates.Rows[selectedIndex].Cells["ServerName"].Value.ToString();
        certificateRecord = this.FindCertificateByServerName(serverName);

        CertificateDetails certificateDetails = new CertificateDetails(certificateRecord);
        certificateDetails.ShowDialog();
      }
      catch
      {
      }
    }

    #endregion

  }
}
