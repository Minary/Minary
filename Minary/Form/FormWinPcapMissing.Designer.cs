namespace Minary.Form
{
  partial class FormWinPcapMissing
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWinPcapMissing));
      this.PB_WinPcap = new System.Windows.Forms.PictureBox();
      this.RTB_Message = new System.Windows.Forms.RichTextBox();
      this.LL_WinPcapURL = new System.Windows.Forms.LinkLabel();
      this.P_MainPanel = new System.Windows.Forms.Panel();
      this.GB_WinPcap = new System.Windows.Forms.GroupBox();
      this.BT_Close = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.PB_WinPcap)).BeginInit();
      this.P_MainPanel.SuspendLayout();
      this.GB_WinPcap.SuspendLayout();
      this.SuspendLayout();
      // 
      // PB_WinPcap
      // 
      this.PB_WinPcap.Image = global::Minary.Properties.Resources.WinPcap_Logo;
      this.PB_WinPcap.Location = new System.Drawing.Point(17, 14);
      this.PB_WinPcap.Name = "PB_WinPcap";
      this.PB_WinPcap.Size = new System.Drawing.Size(449, 118);
      this.PB_WinPcap.TabIndex = 0;
      this.PB_WinPcap.TabStop = false;
      // 
      // RTB_Message
      // 
      this.RTB_Message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.RTB_Message.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.RTB_Message.Location = new System.Drawing.Point(17, 149);
      this.RTB_Message.Name = "RTB_Message";
      this.RTB_Message.Size = new System.Drawing.Size(468, 61);
      this.RTB_Message.TabIndex = 0;
      this.RTB_Message.Text = "";
      // 
      // LL_WinPcapURL
      // 
      this.LL_WinPcapURL.AutoSize = true;
      this.LL_WinPcapURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LL_WinPcapURL.Location = new System.Drawing.Point(13, 235);
      this.LL_WinPcapURL.Name = "LL_WinPcapURL";
      this.LL_WinPcapURL.Size = new System.Drawing.Size(146, 20);
      this.LL_WinPcapURL.TabIndex = 1;
      this.LL_WinPcapURL.TabStop = true;
      this.LL_WinPcapURL.Text = "LINK TO WINPCAP";
      this.LL_WinPcapURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_WinPcapURL_LinkClicked);
      // 
      // P_MainPanel
      // 
      this.P_MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.P_MainPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.P_MainPanel.Controls.Add(this.LL_WinPcapURL);
      this.P_MainPanel.Controls.Add(this.RTB_Message);
      this.P_MainPanel.Controls.Add(this.PB_WinPcap);
      this.P_MainPanel.Location = new System.Drawing.Point(7, 16);
      this.P_MainPanel.Name = "P_MainPanel";
      this.P_MainPanel.Size = new System.Drawing.Size(501, 294);
      this.P_MainPanel.TabIndex = 0;
      // 
      // GB_WinPcap
      // 
      this.GB_WinPcap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GB_WinPcap.Controls.Add(this.P_MainPanel);
      this.GB_WinPcap.Location = new System.Drawing.Point(12, 3);
      this.GB_WinPcap.Name = "GB_WinPcap";
      this.GB_WinPcap.Size = new System.Drawing.Size(515, 318);
      this.GB_WinPcap.TabIndex = 0;
      this.GB_WinPcap.TabStop = false;
      // 
      // BT_Close
      // 
      this.BT_Close.Location = new System.Drawing.Point(410, 332);
      this.BT_Close.Name = "BT_Close";
      this.BT_Close.Size = new System.Drawing.Size(75, 34);
      this.BT_Close.TabIndex = 1;
      this.BT_Close.Text = "Close";
      this.BT_Close.UseVisualStyleBackColor = true;
      this.BT_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // FormWinPcapMissing
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(544, 378);
      this.Controls.Add(this.BT_Close);
      this.Controls.Add(this.GB_WinPcap);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormWinPcapMissing";
      this.Text = "WinPcap is missing";
      ((System.ComponentModel.ISupportInitialize)(this.PB_WinPcap)).EndInit();
      this.P_MainPanel.ResumeLayout(false);
      this.P_MainPanel.PerformLayout();
      this.GB_WinPcap.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox PB_WinPcap;
    private System.Windows.Forms.RichTextBox RTB_Message;
    private System.Windows.Forms.LinkLabel LL_WinPcapURL;
    private System.Windows.Forms.Panel P_MainPanel;
    private System.Windows.Forms.GroupBox GB_WinPcap;
    private System.Windows.Forms.Button BT_Close;
  }
}