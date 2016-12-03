namespace Minary.Certificates.Presentation
{
  using Minary.Certificates.DataTypes;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Forms;


  public partial class CertificateDetails : Form
  {

    #region PUBLIC

    public CertificateDetails(CertificateRecord certificateRecord)
    {
      this.InitializeComponent();

      this.l_IssuerValue.Text = certificateRecord.Issuer;
      this.l_SubjectValue.Text = certificateRecord.Subject;
      this.l_BeginDateValue.Text = certificateRecord.StartDate.ToString();
      this.l_ExpirationDateValue.Text = certificateRecord.ExpirationDate.ToString();
      this.l_SignatureAlgorithmValue.Text = certificateRecord.SignatureAlgorithm;
      this.l_SerialNumberValue.Text = certificateRecord.SerialNumber;
      this.l_ThumbprintValue.Text = certificateRecord.Thumbprint;
      this.l_VersionValue.Text = certificateRecord.Version.ToString();
    }

    #endregion


    #region EVENTS

    private void BT_Close_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Close();
        return false;
      }
      else
      {
        return base.ProcessDialogKey(keyData);
      }
    }

    #endregion

  }
}
