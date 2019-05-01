namespace Minary.Form.SimpleGUI.Presentation
{
  using Minary.Domain.MacVendor;
  using Minary.DataTypes.ArpScan;
  using Minary.Form.Main;
  using PcapDotNet.Core;
  using System.ComponentModel;
  using System.Windows.Forms;


  public partial class SimpleGuiUserControl : UserControl
  {

    #region MEMBERS

    private MinaryMain minaryObj;
    private PacketCommunicator communicator;
    private BindingList<SystemFoundSimple> targetStringList = new BindingList<SystemFoundSimple>();
    private MacVendorHandler macVendorHandler = new MacVendorHandler();

    #endregion


    #region PUBLIC

    public SimpleGuiUserControl(MinaryMain minaryObj)
    {
      InitializeComponent();

      // Params
      this.minaryObj = minaryObj;

      // DataGridView columns declaration
      var columnIpAddress = new DataGridViewTextBoxColumn();
      columnIpAddress.DataPropertyName = "IpAddress";
      columnIpAddress.Name = "IpAddress";
      columnIpAddress.HeaderText = "IP Adresse";
      columnIpAddress.Visible = true;
      columnIpAddress.ReadOnly = true;
      columnIpAddress.Width = 200;
      this.dgv_SimpleGui.Columns.Add(columnIpAddress);

      var columnMacAddress = new DataGridViewImageColumn();
      columnMacAddress.DataPropertyName = "MacAddress";
      columnMacAddress.Name = "MacAddress";
      columnMacAddress.HeaderText = "MacAddress";
      columnMacAddress.Visible = false;
      columnMacAddress.ReadOnly = true;
      columnMacAddress.Width = 200;
      this.dgv_SimpleGui.Columns.Add(columnMacAddress);

      var columnAttack = new DataGridViewCheckBoxColumn();
      columnAttack.DataPropertyName = "Attack";
      columnAttack.Name = "Attack";
      columnAttack.HeaderText = "Attack";
      columnAttack.Visible = true;
      columnAttack.Width = 100;
      this.dgv_SimpleGui.Columns.Add(columnAttack);

      var columnLastSeen = new DataGridViewTextBoxColumn();
      columnLastSeen.DataPropertyName = "LastSeen";
      columnLastSeen.Name = "LastSeen";
      columnLastSeen.HeaderText = "Last seen";
      columnLastSeen.Visible = true;
      columnLastSeen.ReadOnly = true;
      columnLastSeen.Width = 100;
      columnLastSeen.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      columnLastSeen.DefaultCellStyle.Format = "yyyy.MM.dd HH:mm:ss";
      this.dgv_SimpleGui.Columns.Add(columnLastSeen);

      // Initialize DGV data source list
      this.dgv_SimpleGui.DataSource = this.targetStringList;
    }

    #endregion

  }
}
