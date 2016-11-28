namespace Minary.LogConsole.Main
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;


  public partial class LogConsole : Form
  {

    #region MEMBERS

    private static LogConsole instance;

    #endregion


    #region PROPERTIES

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static LogConsole LogInstance
    {
      get { return instance ?? (instance = new LogConsole()); }
      set { }
    }

    #endregion


    #region PUBLIC

    /// <summary>
    /// /
    /// </summary>
    public void ShowLogConsole()
    {
      instance.Show();
      instance.BringToFront();
    }


    /// <summary>
    ///
    /// </summary>
    public void InitializeLogConsole()
    {
      this.LogMessage("Starting Log console");
      this.LogMessage("Minary version : {0}", Config.MinaryVersion);
      this.LogMessage("OS : {0}", Config.OS);
      this.LogMessage("Architecture : {0}", Config.Architecture);
      this.LogMessage("Language : {0}", Config.Language);
      this.LogMessage("Processor : {0}", Config.Processor);
      this.LogMessage("Num. processors : {0}", Config.NumProcessors);
      this.LogMessage(".Net version : {0}", Config.DotNetVersion);
      this.LogMessage("CLR version : {0}", Config.CommonLanguateRuntime);
      this.LogMessage("WinPcap version : {0}", Config.WinPcap);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="formatArgs"></param>
    public delegate void LogMessageDelegate(string message, params object[] formatArgs);

    public void LogMessage(string message, params object[] formatArgs)
    {
      string timeStamp = string.Empty;
      DateTime timeNow = DateTime.Now;

      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      try
      {
        timeStamp = timeNow.ToString("yyyy-MM-dd-HH:mm:ss");
        message = message.Trim();

        if (formatArgs != null && formatArgs.Count() > 0)
        {
          message = string.Format(message, formatArgs);
        }

        string realLogMessage = string.Format("{0} - {1}", timeStamp, message);
        instance.tb_LogContent.AppendText(realLogMessage + Environment.NewLine);
        instance.tb_LogContent.SelectionStart = instance.tb_LogContent.Text.Length;
        instance.tb_LogContent.ScrollToCaret();
      }
      catch (Exception ex)
      {
      }
    }

    #endregion


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
      else if (keyData == (Keys.Control | Keys.R))
      {
        lock (this)
        {
          instance.tb_LogContent.Clear();
        }

        return false;
      }
      else
      {
        return base.ProcessDialogKey(keyData);
      }
    }

    #endregion


    #region PRIVATE

    public LogConsole()
    {
      this.InitializeComponent();
    }

    #endregion

  }
}
