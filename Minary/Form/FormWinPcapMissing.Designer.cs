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
      this.P_MainPanel = new System.Windows.Forms.Panel();
      this.PB_WinPcap = new System.Windows.Forms.PictureBox();
      this.RTB_Message = new System.Windows.Forms.RichTextBox();
      this.LL_WinPcapURL = new System.Windows.Forms.LinkLabel();
      this.P_MainPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PB_WinPcap)).BeginInit();
      this.SuspendLayout();
      // 
      // P_MainPanel
      // 
      this.P_MainPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.P_MainPanel.Controls.Add(this.LL_WinPcapURL);
      this.P_MainPanel.Controls.Add(this.RTB_Message);
      this.P_MainPanel.Controls.Add(this.PB_WinPcap);
      this.P_MainPanel.Location = new System.Drawing.Point(12, 12);
      this.P_MainPanel.Name = "P_MainPanel";
      this.P_MainPanel.Size = new System.Drawing.Size(479, 283);
      this.P_MainPanel.TabIndex = 0;
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
      this.RTB_Message.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.RTB_Message.Location = new System.Drawing.Point(17, 149);
      this.RTB_Message.Name = "RTB_Message";
      this.RTB_Message.Size = new System.Drawing.Size(449, 61);
      this.RTB_Message.TabIndex = 0;
      this.RTB_Message.Text = "";
      // 
      // LL_WinPcapURL
      // 
      this.LL_WinPcapURL.AutoSize = true;
      this.LL_WinPcapURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LL_WinPcapURL.Location = new System.Drawing.Point(46, 234);
      this.LL_WinPcapURL.Name = "LL_WinPcapURL";
      this.LL_WinPcapURL.Size = new System.Drawing.Size(146, 20);
      this.LL_WinPcapURL.TabIndex = 1;
      this.LL_WinPcapURL.TabStop = true;
      this.LL_WinPcapURL.Text = "LINK TO WINPCAP";
      this.LL_WinPcapURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_WinPcapURL_LinkClicked);
      // 
      // FormWinPcapMissing
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(504, 307);
      this.Controls.Add(this.P_MainPanel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormWinPcapMissing";
      this.Text = "WinPcap is missing";
      this.P_MainPanel.ResumeLayout(false);
      this.P_MainPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PB_WinPcap)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel P_MainPanel;
    private System.Windows.Forms.PictureBox PB_WinPcap;
    private System.Windows.Forms.RichTextBox RTB_Message;
    private System.Windows.Forms.LinkLabel LL_WinPcapURL;
  }
}