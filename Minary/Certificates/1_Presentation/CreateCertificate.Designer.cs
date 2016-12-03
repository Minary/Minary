namespace Minary.Certificates.Presentation
{
  partial class CreateCertificate
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
      this.bt_Add = new System.Windows.Forms.Button();
      this.bt_Close = new System.Windows.Forms.Button();
      this.gb_CreateCertificate = new System.Windows.Forms.GroupBox();
      this.tb_HostName = new System.Windows.Forms.TextBox();
      this.l_HostName = new System.Windows.Forms.Label();
      this.l_ValidityEndDate = new System.Windows.Forms.Label();
      this.l_ValidityStartDate = new System.Windows.Forms.Label();
      this.dtp_ExpirationDate = new System.Windows.Forms.DateTimePicker();
      this.dtp_BeginDate = new System.Windows.Forms.DateTimePicker();
      this.gb_CreateCertificate.SuspendLayout();
      this.SuspendLayout();
      // 
      // bt_Add
      // 
      this.bt_Add.Location = new System.Drawing.Point(226, 158);
      this.bt_Add.Name = "bt_Add";
      this.bt_Add.Size = new System.Drawing.Size(75, 23);
      this.bt_Add.TabIndex = 4;
      this.bt_Add.Text = "Add";
      this.bt_Add.UseVisualStyleBackColor = true;
      this.bt_Add.Click += new System.EventHandler(this.BT_Add_Click);
      // 
      // bt_Close
      // 
      this.bt_Close.Location = new System.Drawing.Point(330, 158);
      this.bt_Close.Name = "bt_Close";
      this.bt_Close.Size = new System.Drawing.Size(75, 23);
      this.bt_Close.TabIndex = 5;
      this.bt_Close.Text = "Close";
      this.bt_Close.UseVisualStyleBackColor = true;
      this.bt_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // gb_CreateCertificate
      // 
      this.gb_CreateCertificate.Controls.Add(this.tb_HostName);
      this.gb_CreateCertificate.Controls.Add(this.l_HostName);
      this.gb_CreateCertificate.Controls.Add(this.l_ValidityEndDate);
      this.gb_CreateCertificate.Controls.Add(this.l_ValidityStartDate);
      this.gb_CreateCertificate.Controls.Add(this.dtp_ExpirationDate);
      this.gb_CreateCertificate.Controls.Add(this.dtp_BeginDate);
      this.gb_CreateCertificate.Location = new System.Drawing.Point(13, 13);
      this.gb_CreateCertificate.Name = "gb_CreateCertificate";
      this.gb_CreateCertificate.Size = new System.Drawing.Size(410, 136);
      this.gb_CreateCertificate.TabIndex = 0;
      this.gb_CreateCertificate.TabStop = false;
      // 
      // tb_HostName
      // 
      this.tb_HostName.Location = new System.Drawing.Point(137, 28);
      this.tb_HostName.Name = "tb_HostName";
      this.tb_HostName.Size = new System.Drawing.Size(255, 20);
      this.tb_HostName.TabIndex = 1;
      // 
      // l_HostName
      // 
      this.l_HostName.AutoSize = true;
      this.l_HostName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_HostName.Location = new System.Drawing.Point(16, 28);
      this.l_HostName.Name = "l_HostName";
      this.l_HostName.Size = new System.Drawing.Size(67, 13);
      this.l_HostName.TabIndex = 0;
      this.l_HostName.Text = "Host name";
      // 
      // l_ValidityEndDate
      // 
      this.l_ValidityEndDate.AutoSize = true;
      this.l_ValidityEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_ValidityEndDate.Location = new System.Drawing.Point(16, 102);
      this.l_ValidityEndDate.Name = "l_ValidityEndDate";
      this.l_ValidityEndDate.Size = new System.Drawing.Size(102, 13);
      this.l_ValidityEndDate.TabIndex = 0;
      this.l_ValidityEndDate.Text = "Validity end date";
      // 
      // l_ValidityStartDate
      // 
      this.l_ValidityStartDate.AutoSize = true;
      this.l_ValidityStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_ValidityStartDate.Location = new System.Drawing.Point(16, 65);
      this.l_ValidityStartDate.Name = "l_ValidityStartDate";
      this.l_ValidityStartDate.Size = new System.Drawing.Size(106, 13);
      this.l_ValidityStartDate.TabIndex = 0;
      this.l_ValidityStartDate.Text = "Validity start date";
      // 
      // dtp_ExpirationDate
      // 
      this.dtp_ExpirationDate.Location = new System.Drawing.Point(137, 98);
      this.dtp_ExpirationDate.Name = "dtp_ExpirationDate";
      this.dtp_ExpirationDate.Size = new System.Drawing.Size(255, 20);
      this.dtp_ExpirationDate.TabIndex = 3;
      // 
      // dtp_BeginDate
      // 
      this.dtp_BeginDate.Location = new System.Drawing.Point(137, 62);
      this.dtp_BeginDate.Name = "dtp_BeginDate";
      this.dtp_BeginDate.Size = new System.Drawing.Size(255, 20);
      this.dtp_BeginDate.TabIndex = 2;
      // 
      // CreateCertificate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(438, 199);
      this.Controls.Add(this.gb_CreateCertificate);
      this.Controls.Add(this.bt_Close);
      this.Controls.Add(this.bt_Add);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "CreateCertificate";
      this.Text = "Create new certificate ...";
      this.gb_CreateCertificate.ResumeLayout(false);
      this.gb_CreateCertificate.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button bt_Add;
    private System.Windows.Forms.Button bt_Close;
    private System.Windows.Forms.GroupBox gb_CreateCertificate;
    private System.Windows.Forms.Label l_HostName;
    private System.Windows.Forms.Label l_ValidityEndDate;
    private System.Windows.Forms.Label l_ValidityStartDate;
    private System.Windows.Forms.DateTimePicker dtp_ExpirationDate;
    private System.Windows.Forms.DateTimePicker dtp_BeginDate;
    private System.Windows.Forms.TextBox tb_HostName;
  }
}