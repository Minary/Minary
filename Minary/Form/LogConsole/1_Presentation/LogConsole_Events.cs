namespace Minary.LogConsole.Main
{
  using Minary.DataTypes.Enum;
  using System;
  using System.Windows.Forms;


  public partial class LogCons : Form
  {

    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LogConsole_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.Hide();
      e.Cancel = true;
    }


    /// <summary>
    /// Hide Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
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


    private void TSMI_Loglevel_Click(object sender, EventArgs e)
    {
      var clickedItem = sender as ToolStripMenuItem;
      if (clickedItem != null)
      {
        this.currentLevelObject.CheckState = CheckState.Unchecked;
        this.currentLevelObject = clickedItem;
        this.currentLevelObject.CheckState = CheckState.Checked;

        var tagName = clickedItem.Tag.ToString().ToLower();
        if (tagName == "debug")
        {
          this.currentLevel = LogLevel.Debug;
        }
        else if (tagName == "info")
        {
          this.currentLevel = LogLevel.Info;
        }
        else if (tagName == "warning")
        {
          this.currentLevel = LogLevel.Warning;
        }
        else if (tagName == "error")
        {
          this.currentLevel = LogLevel.Error;
        }
        else if (tagName == "fatal")
        {
          this.currentLevel = LogLevel.Fatal;
        }
      }
    }


    private void TSMI_ClearLog_Click(object sender, EventArgs e)
    {
      lock (this)
      {
        inst.tb_LogContent.Clear();
      }
    }


    private void TSMI_Close_Click(object sender, EventArgs e)
    {
      this.Hide();
    }


    private void TSMI_LogLevel_Debug_Click(object sender, EventArgs e)
    {
      SetLogLevel(LogLevel.Debug);
    }


    private void TSMI_LogLevel_Info_Click(object sender, EventArgs e)
    {
      SetLogLevel(LogLevel.Info);
    }


    private void TSMI_LogLevel_Warning_Click(object sender, EventArgs e)
    {
      SetLogLevel(LogLevel.Warning);
    }


    private void TSMI_LogLevel_Error_Click(object sender, EventArgs e)
    {
      SetLogLevel(LogLevel.Error);
    }

    #endregion
  }
}
