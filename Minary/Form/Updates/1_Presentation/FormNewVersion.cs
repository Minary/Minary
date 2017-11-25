namespace Minary.Form.Updates
{
  using System;
  using System.Text;
  using System.Windows.Forms;


  public partial class FormNewVersion : Form
  {

    #region PUBLIC

    public FormNewVersion()
    {
      this.InitializeComponent();

      StringBuilder newVersionMessageStr = new System.Text.StringBuilder();
      string autoupdateStateStr = Minary.Common.WinRegistry.GetValue("Updates", "Autoupdate");
      int autoupdateState = Convert.ToInt32(autoupdateStateStr);
      this.CB_AutoUpdate.Checked = autoupdateState <= 0 ? false : true;

      try
      {
        Config.UpdateData updateMetaData = Minary.Common.Updates.FetchUpdateInformationFromServer();
        newVersionMessageStr.Append(@"{\rtf1\ansi There is a new Minary version (" + updateMetaData.AvailableVersionStr + @") available.\line Click on the link below to get to the download site.");
        newVersionMessageStr.Append(@"\line \line \line");
        newVersionMessageStr.Append(@"{\b \fs20 Changes } \line ");

        if (updateMetaData != null && updateMetaData.Messages != null)
        {
          foreach (string tmpUpdateMessage in updateMetaData.Messages)
          {
            newVersionMessageStr.Append(@"\line \bullet  " + tmpUpdateMessage);
          }
        }

        newVersionMessageStr.Append("}");
      }
      catch (Exception)
      {
      }

      this.rtb_MinaryUpdate.Rtf = newVersionMessageStr.ToString();
    }

    #endregion


    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Close_Click(object sender, EventArgs e)
    {
      base.Dispose();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LL_DownloadURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      System.Diagnostics.Process.Start(Minary.Config.ToolHomepage);
      base.Dispose();
    }


    /// <summary>
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Close();
        return true;
      }
      else
      {
        return base.ProcessDialogKey(keyData);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CB_StateChange(object sender, EventArgs e)
    {
      if (this.CB_AutoUpdate.Checked == false)
      {
        Common.WinRegistry.CreateOrUpdateValue($@"Software\{Minary.Config.ApplicationName}\Updates", "Autoupdate", "0");
      }
      else
      {
        Common.WinRegistry.CreateOrUpdateValue($@"Software\{Minary.Config.ApplicationName}\Updates", "Autoupdate", "1");
      }
    }

    #endregion

  }
}
