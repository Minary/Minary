namespace Minary.Form.ArpScan.Presentation
{
  using Minary.DataTypes.Enum;
  using Minary.Domain.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using PcapDotNet.Core;
  using System;
  using System.ComponentModel;
  using System.Windows.Forms;


  public partial class ArpScan : Form
  {

    #region MEMBERS

    private BindingList<string> targetList;
    private string interfaceId;
    private string startIp;
    private string stopIp;
    private string gatewayIp;
    private string localIp;
    private string localMac;
    private MinaryMain minaryMain;
    private BindingList<TargetRecord> targetRecords;
    private bool isStopped;
    private PacketCommunicator communicator;

    #endregion


    #region PROPERTIES

    public BindingList<TargetRecord> TargetList { get { return this.targetRecords; } set { } }

    #endregion


    #region PUBLIC

    public ArpScan(MinaryMain minaryMain)
    {
      this.InitializeComponent();

      DataGridViewCheckBoxColumn columnAttack = new DataGridViewCheckBoxColumn();
      columnAttack.DataPropertyName = "Attack";
      columnAttack.Name = "Attack";
      columnAttack.HeaderText = "Attack";
      columnAttack.Visible = true;
      columnAttack.Width = 72;
      this.dgv_Targets.Columns.Add(columnAttack);

      DataGridViewTextBoxColumn columnIp = new DataGridViewTextBoxColumn();
      columnIp.DataPropertyName = "IpAddress";
      columnIp.Name = "IpAddress";
      columnIp.HeaderText = "IP address";
      columnIp.ReadOnly = true;
      columnIp.MinimumWidth = 130;
      this.dgv_Targets.Columns.Add(columnIp);

      DataGridViewTextBoxColumn columnMac = new DataGridViewTextBoxColumn();
      columnMac.DataPropertyName = "MacAddress";
      columnMac.Name = "MacAddress";
      columnMac.HeaderText = "Mac address";
      columnMac.ReadOnly = true;
      columnMac.MinimumWidth = 150;
      this.dgv_Targets.Columns.Add(columnMac);

      DataGridViewTextBoxColumn columnVendor = new DataGridViewTextBoxColumn();
      columnVendor.DataPropertyName = "Vendor";
      columnVendor.Name = "Vendor";
      columnVendor.HeaderText = "Vendor";
      columnVendor.ReadOnly = true;
      columnVendor.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      columnVendor.MinimumWidth = 180;
      this.dgv_Targets.Columns.Add(columnVendor);

      this.targetRecords = new BindingList<TargetRecord>();
      this.dgv_Targets.DataSource = this.targetRecords;
      this.dgv_Targets.CurrentCellDirtyStateChanged += new EventHandler(this.Dgv_CurrentCellDirtyStateChanged);
      this.dgv_Targets.CellValueChanged += new DataGridViewCellEventHandler(this.Dgv_CellValueChanged);
      this.dgv_Targets.CellClick += new DataGridViewCellEventHandler(this.Dgv_CellClick);

      this.minaryMain = minaryMain;

      // Set the owner to keep this form in the foreground/topmost
      this.Owner = minaryMain;

      this.isStopped = false;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetList"></param>
    public delegate void ShowArpScanGuiDelegate(ref BindingList<string> targetList);
    public void ShowArpScanGui(ref BindingList<string> targetList)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowArpScanGuiDelegate(this.ShowArpScanGui), new object[] { targetList });
        return;
      }

      // Initialize GUI values
      try
      {
        this.targetList = targetList;
        this.interfaceId = this.minaryMain.GetCurrentInterface();
        this.startIp = this.minaryMain.NetworkStartIp;
        this.stopIp = this.minaryMain.NetworkStopIp;
        this.gatewayIp = this.minaryMain.CurrentGatewayIp;
        this.localIp = this.minaryMain.CurrentLocalIp;
        this.localMac = this.minaryMain.CurrentLocalMac;

        this.communicator = PcapHandler.Inst.OpenPcapDevice(this.interfaceId, 1);

        this.tb_Subnet1.Text = this.startIp;
        this.tb_Subnet2.Text = this.stopIp;

        this.tb_Netrange1.Text = this.startIp;
        this.tb_Netrange2.Text = this.stopIp;

        this.rb_Subnet.Checked = true;
        this.RB_Subnet_CheckedChanged(null, null);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "ArpScan.ShowDialog(): {0}", ex.Message);
      }

      // Start ARP packet listener BGW
      if (this.bgw_ArpScanListener.IsBusy == false)
      {
        this.bgw_ArpScanListener.RunWorkerAsync();
      }

      try
      {
        this.ShowDialog();
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "ArpScan.ShowDialog(): {0}", ex.Message);
      }
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    private delegate void SetArpScanGuiOnStoppedDelegate();
    private void SetArpScanGuiOnStopped()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new SetArpScanGuiOnStoppedDelegate(this.SetArpScanGuiOnStopped), new object[] { });
        return;
      }

      // Set GUI parameters
      this.dgv_Targets.Enabled = true;
      this.bt_Close.Enabled = true;
      this.rb_Netrange.Enabled = true;
      this.rb_Subnet.Enabled = true;

      if (this.rb_Netrange.Checked)
      {
        this.tb_Netrange1.ReadOnly = false;
        this.tb_Netrange2.ReadOnly = false;

        this.tb_Netrange1.Enabled = true;
        this.tb_Netrange2.Enabled = true;
      }

      this.bt_Scan.Text = "Start";
      this.Cursor = Cursors.Default;
      this.dgv_Targets.Cursor = Cursors.Default;
      this.dgv_Targets.Refresh();

      this.pb_ArpScan.MarqueeAnimationSpeed = 0;

      // Set cancellation/stopping status
      this.IsCancellationPending = true;
      this.isStopped = true;
    }


    /// <summary>
    ///
    /// </summary>
    private delegate void SetArpScanGuiOnStartedDelegate();
    private void SetArpScanGuiOnStarted()
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new SetArpScanGuiOnStartedDelegate(this.SetArpScanGuiOnStarted), new object[] { });
        return;
      }

      string startIp = string.Empty;
      string stopIp = string.Empty;

      // Set cancellation/stopping status
      this.IsCancellationPending = false;
      this.isStopped = false;

      // Check start/stop IpAddress addresses.
      if (this.rb_Subnet.Checked)
      {
        startIp = this.tb_Subnet1.Text;
        stopIp = this.tb_Subnet2.Text;
      }
      else
      {
        startIp = this.tb_Netrange1.Text;
        stopIp = this.tb_Netrange2.Text;
      }

      this.targetList.Clear();
      this.pb_ArpScan.Minimum = 0;
      this.pb_ArpScan.Value = 0;
      this.pb_ArpScan.Maximum = 100;
      this.pb_ArpScan.MarqueeAnimationSpeed = 30;

      // Set GUI parameters
      this.dgv_Targets.Enabled = false;
      this.bt_Close.Enabled = false;
      this.rb_Netrange.Enabled = false;
      this.rb_Subnet.Enabled = false;

      if (this.rb_Netrange.Checked)
      {
        this.tb_Netrange1.ReadOnly = true;
        this.tb_Netrange2.ReadOnly = true;

        this.tb_Netrange1.Enabled = false;
        this.tb_Netrange2.Enabled = false;
      }

      this.bt_Scan.Text = "Stop";
      this.Cursor = Cursors.WaitCursor;
      this.dgv_Targets.Cursor = Cursors.WaitCursor;
    }

    #endregion

  }
}
