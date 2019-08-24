namespace Minary.LogConsole.Main
{
  using Minary.DataTypes.Enum;
  using System;
  using System.Linq;
  using System.Windows.Forms;


  public partial class LogCons : Form
  {

    #region MEMBERS

    private static LogCons inst;
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
      get { return inst ?? (inst = new LogCons()); }
      set { }
    }

    #endregion


    #region PUBLIC
    
    /// <summary>
    /// /
    /// </summary>
    public void ShowLogConsole()
    {
      inst.Show();
      inst.BringToFront();
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

  }
}
