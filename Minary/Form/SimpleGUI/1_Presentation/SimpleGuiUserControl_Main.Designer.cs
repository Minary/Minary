namespace Minary.Form.SimpleGUI.Presentation
{
  public partial class SimpleGuiUserControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.dgv_SimpleGui = new System.Windows.Forms.DataGridView();
      this.bgw_ArpScanSender = new System.ComponentModel.BackgroundWorker();
      this.bgw_ArpScanListener = new System.ComponentModel.BackgroundWorker();
      this.bgw_RemoveInactiveSystems = new System.ComponentModel.BackgroundWorker();
      this.cms_TargetActions = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.openInMiniBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_SimpleGui)).BeginInit();
      this.cms_TargetActions.SuspendLayout();
      this.SuspendLayout();
      // 
      // dgv_SimpleGui
      // 
      this.dgv_SimpleGui.AllowUserToAddRows = false;
      this.dgv_SimpleGui.AllowUserToDeleteRows = false;
      this.dgv_SimpleGui.AllowUserToResizeColumns = false;
      this.dgv_SimpleGui.AllowUserToResizeRows = false;
      this.dgv_SimpleGui.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgv_SimpleGui.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgv_SimpleGui.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv_SimpleGui.Location = new System.Drawing.Point(13, 17);
      this.dgv_SimpleGui.Margin = new System.Windows.Forms.Padding(10);
      this.dgv_SimpleGui.Name = "dgv_SimpleGui";
      this.dgv_SimpleGui.RowHeadersVisible = false;
      this.dgv_SimpleGui.RowTemplate.Height = 28;
      this.dgv_SimpleGui.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgv_SimpleGui.Size = new System.Drawing.Size(1343, 761);
      this.dgv_SimpleGui.TabIndex = 0;
      this.dgv_SimpleGui.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSimpleGui_CellContentClick);
      this.dgv_SimpleGui.DoubleClick += new System.EventHandler(this.DGV_SimpleGui_DoubleClick);
      this.dgv_SimpleGui.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DGV_SimpleGui_MouseDown);
      this.dgv_SimpleGui.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DGV_SimpleGui_MouseUp);
      // 
      // bgw_ArpScanSender
      // 
      this.bgw_ArpScanSender.WorkerSupportsCancellation = true;
      this.bgw_ArpScanSender.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_ArpScanSender_DoWork);
      this.bgw_ArpScanSender.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGW_ArpScanSender_RunWorkerCompleted);
      // 
      // bgw_ArpScanListener
      // 
      this.bgw_ArpScanListener.WorkerSupportsCancellation = true;
      this.bgw_ArpScanListener.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_ArpScanListener_DoWork);
      this.bgw_ArpScanListener.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGW_ArpScanListener_RunWorkerCompleted);
      // 
      // bgw_RemoveInactiveSystems
      // 
      this.bgw_RemoveInactiveSystems.WorkerSupportsCancellation = true;
      this.bgw_RemoveInactiveSystems.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_RemoveInactiveSystems_DoWork);
      this.bgw_RemoveInactiveSystems.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGW_RemoveInactiveSystems_RunWorkerCompleted);
      // 
      // cms_TargetActions
      // 
      this.cms_TargetActions.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.cms_TargetActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInMiniBrowserToolStripMenuItem});
      this.cms_TargetActions.Name = "cms_TargetActions";
      this.cms_TargetActions.Size = new System.Drawing.Size(256, 67);
      // 
      // openInMiniBrowserToolStripMenuItem
      // 
      this.openInMiniBrowserToolStripMenuItem.Name = "openInMiniBrowserToolStripMenuItem";
      this.openInMiniBrowserToolStripMenuItem.Size = new System.Drawing.Size(255, 30);
      this.openInMiniBrowserToolStripMenuItem.Text = "Open in Mini browser";
      this.openInMiniBrowserToolStripMenuItem.Click += new System.EventHandler(this.TSMI_OpenInMiniBrowser_Click);
      // 
      // SimpleGuiUserControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.dgv_SimpleGui);
      this.Name = "SimpleGuiUserControl";
      this.Size = new System.Drawing.Size(1366, 830);
      this.VisibleChanged += new System.EventHandler(this.SimpleGuiUserControl_VisibleChanged);
      ((System.ComponentModel.ISupportInitialize)(this.dgv_SimpleGui)).EndInit();
      this.cms_TargetActions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dgv_SimpleGui;
    private System.ComponentModel.BackgroundWorker bgw_ArpScanSender;
    private System.ComponentModel.BackgroundWorker bgw_ArpScanListener;
    private System.ComponentModel.BackgroundWorker bgw_RemoveInactiveSystems;
    private System.Windows.Forms.ContextMenuStrip cms_TargetActions;
    private System.Windows.Forms.ToolStripMenuItem openInMiniBrowserToolStripMenuItem;
  }
}
