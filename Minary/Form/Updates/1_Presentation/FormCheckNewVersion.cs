namespace Minary.Form.Updates.Presentation
{
  using Minary.DataTypes.Interface.Updates;
  using System;
  using System.Diagnostics;
  using System.Text;
  using System.Windows.Forms;
  using Minary.Form.Updates.Config;

  public partial class FormCheckNewVersion : Form, IObserver
  {

    #region MEMBERS

    private Task.TaskFacade taskFacade;

    #endregion


    #region PUBLIC

    public FormCheckNewVersion()
    {
      this.InitializeComponent();

      // Determine and set autoupdate status
      string autoupdateStateStr = Minary.Common.WinRegistry.GetValue("Updates", "Autoupdate");
      int autoupdateState = Convert.ToInt32(autoupdateStateStr);
      this.CB_AutoUpdate.Checked = autoupdateState <= 0 ? false : true;

      // Set updates loading message
      StringBuilder newVersionMessageStr = new StringBuilder();
      newVersionMessageStr.Append($@"{{\rtf1\ansi Contacting the server to search for new Minary version ...\b \line ");
      newVersionMessageStr.Append(@"}} \line \line ");
      newVersionMessageStr.Append(@"\b Information \b0 about Minary or the latest attack templates are available at {{\field {{\*\fldinst HYPERLINK https://minary.io }} }} \line \line ");
      newVersionMessageStr.Append(@"\b Contributions \b0 to the project all source code is available on Github at {{\field{{\*\fldinst HYPERLINK https://github.com/minary }} }} \line ");
      this.rtb_MinaryUpdate.Rtf = newVersionMessageStr.ToString();

      // Initialize task layer
      this.taskFacade = new Task.TaskFacade();
      this.taskFacade.AddObserver(this);

      // Check for updates
      this.taskFacade.SearchNewVersion();
    }

    #endregion


    #region PRIVATE

    private void ShowMessageUpdatesAvailable(UpdateData updateData)
    {
      this.Text = "New update available";

      // Set up "updates available" message
      StringBuilder newVersionMessageStr = new StringBuilder();
      newVersionMessageStr.Append($@"{{\rtf1\ansi There is a new Minary version \b {updateData.AvailableVersionStr} \b0 available.\line ");
      newVersionMessageStr.Append(@"Click on the link below to get to the download site. \line \line ");
      newVersionMessageStr.Append($@"\b Windows binary \b0  {{\field {{\*\fldinst HYPERLINK {updateData.WinBinaryDownloadUrl} }} }} \line ");
      newVersionMessageStr.Append($@"\b Source code \b0       {{\field {{\*\fldinst HYPERLINK {updateData.SourceDownloadUrl} }} }} \line \line \line ");

      newVersionMessageStr.Append(@"\b Changes \b0 \line ");
      if (updateData != null && updateData.Messages != null)
      {
        foreach (string tmpUpdateMessage in updateData.Messages)
        {
          string tmpBuffer = tmpUpdateMessage.TrimStart(new char[] { '*', ' ', '\t', '"', '\'' });
          tmpBuffer = tmpBuffer.TrimEnd();
          if (string.IsNullOrEmpty(tmpBuffer))
          {
            continue;
          }

          newVersionMessageStr.Append($@" - {tmpBuffer} \line ");
        }
      }

      newVersionMessageStr.Append(@"}} \line \line ");
      newVersionMessageStr.Append(@"\b Information \b0 about Minary or the latest attack templates are available at {{\field {{\*\fldinst HYPERLINK https://minary.io. }} }} \line \line ");
      newVersionMessageStr.Append(@"\b Contributions \b0 to the project all source code is available on Github at {{\field{{\*\fldinst HYPERLINK https://github.com/minary. }} }} \line ");
      this.rtb_MinaryUpdate.Rtf = newVersionMessageStr.ToString();
    }


    private void ShowMessageNoUpdatesAvailable(UpdateData updateData)
    {
      this.Text = "No update available";
      // Set up "updates available" message
      StringBuilder noNewVersionMessageStr = new StringBuilder();
      noNewVersionMessageStr.Append(@"{\rtf1\ansi You have installed the latest version of Minary. Nice!\line \line \line ");
      noNewVersionMessageStr.Append(@"\b Information \b0 about Minary or the latest attack templates are available at {{\field {{\*\fldinst HYPERLINK https://minary.io. }} }} \line \line ");
      noNewVersionMessageStr.Append(@"\b Source code \b0 of the project is available on Github at {{\field{{\*\fldinst HYPERLINK https://github.com/minary. }} }} Contributions in form of" +
                                    @" bug reports, flaw reports or feature implementations are very appreciated.\line ");
      noNewVersionMessageStr.Append(@"\line ");
      noNewVersionMessageStr.Append("}");

      this.rtb_MinaryUpdate.Rtf = noNewVersionMessageStr.ToString();
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


    private void RTB_MinaryUpdate_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      Process.Start(e.LinkText);
    }

    #endregion


    #region INTERFACE: IObserver

    public delegate void UpdateDelegate(UpdateData updateData);
    public void Update(UpdateData updateData)
    {
      if (this.InvokeRequired == true)
      {
        this.BeginInvoke(new UpdateDelegate(this.Update), new object[] { updateData });
        return;
      }

      if (updateData.IsUpdateAvailable == true)
      {
        this.ShowMessageUpdatesAvailable(updateData);
      }
      else
      {
        this.ShowMessageNoUpdatesAvailable(updateData);
      }
    }

    #endregion

  }
}
