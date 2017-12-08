namespace Minary.Form
{
  using System;
  using System.Windows.Forms;


  public partial class FormWinPcapMissing : Form
  {

    #region MEMBERS

    private const string winPcapUrl = "https://www.winpcap.org/install/default.htm";

    #endregion


    #region PUBLIC

    public FormWinPcapMissing()
    {
      this.InitializeComponent();

      this.LL_WinPcapURL.Text = winPcapUrl;
      this.RTB_Message.Text = "Minary cannot be started because WinPcap is not " +
                              "installed on your system. Follow  the link bellow " +
                              "to get the latest WinPcap version.";
    }

    #endregion


    #region PRIVATE

    private void LL_WinPcapURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(winPcapUrl);
    }

    #endregion

  }
}
