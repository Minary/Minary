namespace Minary.Form.SimpleGUI.Presentation
{
  using Minary.DataTypes.Enum;
  using Minary.Domain.ArpScan;
  using Minary.LogConsole.Main;
  using Minary.Form.Main;
  using PcapDotNet.Core;
  using System;
  using System.Windows.Forms;


  public partial class SimpleGuiUserControl : UserControl
  {

    #region MEMBERS

    private MinaryMain minaryObj;
    private PacketCommunicator communicator;

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
      
      var columnOs = new DataGridViewImageColumn();
      columnOs.DataPropertyName = "OperatingSystem";
      columnOs.Name = "OperatingSystem";
      columnOs.HeaderText = "OS";
      columnOs.Visible = true;
      columnOs.ReadOnly = true;
      columnOs.Width = 200;
      this.dgv_SimpleGui.Columns.Add(columnOs);

      var columnBlockNetwork = new DataGridViewCheckBoxColumn();
      columnBlockNetwork.DataPropertyName = "Block";
      columnBlockNetwork.Name = "Block";
      columnBlockNetwork.HeaderText = "Block";
      columnBlockNetwork.Visible = true;
      columnBlockNetwork.Width = 200;
      this.dgv_SimpleGui.Columns.Add(columnBlockNetwork);

      var columnBlockRedir = new DataGridViewTextBoxColumn();
      columnBlockRedir.DataPropertyName = "Redirect";
      columnBlockRedir.Name = "Redirect";
      columnBlockRedir.HeaderText = "Umleiten";
      columnBlockRedir.Visible = true;
      columnBlockRedir.Width = 200;
      columnBlockRedir.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgv_SimpleGui.Columns.Add(columnBlockRedir);      
    }

    #endregion

  }
}
