namespace Minary.Form.Updates
{
  public partial class FormNewVersion
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewVersion));
      this.bt_Close = new System.Windows.Forms.Button();
      this.p_UpdateMsg = new System.Windows.Forms.Panel();
      this.rtb_MinaryUpdate = new System.Windows.Forms.RichTextBox();
      this.ll_DownloadURL = new System.Windows.Forms.LinkLabel();
      this.CB_AutoUpdate = new System.Windows.Forms.CheckBox();
      this.p_UpdateMsg.SuspendLayout();
      this.SuspendLayout();
      // 
      // bt_Close
      // 
      this.bt_Close.Location = new System.Drawing.Point(706, 348);
      this.bt_Close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.bt_Close.Name = "bt_Close";
      this.bt_Close.Size = new System.Drawing.Size(112, 35);
      this.bt_Close.TabIndex = 0;
      this.bt_Close.Text = "Close";
      this.bt_Close.UseVisualStyleBackColor = true;
      this.bt_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // p_UpdateMsg
      // 
      this.p_UpdateMsg.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.p_UpdateMsg.Controls.Add(this.rtb_MinaryUpdate);
      this.p_UpdateMsg.Controls.Add(this.ll_DownloadURL);
      this.p_UpdateMsg.Location = new System.Drawing.Point(12, 12);
      this.p_UpdateMsg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.p_UpdateMsg.Name = "p_UpdateMsg";
      this.p_UpdateMsg.Size = new System.Drawing.Size(831, 326);
      this.p_UpdateMsg.TabIndex = 1;
      // 
      // rtb_MinaryUpdate
      // 
      this.rtb_MinaryUpdate.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.rtb_MinaryUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rtb_MinaryUpdate.Location = new System.Drawing.Point(14, 15);
      this.rtb_MinaryUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.rtb_MinaryUpdate.Name = "rtb_MinaryUpdate";
      this.rtb_MinaryUpdate.Size = new System.Drawing.Size(794, 275);
      this.rtb_MinaryUpdate.TabIndex = 1;
      this.rtb_MinaryUpdate.Text = "";
      // 
      // ll_DownloadURL
      // 
      this.ll_DownloadURL.AutoSize = true;
      this.ll_DownloadURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ll_DownloadURL.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
      this.ll_DownloadURL.Location = new System.Drawing.Point(15, 295);
      this.ll_DownloadURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ll_DownloadURL.Name = "ll_DownloadURL";
      this.ll_DownloadURL.Size = new System.Drawing.Size(155, 24);
      this.ll_DownloadURL.TabIndex = 0;
      this.ll_DownloadURL.TabStop = true;
      this.ll_DownloadURL.Tag = "";
      this.ll_DownloadURL.Text = "Minary web page";
      this.ll_DownloadURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_DownloadURL_LinkClicked);
      // 
      // CB_AutoUpdate
      // 
      this.CB_AutoUpdate.AutoSize = true;
      this.CB_AutoUpdate.Location = new System.Drawing.Point(26, 354);
      this.CB_AutoUpdate.Name = "CB_AutoUpdate";
      this.CB_AutoUpdate.Size = new System.Drawing.Size(261, 24);
      this.CB_AutoUpdate.TabIndex = 2;
      this.CB_AutoUpdate.Text = "Check for updates automatically";
      this.CB_AutoUpdate.UseVisualStyleBackColor = true;
      this.CB_AutoUpdate.CheckedChanged += new System.EventHandler(this.CB_StateChange);
      // 
      // FormNewVersion
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(861, 397);
      this.Controls.Add(this.CB_AutoUpdate);
      this.Controls.Add(this.p_UpdateMsg);
      this.Controls.Add(this.bt_Close);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "FormNewVersion";
      this.Text = "Update available ...";
      this.p_UpdateMsg.ResumeLayout(false);
      this.p_UpdateMsg.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button bt_Close;
    private System.Windows.Forms.Panel p_UpdateMsg;
    private System.Windows.Forms.LinkLabel ll_DownloadURL;
    private System.Windows.Forms.RichTextBox rtb_MinaryUpdate;
    private System.Windows.Forms.CheckBox CB_AutoUpdate;
  }
}