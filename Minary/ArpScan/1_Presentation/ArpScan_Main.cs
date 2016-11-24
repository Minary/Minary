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

    private static ArpScan arpScan;
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

    public ArpScan()
    {
      this.InitializeComponent();
      
      DataGridViewTextBoxColumn columnIp = new DataGridViewTextBoxColumn();
      columnIp.DataPropertyName = "IpAddress";
      columnIp.Name = "IpAddress";
      columnIp.HeaderText = "Ip address";
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
      columnVendor.MinimumWidth = 180;
      this.dgv_Targets.Columns.Add(columnVendor);

      DataGridViewCheckBoxColumn columnStatus = new DataGridViewCheckBoxColumn();
      columnStatus.DataPropertyName = "Status";
      columnStatus.Name = "Status";
      columnStatus.HeaderText = "Attack";
      columnStatus.Visible = true;
      columnStatus.Width = 72;
      this.dgv_Targets.Columns.Add(columnStatus);

      DataGridViewTextBoxColumn columnLastScanDate = new DataGridViewTextBoxColumn();
      columnLastScanDate.DataPropertyName = "LastScanDate";
      columnLastScanDate.Name = "LastScanDate";
      columnLastScanDate.HeaderText = "Scan date";
      columnLastScanDate.ReadOnly = true;
      columnLastScanDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      columnLastScanDate.MinimumWidth = 200;
      this.dgv_Targets.Columns.Add(columnLastScanDate);

      DataGridViewTextBoxColumn columnNote = new DataGridViewTextBoxColumn();
      columnNote.DataPropertyName = "Note";
      columnNote.Name = "Note";
      columnNote.HeaderText = "Note";
      columnNote.ReadOnly = true;
      columnNote.Visible = false;
      columnNote.Width = 0;
      this.dgv_Targets.Columns.Add(columnNote);

      this.targetRecords = new BindingList<TargetRecord>();
      this.dgv_Targets.DataSource = this.targetRecords;
      this.dgv_Targets.CurrentCellDirtyStateChanged += new EventHandler(this.Dgv_CurrentCellDirtyStateChanged);
      this.dgv_Targets.CellValueChanged += new DataGridViewCellEventHandler(this.Dgv_CellValueChanged);
      this.dgv_Targets.CellClick += new DataGridViewCellEventHandler(this.Dgv_CellClick);

      this.arpScanTask = Task.ArpScan.GetInstance();
      this.isScanStarted = false;
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="minaryMain"></param>
    /// <param name="newTargetList"></param>
    /// <returns></returns>
    public static ArpScan GetInstance(MinaryMain minaryMain, ref BindingList<string> targetList)
    {
      if (arpScan == null)
      {
        arpScan = new ArpScan();
      }

      arpScan.ResetValues(minaryMain, ref targetList);

      return arpScan;
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static ArpScan GetInstance()
    {
      return arpScan;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="minaryMain"></param>
    /// <param name="newTargetList"></param>
    public static void InitArpScan(MinaryMain minaryMain, ref BindingList<string> targetList)
    {
      if (arpScan == null)
      {
        arpScan = GetInstance(minaryMain, ref targetList);
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="minaryMain"></param>
    /// <param name="interfaceId"></param>
    /// <param name="startIp"></param>
    /// <param name="stopIp"></param>
    /// <param name="gatewayIp"></param>
    /// <param name="targetList"></param>
    public static void ShowArpScanGui(MinaryMain minaryMain, string interfaceId, string startIp, string stopIp, string gatewayIp, ref BindingList<string> targetList)
    {
      try
      {
        GetInstance(minaryMain, ref targetList).ShowDialog();
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
    /// <param name="minaryMain"></param>
    /// <param name="targetList"></param>
    private void ResetValues(MinaryMain minaryMain, ref BindingList<string> targetList)
    {
      this.minaryMain = minaryMain;
      this.targetList = targetList;
      this.interfaceId = minaryMain.GetCurrentInterface();
      this.startIp = minaryMain.NetworkStartIp;
      this.stopIp = minaryMain.NetworkStopIp;
      this.gatewayIp = minaryMain.CurrentGatewayIp;
      this.localIp = minaryMain.CurrentLocalIp;

      this.tb_Subnet1.Text = this.startIp;
      this.tb_Subnet2.Text = this.stopIp;

      this.tb_Netrange1.Text = this.startIp;
      this.tb_Netrange2.Text = this.stopIp;

      this.rb_Subnet.Checked = true;
      this.Rb_Subnet_CheckedChanged(null, null);
    }


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

      this.minaryMain.GetLogConsole.LogMessage("SetArpScanGuiOnStopped!!");
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

      // Set the tool tips!
      foreach (DataGridViewRow tmpRow in this.dgv_Targets.Rows)
      {
        foreach (DataGridViewCell tmpCell in tmpRow.Cells)
        {
          try
          {
            tmpCell.ToolTipText = tmpRow.Cells["Note"].Value.ToString();
          }
          catch (Exception)
          {
          }
        }
      }

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
      string startIP = string.Empty;
      string stopIP = string.Empty;

      if (this.InvokeRequired)
      {
        this.BeginInvoke(new SetArpScanGuiOnStartedDelegate(this.SetArpScanGuiOnStarted), new object[] { });
        return;
      }

      // Check start/stop IpAddress addresses.
      if (this.rb_Subnet.Checked)
      {
        startIP = this.tb_Subnet1.Text;
        stopIP = this.tb_Subnet2.Text;
      }
      else
      {
        startIP = this.tb_Netrange1.Text;
        stopIP = this.tb_Netrange2.Text;
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
      string note = string.Empty;

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
            vendor = MacVendor.GetInstance().GetVendorByMac(macAddress);

            if (ipAddress != this.gatewayIp && ipAddress != this.localIp)
            {
              this.targetList.Add(ipAddress);

              // lSysDetails = string.Empty; // lTaskFacadeFingerprint.GetSystemDetails(macAddress);
              note = string.Empty; // lTaskFacadeFingerprint.GetFingerprintNote(macAddress);
              this.targetRecords.Add(new TargetRecord(ipAddress, macAddress, vendor, string.Empty, note));
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
