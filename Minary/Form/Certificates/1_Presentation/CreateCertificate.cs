namespace Minary.Certificates.Presentation
{
  using Minary.DataTypes.Enum;
  using Minary.Form;
  using Minary.Form.Main;
  using Minary.LogConsole.Main;
  using System;
  using System.IO;
  using System.Text.RegularExpressions;
  using System.Windows.Forms;


  public partial class CreateCertificate : Form
  {

    #region MEMBERS

    private MinaryMain minaryMain;

    #endregion


    #region PUBLIC

    public CreateCertificate(MinaryMain minaryMain)
    {
      this.InitializeComponent();

      this.minaryMain = minaryMain;
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
        var hostName = this.tb_HostName.Text;
        var certificateFileName = Regex.Replace(hostName, @"[^\d\w_]", "_");
        var certificateOutputPath = Path.Combine(Config.HttpReverseProxyCertrifcateDir, $"{certificateFileName}.pfx");

        NativeWindowsLib.Crypto.Crypto.CreateNewCertificate(certificateOutputPath, hostName, this.dtp_BeginDate.Value, this.dtp_ExpirationDate.Value);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"CreateCertificate: {ex.Message}");
        MessageDialog.Inst.ShowWarning(string.Empty, ex.Message, this);
      }

      this.Close();
    }

    #endregion

  }
}
