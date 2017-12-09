namespace Minary.Form
{
  using System;
  using System.Windows.Forms;


  public partial class FormWinPcapMissing : Form
  {

    #region MEMBERS

    private string winPcapUrl;
    private string winPcapUrlAndPath;

    #endregion


    #region PUBLIC

    public FormWinPcapMissing()
    {
      this.InitializeComponent();

      this.winPcapUrl = "https://www.winpcap.org";
      this.winPcapUrlAndPath = $"{this.winPcapUrl}/install/default.htm";
      this.LL_WinPcapURL.Text = this.winPcapUrl;
      this.RTB_Message.Text = "Minary cannot be started because WinPcap is not " +
                              "installed on your system. Follow  the link bellow " +
                              "to get the latest WinPcap version.";
    }

    #endregion


    #region PRIVATE

    private void LL_WinPcapURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(this.winPcapUrlAndPath);
    }


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
