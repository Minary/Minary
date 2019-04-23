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
      this.dgv_SimpleGui = new System.Windows.Forms.DataGridView();
      this.bgw_ArpScanSender = new System.ComponentModel.BackgroundWorker();
      this.bgw_ArpScanListener = new System.ComponentModel.BackgroundWorker();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_SimpleGui)).BeginInit();
      this.SuspendLayout();
      // 
      // dgv_SimpleGui
      // 
      this.dgv_SimpleGui.AllowUserToAddRows = false;
      this.dgv_SimpleGui.AllowUserToDeleteRows = false;
      this.dgv_SimpleGui.AllowUserToResizeRows = false;
      this.dgv_SimpleGui.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv_SimpleGui.Location = new System.Drawing.Point(13, 17);
      this.dgv_SimpleGui.Margin = new System.Windows.Forms.Padding(10);
      this.dgv_SimpleGui.Name = "dgv_SimpleGui";
      this.dgv_SimpleGui.RowHeadersVisible = false;
      this.dgv_SimpleGui.RowTemplate.Height = 28;
      this.dgv_SimpleGui.Size = new System.Drawing.Size(1040, 764);
      this.dgv_SimpleGui.TabIndex = 0;
      this.dgv_SimpleGui.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_SimpleGui_CellContentClick);
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
      // SimpleGuiUserControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.dgv_SimpleGui);
      this.Name = "SimpleGuiUserControl";
      this.Size = new System.Drawing.Size(1366, 830);
      this.VisibleChanged += new System.EventHandler(this.SimpleGuiUserControl_VisibleChanged);
      ((System.ComponentModel.ISupportInitialize)(this.dgv_SimpleGui)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dgv_SimpleGui;
    private System.ComponentModel.BackgroundWorker bgw_ArpScanSender;
    private System.ComponentModel.BackgroundWorker bgw_ArpScanListener;
  }
}
