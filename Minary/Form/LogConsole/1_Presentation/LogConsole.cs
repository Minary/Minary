namespace Minary.LogConsole.Main
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;


  public partial class LogCons : Form, IObserver
  {

    #region MEMBERS

    private static LogCons instance;
    private Task.LogConsole logConsoleTask;
    private LogLevel currentLevel;
    private ToolStripMenuItem currentLevelObject;

    #endregion


    #region PROPERTIES

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static LogCons Inst
    {
      get { return instance ?? (instance = new LogCons()); }
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
    public void DumpSystemInformation()
    {
      this.Write(LogLevel.Info, "Starting Log console");
      this.Write(LogLevel.Info, "Minary version : {0}", Config.MinaryVersion);
      this.Write(LogLevel.Info, "OS : {0}", Config.OS);
      this.Write(LogLevel.Info, "Architecture : {0}", Config.Architecture);
      this.Write(LogLevel.Info, "Language : {0}", Config.Language);
      this.Write(LogLevel.Info, "Processor : {0}", Config.Processor);
      this.Write(LogLevel.Info, "Num. processors : {0}", Config.NumProcessors);
      this.Write(LogLevel.Info, ".Net version : {0}", Config.DotNetVersion);
      this.Write(LogLevel.Info, "CLR version : {0}", Config.CommonLanguateRuntime);
      this.Write(LogLevel.Info, "WinPcap version : {0}", Config.WinPcap);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="formatArgs"></param>
    public delegate void WriteDelegate(LogLevel level, string message, params object[] formatArgs);
    public void Write(LogLevel level, string logMessage, params object[] formatArgs)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new WriteDelegate(this.Write), new object[] { level, logMessage, formatArgs });
        return;
      }

      if (level < this.currentLevel)
      {
        return;
      }

      if (string.IsNullOrEmpty(logMessage))
      {
        return;
      }

      try
      {
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        logMessage = logMessage.Trim();
        if (formatArgs != null && formatArgs.Count() > 0)
        {
          logMessage = string.Format(logMessage, formatArgs);
        }

        // this.logConsoleTask.AddLogMessage(message);
        logMessage = $"{timeStamp, -25}{level, -10}{logMessage}{Environment.NewLine}";

        this.tb_LogContent.AppendText(logMessage);
        this.tb_LogContent.SelectionStart = this.tb_LogContent.Text.Length;
        this.tb_LogContent.ScrollToCaret();
      }
      catch (Exception ex)
      {
        // OOOPPPS!
        string msg = ex.Message;
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


    private void LoglevelToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
      if (clickedItem != null)
      {
        this.currentLevelObject.CheckState = CheckState.Unchecked;
        this.currentLevelObject = clickedItem;
        this.currentLevelObject.CheckState = CheckState.Checked;

        string tagName = clickedItem.Tag.ToString().ToLower();
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


    private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
    {
      lock (this)
      {
        instance.tb_LogContent.Clear();
      }
    }

    #endregion


    #region PRIVATE

    private LogCons()
    {
      this.InitializeComponent();
      this.logConsoleTask = new Task.LogConsole();
      this.logConsoleTask.AddObserver(this);

      this.currentLevel = LogLevel.Info;
      this.currentLevelObject = this.TSMI_Info;
      this.currentLevelObject.CheckState = CheckState.Checked;

      // The parent (Main GUI) must not have any visual/behavioral
      // influence on the log console
      this.Owner = null;
    }

    #endregion


    #region INTERFACE: IObserver

    public delegate void UpdateLogDelegate(List<string> newLogMessages);
    public void UpdateLog(List<string> newLogMessages)
    {
      if (this.tb_LogContent.InvokeRequired)
      {
        this.tb_LogContent.BeginInvoke(new UpdateLogDelegate(this.UpdateLog), new object[] { newLogMessages });
        return;
      }

      if (newLogMessages == null && newLogMessages.Count <= 0)
      {
        return;
      }

      string newLogChunk = string.Join(Environment.NewLine, newLogMessages.Where(elem => elem != null && elem.Length > 0));

      if (string.IsNullOrEmpty(newLogChunk))
      {
        return;
      }

      this.tb_LogContent.AppendText(newLogChunk);
      this.tb_LogContent.SelectionStart = this.tb_LogContent.Text.Length;
      this.tb_LogContent.ScrollToCaret();
    }

    #endregion

  }
}
