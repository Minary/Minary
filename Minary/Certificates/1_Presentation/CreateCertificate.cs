namespace Minary.Certificates.Presentation
{
  using System;
  using System.IO;
  using System.Text.RegularExpressions;
  using System.Windows.Forms;


  public partial class CreateCertificate : Form
  {

    #region MEMBERS

    private Task.CreateCertificate certificateTaskLayer;
    private Minary.MinaryMain minaryMain;

    #endregion


    #region PUBLIC

    public CreateCertificate(Minary.MinaryMain minaryMain)
    {
      this.InitializeComponent();

      this.minaryMain = minaryMain;
      this.certificateTaskLayer = new Task.CreateCertificate();
      this.dtp_BeginDate.Value = DateTime.Now.AddDays(-1);
      this.dtp_ExpirationDate.Value = DateTime.Now.AddYears(2);
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
        this.Hide();
        return false;
      }
      else
      {
        return base.ProcessDialogKey(keyData);
      }
    }


    private void BT_Add_Click(object sender, EventArgs e)
    {
      try
      {
        string hostName = this.tb_HostName.Text;
        string certificateFileName = Regex.Replace(hostName, @"[^\d\w_]", "_");
        string certificateOutputPath = Path.Combine(Config.HttpReverseProxyCertrifcateDir, string.Format("{0}.pfx", certificateFileName));

        NativeWindowsLib.Crypto.Crypto.CreateNewCertificate(certificateOutputPath, hostName, this.dtp_BeginDate.Value, this.dtp_ExpirationDate.Value);
      }
      catch (Exception ex)
      {
        this.minaryMain.LogConsole.LogMessage("CreateCertificate: {0}", ex.Message);
        MessageBox.Show(string.Format("{0}", ex.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }

      this.Close();
    }

    #endregion
  }
}
