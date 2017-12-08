namespace Minary.Form
{
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;


  public partial class MinaryMain
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        this.attackServiceHandler.ShutDown();
      }
      catch (System.Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, "MinaryMain.Dispose(Exception): {0}", ex.Message);
      }

      if (disposing && (components != null))
      {
        components.Dispose();
      }

      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinaryMain));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.gb_Interfaces = new System.Windows.Forms.GroupBox();
      this.tb_TemplateName = new System.Windows.Forms.TextBox();
      this.l_Template = new System.Windows.Forms.Label();
      this.l_NetrangeSeparator = new System.Windows.Forms.Label();
      this.cb_Interfaces = new System.Windows.Forms.ComboBox();
      this.tb_NetworkStopIp = new System.Windows.Forms.TextBox();
      this.tb_NetworkStartIp = new System.Windows.Forms.TextBox();
      this.lab_Interface = new System.Windows.Forms.Label();
      this.lab_StartIP = new System.Windows.Forms.Label();
      this.bt_Attack = new System.Windows.Forms.Button();
      this.bt_ScanLan = new System.Windows.Forms.Button();
      this.gb_TargetRange = new System.Windows.Forms.GroupBox();
      this.tb_GatewayMac = new System.Windows.Forms.TextBox();
      this.l_GatewayMAC = new System.Windows.Forms.Label();
      this.tb_Vendor = new System.Windows.Forms.TextBox();
      this.l_VendorTitle = new System.Windows.Forms.Label();
      this.tb_GatewayIp = new System.Windows.Forms.TextBox();
      this.l_GatewayIP = new System.Windows.Forms.Label();
      this.tc_Plugins = new System.Windows.Forms.TabControl();
      this.tp_MinaryPluginCatalog = new System.Windows.Forms.TabPage();
      this.dgv_MainPlugins = new System.Windows.Forms.DataGridView();
      this.il_PluginStat = new System.Windows.Forms.ImageList(this.components);
      this.ms_MainWindow = new System.Windows.Forms.MenuStrip();
      this.tsmi_File = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_Exit = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_ResetMinary = new System.Windows.Forms.ToolStripMenuItem();
      this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_Beep = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_Debugging = new System.Windows.Forms.ToolStripMenuItem();
      this.templateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_LoadTemplate = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_CreateTemplate = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_UnloadTemplate = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_Tools = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_Help = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_GetUpdates = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_LogConsole = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_Minibrowser = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmi_DetectInterfaces = new System.Windows.Forms.ToolStripMenuItem();
      this.ofd_ImportSession = new System.Windows.Forms.OpenFileDialog();
      this.l_AS_Proxy_Key = new System.Windows.Forms.Label();
      this.l_AS_APE_Key = new System.Windows.Forms.Label();
      this.l_AS_Sniffer_Key = new System.Windows.Forms.Label();
      this.pb_StatusProxy = new System.Windows.Forms.PictureBox();
      this.il_AttackServiceStat = new System.Windows.Forms.ImageList(this.components);
      this.pb_StatusArpPoison = new System.Windows.Forms.PictureBox();
      this.pb_StatusDataSniffer = new System.Windows.Forms.PictureBox();
      this.gb_Interfaces.SuspendLayout();
      this.gb_TargetRange.SuspendLayout();
      this.tc_Plugins.SuspendLayout();
      this.tp_MinaryPluginCatalog.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_MainPlugins)).BeginInit();
      this.ms_MainWindow.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pb_StatusProxy)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pb_StatusArpPoison)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pb_StatusDataSniffer)).BeginInit();
      this.SuspendLayout();
      // 
      // gb_Interfaces
      // 
      this.gb_Interfaces.Controls.Add(this.tb_TemplateName);
      this.gb_Interfaces.Controls.Add(this.l_Template);
      this.gb_Interfaces.Controls.Add(this.l_NetrangeSeparator);
      this.gb_Interfaces.Controls.Add(this.cb_Interfaces);
      this.gb_Interfaces.Controls.Add(this.tb_NetworkStopIp);
      this.gb_Interfaces.Controls.Add(this.tb_NetworkStartIp);
      this.gb_Interfaces.Controls.Add(this.lab_Interface);
      this.gb_Interfaces.Controls.Add(this.lab_StartIP);
      this.gb_Interfaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.gb_Interfaces.Location = new System.Drawing.Point(633, 35);
      this.gb_Interfaces.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.gb_Interfaces.Name = "gb_Interfaces";
      this.gb_Interfaces.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.gb_Interfaces.Size = new System.Drawing.Size(600, 162);
      this.gb_Interfaces.TabIndex = 2;
      this.gb_Interfaces.TabStop = false;
      // 
      // tb_TemplateName
      // 
      this.tb_TemplateName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tb_TemplateName.Location = new System.Drawing.Point(150, 111);
      this.tb_TemplateName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_TemplateName.Name = "tb_TemplateName";
      this.tb_TemplateName.ReadOnly = true;
      this.tb_TemplateName.Size = new System.Drawing.Size(415, 26);
      this.tb_TemplateName.TabIndex = 10;
      this.tb_TemplateName.TabStop = false;
      // 
      // l_Template
      // 
      this.l_Template.AutoSize = true;
      this.l_Template.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_Template.Location = new System.Drawing.Point(14, 115);
      this.l_Template.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_Template.Name = "l_Template";
      this.l_Template.Size = new System.Drawing.Size(86, 20);
      this.l_Template.TabIndex = 11;
      this.l_Template.Text = "Template";
      // 
      // l_NetrangeSeparator
      // 
      this.l_NetrangeSeparator.AutoSize = true;
      this.l_NetrangeSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_NetrangeSeparator.Location = new System.Drawing.Point(352, 75);
      this.l_NetrangeSeparator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_NetrangeSeparator.Name = "l_NetrangeSeparator";
      this.l_NetrangeSeparator.Size = new System.Drawing.Size(15, 20);
      this.l_NetrangeSeparator.TabIndex = 9;
      this.l_NetrangeSeparator.Text = "-";
      // 
      // cb_Interfaces
      // 
      this.cb_Interfaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cb_Interfaces.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cb_Interfaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cb_Interfaces.FormattingEnabled = true;
      this.cb_Interfaces.Location = new System.Drawing.Point(147, 26);
      this.cb_Interfaces.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cb_Interfaces.Name = "cb_Interfaces";
      this.cb_Interfaces.Size = new System.Drawing.Size(418, 28);
      this.cb_Interfaces.TabIndex = 3;
      this.cb_Interfaces.SelectedIndexChanged += new System.EventHandler(this.CB_Interfaces_SelectedIndexChanged);
      // 
      // tb_NetworkStopIp
      // 
      this.tb_NetworkStopIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tb_NetworkStopIp.Location = new System.Drawing.Point(376, 69);
      this.tb_NetworkStopIp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_NetworkStopIp.Name = "tb_NetworkStopIp";
      this.tb_NetworkStopIp.ReadOnly = true;
      this.tb_NetworkStopIp.Size = new System.Drawing.Size(188, 26);
      this.tb_NetworkStopIp.TabIndex = 0;
      this.tb_NetworkStopIp.TabStop = false;
      // 
      // tb_NetworkStartIp
      // 
      this.tb_NetworkStartIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tb_NetworkStartIp.Location = new System.Drawing.Point(150, 69);
      this.tb_NetworkStartIp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_NetworkStartIp.Name = "tb_NetworkStartIp";
      this.tb_NetworkStartIp.ReadOnly = true;
      this.tb_NetworkStartIp.Size = new System.Drawing.Size(192, 26);
      this.tb_NetworkStartIp.TabIndex = 0;
      this.tb_NetworkStartIp.TabStop = false;
      // 
      // lab_Interface
      // 
      this.lab_Interface.AutoSize = true;
      this.lab_Interface.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lab_Interface.Location = new System.Drawing.Point(14, 31);
      this.lab_Interface.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lab_Interface.Name = "lab_Interface";
      this.lab_Interface.Size = new System.Drawing.Size(83, 20);
      this.lab_Interface.TabIndex = 0;
      this.lab_Interface.Text = "Interface";
      // 
      // lab_StartIP
      // 
      this.lab_StartIP.AutoSize = true;
      this.lab_StartIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lab_StartIP.Location = new System.Drawing.Point(14, 74);
      this.lab_StartIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lab_StartIP.Name = "lab_StartIP";
      this.lab_StartIP.Size = new System.Drawing.Size(91, 20);
      this.lab_StartIP.TabIndex = 0;
      this.lab_StartIP.Text = "Net range";
      // 
      // bt_Attack
      // 
      this.bt_Attack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bt_Attack.BackgroundImage")));
      this.bt_Attack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.bt_Attack.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.bt_Attack.Location = new System.Drawing.Point(1456, 48);
      this.bt_Attack.Margin = new System.Windows.Forms.Padding(0);
      this.bt_Attack.Name = "bt_Attack";
      this.bt_Attack.Size = new System.Drawing.Size(184, 149);
      this.bt_Attack.TabIndex = 6;
      this.bt_Attack.UseVisualStyleBackColor = true;
      this.bt_Attack.Click += new System.EventHandler(this.BT_Attack_Click);
      // 
      // bt_ScanLan
      // 
      this.bt_ScanLan.BackgroundImage = global::Minary.Properties.Resources.FA_Scan;
      this.bt_ScanLan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.bt_ScanLan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.bt_ScanLan.Location = new System.Drawing.Point(1251, 48);
      this.bt_ScanLan.Margin = new System.Windows.Forms.Padding(0);
      this.bt_ScanLan.Name = "bt_ScanLan";
      this.bt_ScanLan.Size = new System.Drawing.Size(184, 149);
      this.bt_ScanLan.TabIndex = 5;
      this.bt_ScanLan.UseVisualStyleBackColor = true;
      this.bt_ScanLan.Click += new System.EventHandler(this.BT_ScanLan_Click);
      // 
      // gb_TargetRange
      // 
      this.gb_TargetRange.Controls.Add(this.tb_GatewayMac);
      this.gb_TargetRange.Controls.Add(this.l_GatewayMAC);
      this.gb_TargetRange.Controls.Add(this.tb_Vendor);
      this.gb_TargetRange.Controls.Add(this.l_VendorTitle);
      this.gb_TargetRange.Controls.Add(this.tb_GatewayIp);
      this.gb_TargetRange.Controls.Add(this.l_GatewayIP);
      this.gb_TargetRange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.gb_TargetRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.gb_TargetRange.Location = new System.Drawing.Point(18, 35);
      this.gb_TargetRange.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.gb_TargetRange.Name = "gb_TargetRange";
      this.gb_TargetRange.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.gb_TargetRange.Size = new System.Drawing.Size(549, 162);
      this.gb_TargetRange.TabIndex = 1;
      this.gb_TargetRange.TabStop = false;
      // 
      // tb_GatewayMac
      // 
      this.tb_GatewayMac.Location = new System.Drawing.Point(164, 69);
      this.tb_GatewayMac.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_GatewayMac.Name = "tb_GatewayMac";
      this.tb_GatewayMac.ReadOnly = true;
      this.tb_GatewayMac.Size = new System.Drawing.Size(358, 26);
      this.tb_GatewayMac.TabIndex = 13;
      this.tb_GatewayMac.TabStop = false;
      // 
      // l_GatewayMAC
      // 
      this.l_GatewayMAC.AutoSize = true;
      this.l_GatewayMAC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_GatewayMAC.Location = new System.Drawing.Point(21, 74);
      this.l_GatewayMAC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_GatewayMAC.Name = "l_GatewayMAC";
      this.l_GatewayMAC.Size = new System.Drawing.Size(127, 20);
      this.l_GatewayMAC.TabIndex = 12;
      this.l_GatewayMAC.Text = "Gateway MAC";
      // 
      // tb_Vendor
      // 
      this.tb_Vendor.Location = new System.Drawing.Point(164, 111);
      this.tb_Vendor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_Vendor.Name = "tb_Vendor";
      this.tb_Vendor.ReadOnly = true;
      this.tb_Vendor.Size = new System.Drawing.Size(358, 26);
      this.tb_Vendor.TabIndex = 11;
      this.tb_Vendor.TabStop = false;
      // 
      // l_VendorTitle
      // 
      this.l_VendorTitle.AutoSize = true;
      this.l_VendorTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_VendorTitle.Location = new System.Drawing.Point(21, 115);
      this.l_VendorTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_VendorTitle.Name = "l_VendorTitle";
      this.l_VendorTitle.Size = new System.Drawing.Size(68, 20);
      this.l_VendorTitle.TabIndex = 10;
      this.l_VendorTitle.Text = "Vendor";
      // 
      // tb_GatewayIp
      // 
      this.tb_GatewayIp.Location = new System.Drawing.Point(164, 28);
      this.tb_GatewayIp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tb_GatewayIp.Name = "tb_GatewayIp";
      this.tb_GatewayIp.ReadOnly = true;
      this.tb_GatewayIp.Size = new System.Drawing.Size(358, 26);
      this.tb_GatewayIp.TabIndex = 0;
      this.tb_GatewayIp.TabStop = false;
      // 
      // l_GatewayIP
      // 
      this.l_GatewayIP.AutoSize = true;
      this.l_GatewayIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_GatewayIP.Location = new System.Drawing.Point(21, 31);
      this.l_GatewayIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_GatewayIP.Name = "l_GatewayIP";
      this.l_GatewayIP.Size = new System.Drawing.Size(104, 20);
      this.l_GatewayIP.TabIndex = 0;
      this.l_GatewayIP.Text = "Gateway IP";
      // 
      // tc_Plugins
      // 
      this.tc_Plugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tc_Plugins.Controls.Add(this.tp_MinaryPluginCatalog);
      this.tc_Plugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tc_Plugins.ImageList = this.il_PluginStat;
      this.tc_Plugins.ItemSize = new System.Drawing.Size(79, 23);
      this.tc_Plugins.Location = new System.Drawing.Point(18, 218);
      this.tc_Plugins.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tc_Plugins.Multiline = true;
      this.tc_Plugins.Name = "tc_Plugins";
      this.tc_Plugins.SelectedIndex = 0;
      this.tc_Plugins.Size = new System.Drawing.Size(1629, 640);
      this.tc_Plugins.TabIndex = 7;
      // 
      // tp_MinaryPluginCatalog
      // 
      this.tp_MinaryPluginCatalog.BackColor = System.Drawing.Color.White;
      this.tp_MinaryPluginCatalog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.tp_MinaryPluginCatalog.Controls.Add(this.dgv_MainPlugins);
      this.tp_MinaryPluginCatalog.Location = new System.Drawing.Point(4, 27);
      this.tp_MinaryPluginCatalog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tp_MinaryPluginCatalog.Name = "tp_MinaryPluginCatalog";
      this.tp_MinaryPluginCatalog.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tp_MinaryPluginCatalog.Size = new System.Drawing.Size(1621, 609);
      this.tp_MinaryPluginCatalog.TabIndex = 0;
      this.tp_MinaryPluginCatalog.Text = "Minary";
      // 
      // dgv_MainPlugins
      // 
      this.dgv_MainPlugins.AllowUserToAddRows = false;
      this.dgv_MainPlugins.AllowUserToDeleteRows = false;
      this.dgv_MainPlugins.AllowUserToResizeColumns = false;
      this.dgv_MainPlugins.AllowUserToResizeRows = false;
      this.dgv_MainPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgv_MainPlugins.BackgroundColor = System.Drawing.Color.White;
      this.dgv_MainPlugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgv_MainPlugins.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
      this.dgv_MainPlugins.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv_MainPlugins.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgv_MainPlugins.ColumnHeadersHeight = 28;
      this.dgv_MainPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_MainPlugins.DefaultCellStyle = dataGridViewCellStyle2;
      this.dgv_MainPlugins.EnableHeadersVisualStyles = false;
      this.dgv_MainPlugins.GridColor = System.Drawing.Color.White;
      this.dgv_MainPlugins.Location = new System.Drawing.Point(69, 31);
      this.dgv_MainPlugins.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.dgv_MainPlugins.MultiSelect = false;
      this.dgv_MainPlugins.Name = "dgv_MainPlugins";
      this.dgv_MainPlugins.ReadOnly = true;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv_MainPlugins.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dgv_MainPlugins.RowHeadersVisible = false;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.dgv_MainPlugins.RowsDefaultCellStyle = dataGridViewCellStyle4;
      this.dgv_MainPlugins.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.dgv_MainPlugins.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.dgv_MainPlugins.RowTemplate.ReadOnly = true;
      this.dgv_MainPlugins.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_MainPlugins.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dgv_MainPlugins.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgv_MainPlugins.Size = new System.Drawing.Size(1515, 518);
      this.dgv_MainPlugins.TabIndex = 0;
      this.dgv_MainPlugins.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Dgv_MainPlugins_CellContentClick);
      this.dgv_MainPlugins.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_MainPlugins_DataError);
      // 
      // il_PluginStat
      // 
      this.il_PluginStat.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_PluginStat.ImageStream")));
      this.il_PluginStat.TransparentColor = System.Drawing.Color.Transparent;
      this.il_PluginStat.Images.SetKeyName(0, "fa_paus_plugin.png");
      this.il_PluginStat.Images.SetKeyName(1, "fa_play_plugin.png");
      this.il_PluginStat.Images.SetKeyName(2, "fa_stop_plugin.png");
      // 
      // ms_MainWindow
      // 
      this.ms_MainWindow.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.ms_MainWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_File,
            this.settingsToolStripMenuItem,
            this.templateToolStripMenuItem,
            this.tsmi_Tools,
            this.tsmi_Help});
      this.ms_MainWindow.Location = new System.Drawing.Point(0, 0);
      this.ms_MainWindow.Name = "ms_MainWindow";
      this.ms_MainWindow.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
      this.ms_MainWindow.Size = new System.Drawing.Size(1671, 35);
      this.ms_MainWindow.TabIndex = 0;
      this.ms_MainWindow.Text = "ms_MainGUI";
      // 
      // tsmi_File
      // 
      this.tsmi_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Exit,
            this.tsmi_ResetMinary});
      this.tsmi_File.Name = "tsmi_File";
      this.tsmi_File.Size = new System.Drawing.Size(50, 29);
      this.tsmi_File.Text = "File";
      // 
      // tsmi_Exit
      // 
      this.tsmi_Exit.Name = "tsmi_Exit";
      this.tsmi_Exit.Size = new System.Drawing.Size(197, 30);
      this.tsmi_Exit.Text = "Exit";
      this.tsmi_Exit.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
      // 
      // tsmi_ResetMinary
      // 
      this.tsmi_ResetMinary.Name = "tsmi_ResetMinary";
      this.tsmi_ResetMinary.Size = new System.Drawing.Size(197, 30);
      this.tsmi_ResetMinary.Text = "Reset Minary";
      this.tsmi_ResetMinary.Click += new System.EventHandler(this.TSMI_ResetAllPlugins_Click);
      // 
      // settingsToolStripMenuItem
      // 
      this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Beep,
            this.tsmi_Debugging});
      this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
      this.settingsToolStripMenuItem.Size = new System.Drawing.Size(88, 29);
      this.settingsToolStripMenuItem.Text = "Settings";
      // 
      // tsmi_Beep
      // 
      this.tsmi_Beep.Name = "tsmi_Beep";
      this.tsmi_Beep.Size = new System.Drawing.Size(305, 30);
      this.tsmi_Beep.Text = "Beep (is off)";
      this.tsmi_Beep.Click += new System.EventHandler(this.BeepToolStripMenuItem_Click);
      // 
      // tsmi_Debugging
      // 
      this.tsmi_Debugging.Name = "tsmi_Debugging";
      this.tsmi_Debugging.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
      this.tsmi_Debugging.Size = new System.Drawing.Size(305, 30);
      this.tsmi_Debugging.Text = "Debugging (is off)";
      this.tsmi_Debugging.Click += new System.EventHandler(this.DebugginOnToolStripMenuItem_Click);
      // 
      // templateToolStripMenuItem
      // 
      this.templateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_LoadTemplate,
            this.tsmi_CreateTemplate,
            this.tsmi_UnloadTemplate});
      this.templateToolStripMenuItem.Name = "templateToolStripMenuItem";
      this.templateToolStripMenuItem.Size = new System.Drawing.Size(95, 29);
      this.templateToolStripMenuItem.Text = "Template";
      // 
      // tsmi_LoadTemplate
      // 
      this.tsmi_LoadTemplate.Name = "tsmi_LoadTemplate";
      this.tsmi_LoadTemplate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
      this.tsmi_LoadTemplate.Size = new System.Drawing.Size(227, 30);
      this.tsmi_LoadTemplate.Text = "Load ...";
      this.tsmi_LoadTemplate.Click += new System.EventHandler(this.LoadTemplateToolStripMenuItem_Click);
      // 
      // tsmi_CreateTemplate
      // 
      this.tsmi_CreateTemplate.Name = "tsmi_CreateTemplate";
      this.tsmi_CreateTemplate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
      this.tsmi_CreateTemplate.Size = new System.Drawing.Size(227, 30);
      this.tsmi_CreateTemplate.Text = "Create ...";
      this.tsmi_CreateTemplate.Click += new System.EventHandler(this.CreateToolStripMenuItem_Click);
      // 
      // tsmi_UnloadTemplate
      // 
      this.tsmi_UnloadTemplate.Name = "tsmi_UnloadTemplate";
      this.tsmi_UnloadTemplate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
      this.tsmi_UnloadTemplate.Size = new System.Drawing.Size(227, 30);
      this.tsmi_UnloadTemplate.Text = "Unload ";
      this.tsmi_UnloadTemplate.Click += new System.EventHandler(this.UnloadTemplateToolStripMenuItem_Click);
      // 
      // tsmi_Tools
      // 
      this.tsmi_Tools.Name = "tsmi_Tools";
      this.tsmi_Tools.Size = new System.Drawing.Size(65, 29);
      this.tsmi_Tools.Text = "Tools";
      // 
      // tsmi_Help
      // 
      this.tsmi_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_GetUpdates,
            this.tsmi_LogConsole});
      this.tsmi_Help.Name = "tsmi_Help";
      this.tsmi_Help.Size = new System.Drawing.Size(61, 29);
      this.tsmi_Help.Text = "Help";
      // 
      // tsmi_GetUpdates
      // 
      this.tsmi_GetUpdates.Name = "tsmi_GetUpdates";
      this.tsmi_GetUpdates.Size = new System.Drawing.Size(268, 30);
      this.tsmi_GetUpdates.Text = "Check for updates ...";
      this.tsmi_GetUpdates.Click += new System.EventHandler(this.GetUpdatesToolStripMenuItem_Click);
      // 
      // tsmi_LogConsole
      // 
      this.tsmi_LogConsole.Name = "tsmi_LogConsole";
      this.tsmi_LogConsole.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
      this.tsmi_LogConsole.Size = new System.Drawing.Size(268, 30);
      this.tsmi_LogConsole.Text = "Log console ...";
      this.tsmi_LogConsole.Click += new System.EventHandler(this.LogConsoleToolStripMenuItem_Click);
      // 
      // tsmi_Minibrowser
      // 
      this.tsmi_Minibrowser.Name = "tsmi_Minibrowser";
      this.tsmi_Minibrowser.Size = new System.Drawing.Size(296, 30);
      this.tsmi_Minibrowser.Text = "Minibrowser ...";
      this.tsmi_Minibrowser.Click += new System.EventHandler(this.TSMI_Minibrowser_Click);
      // 
      // tsmi_DetectInterfaces
      // 
      this.tsmi_DetectInterfaces.Name = "tsmi_DetectInterfaces";
      this.tsmi_DetectInterfaces.Size = new System.Drawing.Size(296, 30);
      this.tsmi_DetectInterfaces.Text = "Detect network interfaces";
      this.tsmi_DetectInterfaces.Click += new System.EventHandler(this.SearchNetworkInterfacesToolStripMenuItem_Click);
      // 
      // ofd_ImportSession
      // 
      this.ofd_ImportSession.DefaultExt = "mry";
      this.ofd_ImportSession.Filter = "Minary session file | *.mry";
      this.ofd_ImportSession.Title = "Select Minary session file";
      // 
      // l_AS_Proxy_Key
      // 
      this.l_AS_Proxy_Key.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.l_AS_Proxy_Key.AutoSize = true;
      this.l_AS_Proxy_Key.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_AS_Proxy_Key.Location = new System.Drawing.Point(24, 872);
      this.l_AS_Proxy_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_AS_Proxy_Key.Name = "l_AS_Proxy_Key";
      this.l_AS_Proxy_Key.Size = new System.Drawing.Size(62, 20);
      this.l_AS_Proxy_Key.TabIndex = 0;
      this.l_AS_Proxy_Key.Text = "Proxy:";
      // 
      // l_AS_APE_Key
      // 
      this.l_AS_APE_Key.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.l_AS_APE_Key.AutoSize = true;
      this.l_AS_APE_Key.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_AS_APE_Key.Location = new System.Drawing.Point(159, 872);
      this.l_AS_APE_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_AS_APE_Key.Name = "l_AS_APE_Key";
      this.l_AS_APE_Key.Size = new System.Drawing.Size(113, 20);
      this.l_AS_APE_Key.TabIndex = 0;
      this.l_AS_APE_Key.Text = "ARP poison:";
      // 
      // l_AS_Sniffer_Key
      // 
      this.l_AS_Sniffer_Key.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.l_AS_Sniffer_Key.AutoSize = true;
      this.l_AS_Sniffer_Key.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_AS_Sniffer_Key.Location = new System.Drawing.Point(350, 872);
      this.l_AS_Sniffer_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.l_AS_Sniffer_Key.Name = "l_AS_Sniffer_Key";
      this.l_AS_Sniffer_Key.Size = new System.Drawing.Size(71, 20);
      this.l_AS_Sniffer_Key.TabIndex = 0;
      this.l_AS_Sniffer_Key.Text = "Sniffer:";
      // 
      // pb_StatusProxy
      // 
      this.pb_StatusProxy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.pb_StatusProxy.Location = new System.Drawing.Point(94, 871);
      this.pb_StatusProxy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.pb_StatusProxy.Name = "pb_StatusProxy";
      this.pb_StatusProxy.Size = new System.Drawing.Size(27, 22);
      this.pb_StatusProxy.TabIndex = 8;
      this.pb_StatusProxy.TabStop = false;
      this.pb_StatusProxy.Tag = "HttpReverseProxy";
      // 
      // il_AttackServiceStat
      // 
      this.il_AttackServiceStat.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_AttackServiceStat.ImageStream")));
      this.il_AttackServiceStat.TransparentColor = System.Drawing.Color.Transparent;
      this.il_AttackServiceStat.Images.SetKeyName(0, "fa_paus_plugin.png");
      this.il_AttackServiceStat.Images.SetKeyName(1, "fa_play_plugin.png");
      this.il_AttackServiceStat.Images.SetKeyName(2, "fa_stop_plugin.png");
      // 
      // pb_StatusArpPoison
      // 
      this.pb_StatusArpPoison.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.pb_StatusArpPoison.Location = new System.Drawing.Point(284, 871);
      this.pb_StatusArpPoison.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.pb_StatusArpPoison.Name = "pb_StatusArpPoison";
      this.pb_StatusArpPoison.Size = new System.Drawing.Size(27, 22);
      this.pb_StatusArpPoison.TabIndex = 9;
      this.pb_StatusArpPoison.TabStop = false;
      this.pb_StatusArpPoison.Tag = "ArpPoisoning";
      // 
      // pb_StatusDataSniffer
      // 
      this.pb_StatusDataSniffer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.pb_StatusDataSniffer.Location = new System.Drawing.Point(432, 871);
      this.pb_StatusDataSniffer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.pb_StatusDataSniffer.Name = "pb_StatusDataSniffer";
      this.pb_StatusDataSniffer.Size = new System.Drawing.Size(27, 22);
      this.pb_StatusDataSniffer.TabIndex = 10;
      this.pb_StatusDataSniffer.TabStop = false;
      this.pb_StatusDataSniffer.Tag = "Sniffer";
      // 
      // MinaryMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1671, 906);
      this.Controls.Add(this.pb_StatusDataSniffer);
      this.Controls.Add(this.pb_StatusArpPoison);
      this.Controls.Add(this.pb_StatusProxy);
      this.Controls.Add(this.l_AS_Sniffer_Key);
      this.Controls.Add(this.l_AS_APE_Key);
      this.Controls.Add(this.l_AS_Proxy_Key);
      this.Controls.Add(this.tc_Plugins);
      this.Controls.Add(this.gb_TargetRange);
      this.Controls.Add(this.bt_Attack);
      this.Controls.Add(this.bt_ScanLan);
      this.Controls.Add(this.gb_Interfaces);
      this.Controls.Add(this.ms_MainWindow);
      this.Icon = global::Minary.Properties.Resources.Minary;
      this.MainMenuStrip = this.ms_MainWindow;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MinimumSize = new System.Drawing.Size(1684, 934);
      this.Name = "MinaryMain";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = " Minary ";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MinaryMain_FormClosing);
      this.Shown += new System.EventHandler(this.MinaryMain_Shown);
      this.gb_Interfaces.ResumeLayout(false);
      this.gb_Interfaces.PerformLayout();
      this.gb_TargetRange.ResumeLayout(false);
      this.gb_TargetRange.PerformLayout();
      this.tc_Plugins.ResumeLayout(false);
      this.tp_MinaryPluginCatalog.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgv_MainPlugins)).EndInit();
      this.ms_MainWindow.ResumeLayout(false);
      this.ms_MainWindow.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pb_StatusProxy)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pb_StatusArpPoison)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pb_StatusDataSniffer)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox gb_Interfaces;
    private System.Windows.Forms.ComboBox cb_Interfaces;
    private System.Windows.Forms.Label lab_Interface;
    private System.Windows.Forms.GroupBox gb_TargetRange;
    private System.Windows.Forms.TextBox tb_NetworkStopIp;
    private System.Windows.Forms.TextBox tb_NetworkStartIp;
    private System.Windows.Forms.Label lab_StartIP;
    private System.Windows.Forms.TabControl tc_Plugins;
    private System.Windows.Forms.TabPage tp_MinaryPluginCatalog;
    private System.Windows.Forms.DataGridView dgv_MainPlugins;
    private System.Windows.Forms.MenuStrip ms_MainWindow;
    private System.Windows.Forms.ToolStripMenuItem tsmi_File;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Exit;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Help;
    private System.Windows.Forms.ToolStripMenuItem tsmi_GetUpdates;
    private System.Windows.Forms.ToolStripMenuItem tsmi_LogConsole;
    private System.Windows.Forms.Button bt_ScanLan;
    private System.Windows.Forms.Label l_GatewayIP;
    private System.Windows.Forms.TextBox tb_GatewayIp;
    private System.Windows.Forms.Button bt_Attack;
    private System.Windows.Forms.Label l_NetrangeSeparator;
    private System.Windows.Forms.ImageList il_PluginStat;
    private System.Windows.Forms.TextBox tb_Vendor;
    private System.Windows.Forms.Label l_VendorTitle;
    private System.Windows.Forms.TextBox tb_GatewayMac;
    private System.Windows.Forms.Label l_GatewayMAC;
    private System.Windows.Forms.OpenFileDialog ofd_ImportSession;
    private System.Windows.Forms.ToolStripMenuItem templateToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tsmi_LoadTemplate;
    private System.Windows.Forms.ToolStripMenuItem tsmi_CreateTemplate;
    private System.Windows.Forms.TextBox tb_TemplateName;
    private System.Windows.Forms.Label l_Template;
    private System.Windows.Forms.Label l_AS_Proxy_Key;
    private System.Windows.Forms.Label l_AS_APE_Key;
    private System.Windows.Forms.Label l_AS_Sniffer_Key;
    private System.Windows.Forms.PictureBox pb_StatusProxy;
    private System.Windows.Forms.ImageList il_AttackServiceStat;
    private System.Windows.Forms.PictureBox pb_StatusArpPoison;
    private System.Windows.Forms.PictureBox pb_StatusDataSniffer;
    private System.Windows.Forms.ToolStripMenuItem tsmi_UnloadTemplate;
    private System.Windows.Forms.ToolStripMenuItem tsmi_ResetMinary;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Tools;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Minibrowser;
    private System.Windows.Forms.ToolStripMenuItem tsmi_DetectInterfaces;
    private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Beep;
    private System.Windows.Forms.ToolStripMenuItem tsmi_Debugging;
    private System.Windows.Forms.ToolStripMenuItem tsmi_About;
  }
}