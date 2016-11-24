namespace Minary.Template.Presentation
{

  public partial class CreateTemplate
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
      this.gb_CreateTemplate = new System.Windows.Forms.GroupBox();
      this.tb_Version = new System.Windows.Forms.TextBox();
      this.l_Version = new System.Windows.Forms.Label();
      this.tb_AuthorName = new System.Windows.Forms.TextBox();
      this.tb_TemplateReferenceLink = new System.Windows.Forms.TextBox();
      this.l_TemplateReferenceLink = new System.Windows.Forms.Label();
      this.l_AuthorName = new System.Windows.Forms.Label();
      this.tb_TemplateDescription = new System.Windows.Forms.TextBox();
      this.tb_TemplateName = new System.Windows.Forms.TextBox();
      this.l_TemplateDescription = new System.Windows.Forms.Label();
      this.l_TemplateName = new System.Windows.Forms.Label();
      this.bt_Cancel = new System.Windows.Forms.Button();
      this.bt_Save = new System.Windows.Forms.Button();
      this.cms_Hops = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.deleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.sfd_TemplateFile = new System.Windows.Forms.SaveFileDialog();
      this.tc_CreateTemplate = new System.Windows.Forms.TabControl();
      this.tp_description = new System.Windows.Forms.TabPage();
      this.tp_AttackConfig = new System.Windows.Forms.TabPage();
      this.gb_AttackConfig = new System.Windows.Forms.GroupBox();
      this.tb_MaxNoTargetSystems = new System.Windows.Forms.TextBox();
      this.l_MaxNoTargetSystems = new System.Windows.Forms.Label();
      this.cb_StartAttackingTargets = new System.Windows.Forms.CheckBox();
      this.cb_ArpScan = new System.Windows.Forms.CheckBox();
      this.gb_CreateTemplate.SuspendLayout();
      this.cms_Hops.SuspendLayout();
      this.tc_CreateTemplate.SuspendLayout();
      this.tp_description.SuspendLayout();
      this.tp_AttackConfig.SuspendLayout();
      this.gb_AttackConfig.SuspendLayout();
      this.SuspendLayout();
      // 
      // gb_CreateTemplate
      // 
      this.gb_CreateTemplate.Controls.Add(this.tb_Version);
      this.gb_CreateTemplate.Controls.Add(this.l_Version);
      this.gb_CreateTemplate.Controls.Add(this.tb_AuthorName);
      this.gb_CreateTemplate.Controls.Add(this.tb_TemplateReferenceLink);
      this.gb_CreateTemplate.Controls.Add(this.l_TemplateReferenceLink);
      this.gb_CreateTemplate.Controls.Add(this.l_AuthorName);
      this.gb_CreateTemplate.Controls.Add(this.tb_TemplateDescription);
      this.gb_CreateTemplate.Controls.Add(this.tb_TemplateName);
      this.gb_CreateTemplate.Controls.Add(this.l_TemplateDescription);
      this.gb_CreateTemplate.Controls.Add(this.l_TemplateName);
      this.gb_CreateTemplate.Location = new System.Drawing.Point(17, 20);
      this.gb_CreateTemplate.Name = "gb_CreateTemplate";
      this.gb_CreateTemplate.Size = new System.Drawing.Size(621, 258);
      this.gb_CreateTemplate.TabIndex = 1;
      this.gb_CreateTemplate.TabStop = false;
      // 
      // tb_Version
      // 
      this.tb_Version.Location = new System.Drawing.Point(147, 140);
      this.tb_Version.Name = "tb_Version";
      this.tb_Version.Size = new System.Drawing.Size(450, 20);
      this.tb_Version.TabIndex = 3;
      // 
      // l_Version
      // 
      this.l_Version.AutoSize = true;
      this.l_Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_Version.Location = new System.Drawing.Point(24, 143);
      this.l_Version.Name = "l_Version";
      this.l_Version.Size = new System.Drawing.Size(49, 13);
      this.l_Version.TabIndex = 0;
      this.l_Version.Text = "Version";
      // 
      // tb_AuthorName
      // 
      this.tb_AuthorName.Location = new System.Drawing.Point(145, 202);
      this.tb_AuthorName.Name = "tb_AuthorName";
      this.tb_AuthorName.Size = new System.Drawing.Size(452, 20);
      this.tb_AuthorName.TabIndex = 5;
      // 
      // tb_TemplateReferenceLink
      // 
      this.tb_TemplateReferenceLink.Location = new System.Drawing.Point(145, 170);
      this.tb_TemplateReferenceLink.Name = "tb_TemplateReferenceLink";
      this.tb_TemplateReferenceLink.Size = new System.Drawing.Size(452, 20);
      this.tb_TemplateReferenceLink.TabIndex = 4;
      // 
      // l_TemplateReferenceLink
      // 
      this.l_TemplateReferenceLink.AutoSize = true;
      this.l_TemplateReferenceLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_TemplateReferenceLink.Location = new System.Drawing.Point(22, 173);
      this.l_TemplateReferenceLink.Name = "l_TemplateReferenceLink";
      this.l_TemplateReferenceLink.Size = new System.Drawing.Size(90, 13);
      this.l_TemplateReferenceLink.TabIndex = 0;
      this.l_TemplateReferenceLink.Text = "Reference link";
      // 
      // l_AuthorName
      // 
      this.l_AuthorName.AutoSize = true;
      this.l_AuthorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_AuthorName.Location = new System.Drawing.Point(22, 205);
      this.l_AuthorName.Name = "l_AuthorName";
      this.l_AuthorName.Size = new System.Drawing.Size(78, 13);
      this.l_AuthorName.TabIndex = 0;
      this.l_AuthorName.Text = "Author name";
      // 
      // tb_TemplateDescription
      // 
      this.tb_TemplateDescription.Location = new System.Drawing.Point(145, 61);
      this.tb_TemplateDescription.Multiline = true;
      this.tb_TemplateDescription.Name = "tb_TemplateDescription";
      this.tb_TemplateDescription.Size = new System.Drawing.Size(452, 65);
      this.tb_TemplateDescription.TabIndex = 2;
      // 
      // tb_TemplateName
      // 
      this.tb_TemplateName.Location = new System.Drawing.Point(145, 28);
      this.tb_TemplateName.Name = "tb_TemplateName";
      this.tb_TemplateName.Size = new System.Drawing.Size(452, 20);
      this.tb_TemplateName.TabIndex = 1;
      // 
      // l_TemplateDescription
      // 
      this.l_TemplateDescription.AutoSize = true;
      this.l_TemplateDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_TemplateDescription.Location = new System.Drawing.Point(22, 64);
      this.l_TemplateDescription.Name = "l_TemplateDescription";
      this.l_TemplateDescription.Size = new System.Drawing.Size(71, 13);
      this.l_TemplateDescription.TabIndex = 0;
      this.l_TemplateDescription.Text = "Description";
      // 
      // l_TemplateName
      // 
      this.l_TemplateName.AutoSize = true;
      this.l_TemplateName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_TemplateName.Location = new System.Drawing.Point(22, 35);
      this.l_TemplateName.Name = "l_TemplateName";
      this.l_TemplateName.Size = new System.Drawing.Size(39, 13);
      this.l_TemplateName.TabIndex = 0;
      this.l_TemplateName.Text = "Name";
      // 
      // bt_Cancel
      // 
      this.bt_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.bt_Cancel.Location = new System.Drawing.Point(498, 361);
      this.bt_Cancel.Name = "bt_Cancel";
      this.bt_Cancel.Size = new System.Drawing.Size(75, 22);
      this.bt_Cancel.TabIndex = 21;
      this.bt_Cancel.Text = "Cancel";
      this.bt_Cancel.UseVisualStyleBackColor = true;
      this.bt_Cancel.Click += new System.EventHandler(this.BT_Cancel_Click);
      // 
      // bt_Save
      // 
      this.bt_Save.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.bt_Save.Location = new System.Drawing.Point(592, 361);
      this.bt_Save.Name = "bt_Save";
      this.bt_Save.Size = new System.Drawing.Size(75, 22);
      this.bt_Save.TabIndex = 22;
      this.bt_Save.Text = "Save";
      this.bt_Save.UseVisualStyleBackColor = true;
      this.bt_Save.Click += new System.EventHandler(this.Bt_Save_Click);
      // 
      // cms_Hops
      // 
      this.cms_Hops.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteEntryToolStripMenuItem,
            this.clearListToolStripMenuItem});
      this.cms_Hops.Name = "cms_Hops";
      this.cms_Hops.Size = new System.Drawing.Size(68, 48);
      // 
      // deleteEntryToolStripMenuItem
      // 
      this.deleteEntryToolStripMenuItem.Name = "deleteEntryToolStripMenuItem";
      this.deleteEntryToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
      // 
      // clearListToolStripMenuItem
      // 
      this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
      this.clearListToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
      // 
      // tc_CreateTemplate
      // 
      this.tc_CreateTemplate.Controls.Add(this.tp_description);
      this.tc_CreateTemplate.Controls.Add(this.tp_AttackConfig);
      this.tc_CreateTemplate.Location = new System.Drawing.Point(12, 23);
      this.tc_CreateTemplate.Name = "tc_CreateTemplate";
      this.tc_CreateTemplate.SelectedIndex = 0;
      this.tc_CreateTemplate.Size = new System.Drawing.Size(685, 330);
      this.tc_CreateTemplate.TabIndex = 0;
      this.tc_CreateTemplate.TabStop = false;
      // 
      // tp_description
      // 
      this.tp_description.BackColor = System.Drawing.Color.WhiteSmoke;
      this.tp_description.Controls.Add(this.gb_CreateTemplate);
      this.tp_description.Location = new System.Drawing.Point(4, 22);
      this.tp_description.Name = "tp_description";
      this.tp_description.Padding = new System.Windows.Forms.Padding(3);
      this.tp_description.Size = new System.Drawing.Size(677, 304);
      this.tp_description.TabIndex = 0;
      this.tp_description.Text = "Description";
      // 
      // tp_AttackConfig
      // 
      this.tp_AttackConfig.BackColor = System.Drawing.Color.WhiteSmoke;
      this.tp_AttackConfig.Controls.Add(this.gb_AttackConfig);
      this.tp_AttackConfig.Location = new System.Drawing.Point(4, 22);
      this.tp_AttackConfig.Name = "tp_AttackConfig";
      this.tp_AttackConfig.Padding = new System.Windows.Forms.Padding(3);
      this.tp_AttackConfig.Size = new System.Drawing.Size(677, 304);
      this.tp_AttackConfig.TabIndex = 2;
      this.tp_AttackConfig.Text = "Attack config";
      // 
      // gb_AttackConfig
      // 
      this.gb_AttackConfig.Controls.Add(this.tb_MaxNoTargetSystems);
      this.gb_AttackConfig.Controls.Add(this.l_MaxNoTargetSystems);
      this.gb_AttackConfig.Controls.Add(this.cb_StartAttackingTargets);
      this.gb_AttackConfig.Controls.Add(this.cb_ArpScan);
      this.gb_AttackConfig.Location = new System.Drawing.Point(17, 24);
      this.gb_AttackConfig.Name = "gb_AttackConfig";
      this.gb_AttackConfig.Size = new System.Drawing.Size(621, 258);
      this.gb_AttackConfig.TabIndex = 0;
      this.gb_AttackConfig.TabStop = false;
      // 
      // tb_MaxNoTargetSystems
      // 
      this.tb_MaxNoTargetSystems.Location = new System.Drawing.Point(224, 102);
      this.tb_MaxNoTargetSystems.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
      this.tb_MaxNoTargetSystems.MaxLength = 3;
      this.tb_MaxNoTargetSystems.Name = "tb_MaxNoTargetSystems";
      this.tb_MaxNoTargetSystems.Size = new System.Drawing.Size(23, 20);
      this.tb_MaxNoTargetSystems.TabIndex = 3;
      this.tb_MaxNoTargetSystems.Text = "5";
      this.tb_MaxNoTargetSystems.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // l_MaxNoTargetSystems
      // 
      this.l_MaxNoTargetSystems.AutoSize = true;
      this.l_MaxNoTargetSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_MaxNoTargetSystems.Location = new System.Drawing.Point(20, 105);
      this.l_MaxNoTargetSystems.Name = "l_MaxNoTargetSystems";
      this.l_MaxNoTargetSystems.Size = new System.Drawing.Size(188, 13);
      this.l_MaxNoTargetSystems.TabIndex = 0;
      this.l_MaxNoTargetSystems.Text = "Maximum number target systems";
      // 
      // cb_StartAttackingTargets
      // 
      this.cb_StartAttackingTargets.AutoSize = true;
      this.cb_StartAttackingTargets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.cb_StartAttackingTargets.Checked = true;
      this.cb_StartAttackingTargets.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cb_StartAttackingTargets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cb_StartAttackingTargets.Location = new System.Drawing.Point(23, 65);
      this.cb_StartAttackingTargets.Name = "cb_StartAttackingTargets";
      this.cb_StartAttackingTargets.Size = new System.Drawing.Size(224, 17);
      this.cb_StartAttackingTargets.TabIndex = 2;
      this.cb_StartAttackingTargets.Text = "Start attacking target network       ";
      this.cb_StartAttackingTargets.UseVisualStyleBackColor = true;
      // 
      // cb_ArpScan
      // 
      this.cb_ArpScan.AutoSize = true;
      this.cb_ArpScan.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.cb_ArpScan.Checked = true;
      this.cb_ArpScan.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cb_ArpScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cb_ArpScan.Location = new System.Drawing.Point(23, 29);
      this.cb_ArpScan.Name = "cb_ArpScan";
      this.cb_ArpScan.Size = new System.Drawing.Size(224, 17);
      this.cb_ArpScan.TabIndex = 1;
      this.cb_ArpScan.Text = "ARP scan target network              ";
      this.cb_ArpScan.UseVisualStyleBackColor = true;
      // 
      // CreateTemplate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(707, 395);
      this.Controls.Add(this.tc_CreateTemplate);
      this.Controls.Add(this.bt_Save);
      this.Controls.Add(this.bt_Cancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "CreateTemplate";
      this.Text = "Create attack template";
      this.gb_CreateTemplate.ResumeLayout(false);
      this.gb_CreateTemplate.PerformLayout();
      this.cms_Hops.ResumeLayout(false);
      this.tc_CreateTemplate.ResumeLayout(false);
      this.tp_description.ResumeLayout(false);
      this.tp_AttackConfig.ResumeLayout(false);
      this.gb_AttackConfig.ResumeLayout(false);
      this.gb_AttackConfig.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox gb_CreateTemplate;
    private System.Windows.Forms.Button bt_Cancel;
    private System.Windows.Forms.Button bt_Save;
    private System.Windows.Forms.Label l_TemplateName;
    private System.Windows.Forms.Label l_TemplateDescription;
    private System.Windows.Forms.TextBox tb_TemplateDescription;
    private System.Windows.Forms.TextBox tb_TemplateName;
    private System.Windows.Forms.TextBox tb_AuthorName;
    private System.Windows.Forms.TextBox tb_TemplateReferenceLink;
    private System.Windows.Forms.Label l_TemplateReferenceLink;
    private System.Windows.Forms.Label l_AuthorName;
    private System.Windows.Forms.ContextMenuStrip cms_Hops;
    private System.Windows.Forms.ToolStripMenuItem deleteEntryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
    private System.Windows.Forms.SaveFileDialog sfd_TemplateFile;
    private System.Windows.Forms.TextBox tb_Version;
    private System.Windows.Forms.Label l_Version;
    private System.Windows.Forms.TabControl tc_CreateTemplate;
    private System.Windows.Forms.TabPage tp_description;
    private System.Windows.Forms.TabPage tp_AttackConfig;
    private System.Windows.Forms.GroupBox gb_AttackConfig;
    private System.Windows.Forms.CheckBox cb_StartAttackingTargets;
    private System.Windows.Forms.CheckBox cb_ArpScan;
    private System.Windows.Forms.Label l_MaxNoTargetSystems;
    private System.Windows.Forms.TextBox tb_MaxNoTargetSystems;
  }
}