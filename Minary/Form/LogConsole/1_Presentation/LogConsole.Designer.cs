namespace Minary.LogConsole.Main
{
  public partial class LogCons
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogCons));
      this.tb_LogContent = new System.Windows.Forms.TextBox();
      this.MS_LogConsole = new System.Windows.Forms.MenuStrip();
      this.TSMI_Level = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Debug = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Info = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Warning = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Error = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Fatal = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ms_LogMain = new System.Windows.Forms.MenuStrip();
      this.TSMI_File = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Close = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_Edit = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_ClearLog = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_SetLogLevel = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_LogLevel_Debug = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_LogLevel_Info = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_LogLevel_Warning = new System.Windows.Forms.ToolStripMenuItem();
      this.TSMI_LogLevel_Error = new System.Windows.Forms.ToolStripMenuItem();
      this.ms_LogMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // tb_LogContent
      // 
      this.tb_LogContent.BackColor = System.Drawing.SystemColors.Window;
      this.tb_LogContent.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tb_LogContent.Location = new System.Drawing.Point(0, 57);
      this.tb_LogContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_LogContent.Multiline = true;
      this.tb_LogContent.Name = "tb_LogContent";
      this.tb_LogContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.tb_LogContent.Size = new System.Drawing.Size(1810, 346);
      this.tb_LogContent.TabIndex = 0;
      // 
      // MS_LogConsole
      // 
      this.MS_LogConsole.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.MS_LogConsole.Location = new System.Drawing.Point(0, 33);
      this.MS_LogConsole.Name = "MS_LogConsole";
      this.MS_LogConsole.Size = new System.Drawing.Size(1810, 24);
      this.MS_LogConsole.TabIndex = 1;
      this.MS_LogConsole.Text = "TS_LogConsole";
      // 
      // TSMI_Level
      // 
      this.TSMI_Level.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Debug,
            this.TSMI_Info,
            this.TSMI_Warning,
            this.TSMI_Error,
            this.TSMI_Fatal});
      this.TSMI_Level.Name = "TSMI_Level";
      this.TSMI_Level.Size = new System.Drawing.Size(63, 29);
      this.TSMI_Level.Text = "Level";
      // 
      // TSMI_Debug
      // 
      this.TSMI_Debug.Name = "TSMI_Debug";
      this.TSMI_Debug.Size = new System.Drawing.Size(180, 34);
      this.TSMI_Debug.Tag = "Debug";
      this.TSMI_Debug.Text = "Debug";
      this.TSMI_Debug.Click += new System.EventHandler(this.TSMI_Loglevel_Click);
      // 
      // TSMI_Info
      // 
      this.TSMI_Info.Name = "TSMI_Info";
      this.TSMI_Info.Size = new System.Drawing.Size(180, 34);
      this.TSMI_Info.Tag = "Info";
      this.TSMI_Info.Text = "Info";
      this.TSMI_Info.Click += new System.EventHandler(this.TSMI_Loglevel_Click);
      // 
      // TSMI_Warning
      // 
      this.TSMI_Warning.Name = "TSMI_Warning";
      this.TSMI_Warning.Size = new System.Drawing.Size(180, 34);
      this.TSMI_Warning.Tag = "Warning";
      this.TSMI_Warning.Text = "Warning";
      this.TSMI_Warning.Click += new System.EventHandler(this.TSMI_Loglevel_Click);
      // 
      // TSMI_Error
      // 
      this.TSMI_Error.Name = "TSMI_Error";
      this.TSMI_Error.Size = new System.Drawing.Size(180, 34);
      this.TSMI_Error.Tag = "Error";
      this.TSMI_Error.Text = "Error";
      this.TSMI_Error.Click += new System.EventHandler(this.TSMI_Loglevel_Click);
      // 
      // TSMI_Fatal
      // 
      this.TSMI_Fatal.Name = "TSMI_Fatal";
      this.TSMI_Fatal.Size = new System.Drawing.Size(180, 34);
      this.TSMI_Fatal.Tag = "Fatal";
      this.TSMI_Fatal.Text = "Fatal";
      this.TSMI_Fatal.Click += new System.EventHandler(this.TSMI_Loglevel_Click);
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem});
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
      this.editToolStripMenuItem.Text = "Edit";
      // 
      // clearLogToolStripMenuItem
      // 
      this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
      this.clearLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
      this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(246, 34);
      this.clearLogToolStripMenuItem.Text = "Clear log";
      this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.TSMI_ClearLog_Click);
      // 
      // ms_LogMain
      // 
      this.ms_LogMain.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.ms_LogMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_File,
            this.TSMI_Edit});
      this.ms_LogMain.Location = new System.Drawing.Point(0, 0);
      this.ms_LogMain.Name = "ms_LogMain";
      this.ms_LogMain.Size = new System.Drawing.Size(1810, 33);
      this.ms_LogMain.TabIndex = 2;
      this.ms_LogMain.Text = "menuStrip1";
      // 
      // TSMI_File
      // 
      this.TSMI_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Close});
      this.TSMI_File.Name = "TSMI_File";
      this.TSMI_File.Size = new System.Drawing.Size(54, 29);
      this.TSMI_File.Text = "File";
      // 
      // TSMI_Close
      // 
      this.TSMI_Close.Name = "TSMI_Close";
      this.TSMI_Close.Size = new System.Drawing.Size(270, 34);
      this.TSMI_Close.Text = "Close";
      this.TSMI_Close.Click += new System.EventHandler(this.TSMI_Close_Click);
      // 
      // TSMI_Edit
      // 
      this.TSMI_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_ClearLog,
            this.TSMI_SetLogLevel});
      this.TSMI_Edit.Name = "TSMI_Edit";
      this.TSMI_Edit.Size = new System.Drawing.Size(58, 29);
      this.TSMI_Edit.Text = "Edit";
      // 
      // TSMI_ClearLog
      // 
      this.TSMI_ClearLog.Name = "TSMI_ClearLog";
      this.TSMI_ClearLog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
      this.TSMI_ClearLog.Size = new System.Drawing.Size(270, 34);
      this.TSMI_ClearLog.Text = "Clear log";
      this.TSMI_ClearLog.Click += new System.EventHandler(this.TSMI_ClearLog_Click);
      // 
      // TSMI_SetLogLevel
      // 
      this.TSMI_SetLogLevel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_LogLevel_Debug,
            this.TSMI_LogLevel_Info,
            this.TSMI_LogLevel_Warning,
            this.TSMI_LogLevel_Error});
      this.TSMI_SetLogLevel.Name = "TSMI_SetLogLevel";
      this.TSMI_SetLogLevel.Size = new System.Drawing.Size(270, 34);
      this.TSMI_SetLogLevel.Text = "Set log level";
      // 
      // TSMI_LogLevel_Debug
      // 
      this.TSMI_LogLevel_Debug.Name = "TSMI_LogLevel_Debug";
      this.TSMI_LogLevel_Debug.Size = new System.Drawing.Size(180, 34);
      this.TSMI_LogLevel_Debug.Text = "Debug";
      this.TSMI_LogLevel_Debug.Click += new System.EventHandler(this.TSMI_LogLevel_Debug_Click);
      // 
      // TSMI_LogLevel_Info
      // 
      this.TSMI_LogLevel_Info.Name = "TSMI_LogLevel_Info";
      this.TSMI_LogLevel_Info.Size = new System.Drawing.Size(180, 34);
      this.TSMI_LogLevel_Info.Text = "Info";
      this.TSMI_LogLevel_Info.Click += new System.EventHandler(this.TSMI_LogLevel_Info_Click);
      // 
      // TSMI_LogLevel_Warning
      // 
      this.TSMI_LogLevel_Warning.Name = "TSMI_LogLevel_Warning";
      this.TSMI_LogLevel_Warning.Size = new System.Drawing.Size(180, 34);
      this.TSMI_LogLevel_Warning.Text = "Warning";
      this.TSMI_LogLevel_Warning.Click += new System.EventHandler(this.TSMI_LogLevel_Warning_Click);
      // 
      // TSMI_LogLevel_Error
      // 
      this.TSMI_LogLevel_Error.Name = "TSMI_LogLevel_Error";
      this.TSMI_LogLevel_Error.Size = new System.Drawing.Size(180, 34);
      this.TSMI_LogLevel_Error.Text = "Error";
      this.TSMI_LogLevel_Error.Click += new System.EventHandler(this.TSMI_LogLevel_Error_Click);
      // 
      // LogCons
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1810, 403);
      this.Controls.Add(this.tb_LogContent);
      this.Controls.Add(this.MS_LogConsole);
      this.Controls.Add(this.ms_LogMain);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.MS_LogConsole;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "LogCons";
      this.Text = "Minary - LogConsole";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogConsole_FormClosing);
      this.ms_LogMain.ResumeLayout(false);
      this.ms_LogMain.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_LogContent;
    private System.Windows.Forms.MenuStrip MS_LogConsole;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Level;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Debug;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Info;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Warning;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Error;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Fatal;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
    private System.Windows.Forms.MenuStrip ms_LogMain;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Edit;
    private System.Windows.Forms.ToolStripMenuItem TSMI_ClearLog;
    private System.Windows.Forms.ToolStripMenuItem TSMI_SetLogLevel;
    private System.Windows.Forms.ToolStripMenuItem TSMI_LogLevel_Debug;
    private System.Windows.Forms.ToolStripMenuItem TSMI_LogLevel_Info;
    private System.Windows.Forms.ToolStripMenuItem TSMI_LogLevel_Warning;
    private System.Windows.Forms.ToolStripMenuItem TSMI_LogLevel_Error;
    private System.Windows.Forms.ToolStripMenuItem TSMI_File;
    private System.Windows.Forms.ToolStripMenuItem TSMI_Close;
  }
}