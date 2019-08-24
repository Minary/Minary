namespace Minary.LogConsole.Main
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface.LogConsole;
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
    private const string TITLE_BASIS = "Minary Log";

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
      this.Write(LogLevel.Info, $"Minary version : {Config.MinaryVersion}");
      this.Write(LogLevel.Info, $"OS : {Config.OS}");
      this.Write(LogLevel.Info, $"Architecture : {Config.Architecture}");
      this.Write(LogLevel.Info, $"Language : {Config.Language}");
      this.Write(LogLevel.Info, $"Processor : {Config.Processor}");
      this.Write(LogLevel.Info, $"Num. processors : {Config.NumProcessors}");
      this.Write(LogLevel.Info, $".Net version : {Config.DotNetVersion}");
      this.Write(LogLevel.Info, $"CLR version : {Config.CommonLanguateRuntime}");
      this.Write(LogLevel.Info, $"NPcap version : {Config.NPcap}");
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
        var timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        logMessage = logMessage.Trim();
        if (formatArgs?.Count() > 0)
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
        var msg = ex.Message;
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
        instance.tb_LogContent.Clear();
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

      // Get last loglevel 
      string tmpLevel = Common.WinRegistry.GetValue($@"Logging", "Level");
      tmpLevel = string.IsNullOrEmpty(tmpLevel) == false ? tmpLevel : "info";

      switch (tmpLevel.Trim().ToLower())
      {
        case "debug":
          this.SetLogLevel(LogLevel.Debug);
          break;

        case "info":
          this.SetLogLevel(LogLevel.Info);
          break;

        case "warning":
          this.SetLogLevel(LogLevel.Warning);
          break;

        case "error":
          this.SetLogLevel(LogLevel.Error);
          break;

        default:
          this.SetLogLevel(LogLevel.Info);
          break;
      }

      this.Text = $"{TITLE_BASIS}:   Level {this.currentLevel.ToString()}";
    }


    private void SetLogLevel(LogLevel logLevel)
    {
      this.currentLevel = logLevel;
      Common.WinRegistry.CreateOrUpdateValue($@"Software\{Minary.Config.ApplicationName}\Logging", "Level", this.currentLevel.ToString());
      this.Text = $"{TITLE_BASIS}:   Level {this.currentLevel.ToString()}";
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

      if (newLogMessages == null || 
          newLogMessages.Count <= 0)
      {
        return;
      }

      var newLogChunk = string.Join(Environment.NewLine, newLogMessages.Where(elem => elem != null && elem.Length > 0));
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
