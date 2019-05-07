namespace Minary.Form.Template.Presentation
{
  public partial class LoadTemplate
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadTemplate));
      this.bt_Close = new System.Windows.Forms.Button();
      this.gb_TemplateLoadingProgress = new System.Windows.Forms.GroupBox();
      this.rtb_Logs = new System.Windows.Forms.RichTextBox();
      this.bgw_LoadTemplate = new System.ComponentModel.BackgroundWorker();
      this.gb_TemplateLoadingProgress.SuspendLayout();
      this.SuspendLayout();
      // 
      // bt_Close
      // 
      this.bt_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.bt_Close.Location = new System.Drawing.Point(958, 354);
      this.bt_Close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.bt_Close.Name = "bt_Close";
      this.bt_Close.Size = new System.Drawing.Size(112, 35);
      this.bt_Close.TabIndex = 1;
      this.bt_Close.Text = "Close";
      this.bt_Close.UseVisualStyleBackColor = true;
      this.bt_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // gb_TemplateLoadingProgress
      // 
      this.gb_TemplateLoadingProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gb_TemplateLoadingProgress.Controls.Add(this.rtb_Logs);
      this.gb_TemplateLoadingProgress.Location = new System.Drawing.Point(18, 12);
      this.gb_TemplateLoadingProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.gb_TemplateLoadingProgress.Name = "gb_TemplateLoadingProgress";
      this.gb_TemplateLoadingProgress.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.gb_TemplateLoadingProgress.Size = new System.Drawing.Size(1098, 326);
      this.gb_TemplateLoadingProgress.TabIndex = 2;
      this.gb_TemplateLoadingProgress.TabStop = false;
      // 
      // rtb_Logs
      // 
      this.rtb_Logs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.rtb_Logs.BackColor = System.Drawing.SystemColors.Control;
      this.rtb_Logs.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.rtb_Logs.Location = new System.Drawing.Point(9, 22);
      this.rtb_Logs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.rtb_Logs.Name = "rtb_Logs";
      this.rtb_Logs.ReadOnly = true;
      this.rtb_Logs.Size = new System.Drawing.Size(1080, 295);
      this.rtb_Logs.TabIndex = 3;
      this.rtb_Logs.Text = "";
      // 
      // bgw_LoadTemplate
      // 
      this.bgw_LoadTemplate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_LoadTemplateDoWork);
      this.bgw_LoadTemplate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGW_LoadTemplateRunWorkerCompleted);
      // 
      // LoadTemplate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1134, 409);
      this.Controls.Add(this.gb_TemplateLoadingProgress);
      this.Controls.Add(this.bt_Close);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "LoadTemplate";
      this.Text = "Load and execute template";
      this.Shown += new System.EventHandler(this.LoadTemplate_Shown);
      this.gb_TemplateLoadingProgress.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button bt_Close;
    private System.Windows.Forms.GroupBox gb_TemplateLoadingProgress;
    private System.ComponentModel.BackgroundWorker bgw_LoadTemplate;
    private System.Windows.Forms.RichTextBox rtb_Logs;
  }
}