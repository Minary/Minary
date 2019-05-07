namespace Minary.Form
{
  partial class FormNPcapMissing
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNPcapMissing));
      this.PB_NPcap = new System.Windows.Forms.PictureBox();
      this.RTB_Message = new System.Windows.Forms.RichTextBox();
      this.LL_NPcapURL = new System.Windows.Forms.LinkLabel();
      this.P_MainPanel = new System.Windows.Forms.Panel();
      this.GB_NPcap = new System.Windows.Forms.GroupBox();
      this.BT_Close = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.PB_NPcap)).BeginInit();
      this.P_MainPanel.SuspendLayout();
      this.GB_NPcap.SuspendLayout();
      this.SuspendLayout();
      // 
      // PB_NPcap
      // 
      this.PB_NPcap.Location = new System.Drawing.Point(17, 14);
      this.PB_NPcap.Name = "PB_NPcap";
      this.PB_NPcap.Size = new System.Drawing.Size(449, 118);
      this.PB_NPcap.TabIndex = 0;
      this.PB_NPcap.TabStop = false;
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
      // LL_NPcapURL
      // 
      this.LL_NPcapURL.AutoSize = true;
      this.LL_NPcapURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LL_NPcapURL.Location = new System.Drawing.Point(13, 235);
      this.LL_NPcapURL.Name = "LL_NPcapURL";
      this.LL_NPcapURL.Size = new System.Drawing.Size(126, 20);
      this.LL_NPcapURL.TabIndex = 1;
      this.LL_NPcapURL.TabStop = true;
      this.LL_NPcapURL.Text = "LINK TO NPCAP";
      this.LL_NPcapURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_NPcapURL_LinkClicked);
      // 
      // P_MainPanel
      // 
      this.P_MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.P_MainPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.P_MainPanel.Controls.Add(this.LL_NPcapURL);
      this.P_MainPanel.Controls.Add(this.RTB_Message);
      this.P_MainPanel.Controls.Add(this.PB_NPcap);
      this.P_MainPanel.Location = new System.Drawing.Point(7, 16);
      this.P_MainPanel.Name = "P_MainPanel";
      this.P_MainPanel.Size = new System.Drawing.Size(501, 294);
      this.P_MainPanel.TabIndex = 0;
      // 
      // GB_NPcap
      // 
      this.GB_NPcap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GB_NPcap.Controls.Add(this.P_MainPanel);
      this.GB_NPcap.Location = new System.Drawing.Point(12, 3);
      this.GB_NPcap.Name = "GB_NPcap";
      this.GB_NPcap.Size = new System.Drawing.Size(515, 318);
      this.GB_NPcap.TabIndex = 0;
      this.GB_NPcap.TabStop = false;
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
      // FormNPcapMissing
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(544, 378);
      this.Controls.Add(this.BT_Close);
      this.Controls.Add(this.GB_NPcap);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormNPcapMissing";
      this.Text = "NPcap is missing";
      ((System.ComponentModel.ISupportInitialize)(this.PB_NPcap)).EndInit();
      this.P_MainPanel.ResumeLayout(false);
      this.P_MainPanel.PerformLayout();
      this.GB_NPcap.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox PB_NPcap;
    private System.Windows.Forms.RichTextBox RTB_Message;
    private System.Windows.Forms.LinkLabel LL_NPcapURL;
    private System.Windows.Forms.Panel P_MainPanel;
    private System.Windows.Forms.GroupBox GB_NPcap;
    private System.Windows.Forms.Button BT_Close;
  }
}