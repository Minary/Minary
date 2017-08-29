namespace Minary.LogConsole.Main
{
  using Minary.LogConsole.DataTypes;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;


  public partial class LogCons : Form, IObserver
  {

    #region MEMBERS

    private static LogCons instance;
    private Task.LogConsole logConsoleTask;

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
      this.Write("Starting Log console");
      this.Write("Minary version : {0}", Config.MinaryVersion);
      this.Write("OS : {0}", Config.OS);
      this.Write("Architecture : {0}", Config.Architecture);
      this.Write("Language : {0}", Config.Language);
      this.Write("Processor : {0}", Config.Processor);
      this.Write("Num. processors : {0}", Config.NumProcessors);
      this.Write(".Net version : {0}", Config.DotNetVersion);
      this.Write("CLR version : {0}", Config.CommonLanguateRuntime);
      this.Write("WinPcap version : {0}", Config.WinPcap);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="formatArgs"></param>
    public delegate void WriteDelegate(string message, params object[] formatArgs);
    public void Write(string message, params object[] formatArgs)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new WriteDelegate(this.Write), new object[] { message, formatArgs });
        return;
      }


      if (string.IsNullOrEmpty(message))
      {
        return;
      }

      try
      {
        message = message.Trim();
        if (formatArgs != null && formatArgs.Count() > 0)
        {
          message = string.Format(message, formatArgs);
        }

        // this.logConsoleTask.AddLogMessage(message);
        message += Environment.NewLine;
        this.tb_LogContent.AppendText(message);
        this.tb_LogContent.SelectionStart = this.tb_LogContent.Text.Length;
        this.tb_LogContent.ScrollToCaret();
      }
      catch
      {
        // OOOPPPS!
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


    #region PRIVATE

    public LogCons()
    {
      this.InitializeComponent();
      this.logConsoleTask = new Task.LogConsole();
      this.logConsoleTask.AddObserver(this);
    }

    #endregion

  }
}
