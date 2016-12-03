namespace Minary.Certificates.Presentation
{
  partial class ManageServerCertificates
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.bt_Close = new System.Windows.Forms.Button();
      this.dgv_ServerCertificates = new System.Windows.Forms.DataGridView();
      this.cms_ManageCertificates = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.cmsServerCertificates_Add = new System.Windows.Forms.ToolStripMenuItem();
      this.cmsServerCertificates_Delete = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_ServerCertificates)).BeginInit();
      this.cms_ManageCertificates.SuspendLayout();
      this.SuspendLayout();
      // 
      // bt_Close
      // 
      this.bt_Close.Location = new System.Drawing.Point(453, 329);
      this.bt_Close.Name = "bt_Close";
      this.bt_Close.Size = new System.Drawing.Size(65, 23);
      this.bt_Close.TabIndex = 2;
      this.bt_Close.Text = "Close";
      this.bt_Close.UseVisualStyleBackColor = true;
      this.bt_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // dgv_ServerCertificates
      // 
      this.dgv_ServerCertificates.AllowUserToAddRows = false;
      this.dgv_ServerCertificates.AllowUserToDeleteRows = false;
      this.dgv_ServerCertificates.AllowUserToResizeColumns = false;
      this.dgv_ServerCertificates.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv_ServerCertificates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgv_ServerCertificates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv_ServerCertificates.Location = new System.Drawing.Point(11, 12);
      this.dgv_ServerCertificates.MultiSelect = false;
      this.dgv_ServerCertificates.Name = "dgv_ServerCertificates";
      this.dgv_ServerCertificates.RowHeadersVisible = false;
      this.dgv_ServerCertificates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgv_ServerCertificates.Size = new System.Drawing.Size(533, 311);
      this.dgv_ServerCertificates.TabIndex = 1;
      this.dgv_ServerCertificates.DoubleClick += new System.EventHandler(this.DGV_ServerCertificates_DoubleClick);
      this.dgv_ServerCertificates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_ServerCertificates_MouseDown);
      this.dgv_ServerCertificates.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_ServerCertificates_MouseUp);
      // 
      // cms_ManageCertificates
      // 
      this.cms_ManageCertificates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsServerCertificates_Add,
            this.cmsServerCertificates_Delete});
      this.cms_ManageCertificates.Name = "cms_ManageCertificates";
      this.cms_ManageCertificates.Size = new System.Drawing.Size(121, 48);
      // 
      // cmsServerCertificates_Add
      // 
      this.cmsServerCertificates_Add.Name = "cmsServerCertificates_Add";
      this.cmsServerCertificates_Add.Size = new System.Drawing.Size(120, 22);
      this.cmsServerCertificates_Add.Text = "Create ...";
      this.cmsServerCertificates_Add.Click += new System.EventHandler(this.CmsServerCertificates_Add_Click);
      // 
      // cmsServerCertificates_Delete
      // 
      this.cmsServerCertificates_Delete.Name = "cmsServerCertificates_Delete";
      this.cmsServerCertificates_Delete.Size = new System.Drawing.Size(120, 22);
      this.cmsServerCertificates_Delete.Text = "Delete";
      this.cmsServerCertificates_Delete.Click += new System.EventHandler(this.CmsServerCertificates_Delete_Click);
      // 
      // ManageServerCertificates
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(556, 361);
      this.Controls.Add(this.dgv_ServerCertificates);
      this.Controls.Add(this.bt_Close);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ManageServerCertificates";
      this.Text = "Server certificates";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManageServerCertificates_FormClosing);
      ((System.ComponentModel.ISupportInitialize)(this.dgv_ServerCertificates)).EndInit();
      this.cms_ManageCertificates.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button bt_Close;
    private System.Windows.Forms.DataGridView dgv_ServerCertificates;
    private System.Windows.Forms.ContextMenuStrip cms_ManageCertificates;
    private System.Windows.Forms.ToolStripMenuItem cmsServerCertificates_Add;
    private System.Windows.Forms.ToolStripMenuItem cmsServerCertificates_Delete;
  }
}