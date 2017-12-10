namespace Minary.Form.Updates.Presentation
{


  public partial class FormCheckNewVersion
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCheckNewVersion));
      this.bt_Close = new System.Windows.Forms.Button();
      this.p_UpdateMsg = new System.Windows.Forms.Panel();
      this.PB_MinaryLogo = new System.Windows.Forms.PictureBox();
      this.rtb_MinaryUpdate = new System.Windows.Forms.RichTextBox();
      this.CB_AutoUpdate = new System.Windows.Forms.CheckBox();
      this.p_UpdateMsg.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PB_MinaryLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // bt_Close
      // 
      this.bt_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.bt_Close.Location = new System.Drawing.Point(798, 308);
      this.bt_Close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.bt_Close.Name = "bt_Close";
      this.bt_Close.Size = new System.Drawing.Size(112, 35);
      this.bt_Close.TabIndex = 2;
      this.bt_Close.Text = "Close";
      this.bt_Close.UseVisualStyleBackColor = true;
      this.bt_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // p_UpdateMsg
      // 
      this.p_UpdateMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.p_UpdateMsg.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.p_UpdateMsg.Controls.Add(this.PB_MinaryLogo);
      this.p_UpdateMsg.Controls.Add(this.rtb_MinaryUpdate);
      this.p_UpdateMsg.Location = new System.Drawing.Point(12, 12);
      this.p_UpdateMsg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.p_UpdateMsg.Name = "p_UpdateMsg";
      this.p_UpdateMsg.Size = new System.Drawing.Size(923, 286);
      this.p_UpdateMsg.TabIndex = 0;
      // 
      // PB_MinaryLogo
      // 
      this.PB_MinaryLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.PB_MinaryLogo.Image = global::Minary.Properties.Resources.Minary_Logo_Small;
      this.PB_MinaryLogo.Location = new System.Drawing.Point(811, 12);
      this.PB_MinaryLogo.Name = "PB_MinaryLogo";
      this.PB_MinaryLogo.Size = new System.Drawing.Size(101, 98);
      this.PB_MinaryLogo.TabIndex = 2;
      this.PB_MinaryLogo.TabStop = false;
      // 
      // rtb_MinaryUpdate
      // 
      this.rtb_MinaryUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.rtb_MinaryUpdate.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.rtb_MinaryUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rtb_MinaryUpdate.Location = new System.Drawing.Point(14, 12);
      this.rtb_MinaryUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.rtb_MinaryUpdate.Name = "rtb_MinaryUpdate";
      this.rtb_MinaryUpdate.Size = new System.Drawing.Size(771, 251);
      this.rtb_MinaryUpdate.TabIndex = 0;
      this.rtb_MinaryUpdate.TabStop = false;
      this.rtb_MinaryUpdate.Text = "";
      this.rtb_MinaryUpdate.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RTB_MinaryUpdate_LinkClicked);
      // 
      // CB_AutoUpdate
      // 
      this.CB_AutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.CB_AutoUpdate.AutoSize = true;
      this.CB_AutoUpdate.Location = new System.Drawing.Point(26, 314);
      this.CB_AutoUpdate.Name = "CB_AutoUpdate";
      this.CB_AutoUpdate.Size = new System.Drawing.Size(237, 24);
      this.CB_AutoUpdate.TabIndex = 1;
      this.CB_AutoUpdate.Text = "Check for updates at startup";
      this.CB_AutoUpdate.UseVisualStyleBackColor = true;
      this.CB_AutoUpdate.CheckedChanged += new System.EventHandler(this.CB_StateChange);
      // 
      // FormNewVersion
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(946, 354);
      this.Controls.Add(this.CB_AutoUpdate);
      this.Controls.Add(this.p_UpdateMsg);
      this.Controls.Add(this.bt_Close);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormNewVersion";
      this.Text = "New version available ...";
      this.p_UpdateMsg.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PB_MinaryLogo)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button bt_Close;
    private System.Windows.Forms.Panel p_UpdateMsg;
    private System.Windows.Forms.RichTextBox rtb_MinaryUpdate;
    private System.Windows.Forms.CheckBox CB_AutoUpdate;
    private System.Windows.Forms.PictureBox PB_MinaryLogo;
  }
}