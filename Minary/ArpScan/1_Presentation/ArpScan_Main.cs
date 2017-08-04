namespace Minary.ArpScan.Presentation
{
  using Minary.ArpScan.DataTypes;
  using Minary.MacVendors;
  using System;
  using System.ComponentModel;
  using System.Linq;
  using System.Windows.Forms;
  using System.Xml.Linq;


  public partial class ArpScan : Form
  {

    #region MEMBERS
    
    private BindingList<string> targetList;
    private string interfaceId;
    private string startIp;
    private string stopIp;
    private string gatewayIp;
    private string localIp;
    private MinaryMain minaryMain;
    private BindingList<TargetRecord> targetRecords;
    private Task.ArpScan arpScanTask;
    private bool isScanStarted;

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

      this.arpScanTask = new Task.ArpScan();
      this.isScanStarted = false;
      this.minaryMain = minaryMain;
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetList"></param>
    public void ShowArpScanGui(ref BindingList<string> targetList)
    {
      try
      {
        this.targetList = targetList;
        this.interfaceId = this.minaryMain.GetCurrentInterface();
        this.startIp = this.minaryMain.NetworkStartIp;
        this.stopIp = this.minaryMain.NetworkStopIp;
        this.gatewayIp = this.minaryMain.CurrentGatewayIp;
        this.localIp = this.minaryMain.CurrentLocalIp;

        this.tb_Subnet1.Text = this.startIp;
        this.tb_Subnet2.Text = this.stopIp;

        this.tb_Netrange1.Text = this.startIp;
        this.tb_Netrange2.Text = this.stopIp;

        this.rb_Subnet.Checked = true;
        this.Rb_Subnet_CheckedChanged(null, null);
        this.ShowDialog();
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpScan.ShowDialog(): {0}", ex.Message);
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
      this.isScanStarted = false;
      this.Cursor = Cursors.Default;
      this.dgv_Targets.Cursor = Cursors.Default;
      this.dgv_Targets.Refresh();

      // Stop ARP scan. First the regular, then the brute way.
      try
      {
        this.arpScanTask.StopArpScan();
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.StackTrace);
      }

      try
      {
        this.arpScanTask.KillAllRunningArpScans();
      }
      catch
      {
      }
    }


    /// <summary>
    ///
    /// </summary>
    private delegate void SetArpScanGuiOnStartedDelegate();
    private void SetArpScanGuiOnStarted()
    {
      string startIp = string.Empty;
      string stopIp = string.Empty;

      if (this.InvokeRequired)
      {
        this.BeginInvoke(new SetArpScanGuiOnStartedDelegate(this.SetArpScanGuiOnStarted), new object[] { });
        return;
      }

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
      this.isScanStarted = true;
      this.Cursor = Cursors.WaitCursor;
      this.dgv_Targets.Cursor = Cursors.WaitCursor;
    }


    /// <summary>
    ///
    /// </summary>
    private void ArpScanStopped()
    {
      this.SetArpScanGuiOnStopped();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="inputData"></param>
    public delegate void UpdateTextBoxDelegate(string inputData);
    public void UpdateTextBox(string inputData)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new UpdateTextBoxDelegate(this.UpdateTextBox), new object[] { inputData });
        return;
      }

      string type = string.Empty;
      string ipAddress = string.Empty;
      string macAddress = string.Empty;
      string vendor = string.Empty;

      try
      {
        XDocument xmlContent = XDocument.Parse(inputData);

        var packetEntries = from service in xmlContent.Descendants("arp")
                            select new
                            {
                              Type = service.Element("type").Value,
                              IP = service.Element("ip").Value,
                              MAC = service.Element("mac").Value
                            };

        if (packetEntries == null)
        {
          return;
        }

        foreach (var entry in packetEntries)
        {
          try
          {
            type = entry.Type;
            ipAddress = entry.IP;
            macAddress = entry.MAC;

            // Determine vendor
            vendor = this.minaryMain.MacVendor.GetVendorByMac(macAddress);

            if (ipAddress != this.gatewayIp && ipAddress != this.localIp)
            {
              this.targetList.Add(ipAddress);
              this.targetRecords.Add(new TargetRecord(ipAddress, macAddress, vendor));
            }
          }
          catch (Exception ex)
          {
            LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpScan: {0}", ex.Message);
          }
        }
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage(ex.StackTrace);
      }
    }

    #endregion

  }
}
