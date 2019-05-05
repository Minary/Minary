namespace Minary.Form
{
  using System;
  using System.Windows.Forms;


  public partial class FormNPcapMissing : Form
  {

    #region MEMBERS

    private string nPcapUrl;
    private string nPcapUrlAndPath;

    #endregion


    #region PUBLIC

    public FormNPcapMissing()
    {
      this.InitializeComponent();

      this.nPcapUrl = "https://nmap.org/npcap/";
      this.nPcapUrlAndPath = $"https://nmap.org/npcap/#download";
      this.LL_NPcapURL.Text = this.nPcapUrl;
      this.RTB_Message.Text = "Minary cannot be started because NPcap is not " +
                              "installed on your system. Follow  the link bellow " +
                              "to get the latest NPcap version.";
    }

    #endregion


    #region PRIVATE

    private void LL_NPcapURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(this.nPcapUrlAndPath);
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
