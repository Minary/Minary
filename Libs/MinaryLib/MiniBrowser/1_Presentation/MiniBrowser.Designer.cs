namespace Minary.MiniBrowser
{
  public partial class Browser
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param firewallRuleName="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
      this.gb_Details = new System.Windows.Forms.GroupBox();
      this.cmb_UserAgent = new System.Windows.Forms.ComboBox();
      this.cb_Cookies = new System.Windows.Forms.CheckBox();
      this.bt_Open = new System.Windows.Forms.Button();
      this.tb_UserAgent = new System.Windows.Forms.TextBox();
      this.tb_Cookies = new System.Windows.Forms.TextBox();
      this.l_UserAgent = new System.Windows.Forms.Label();
      this.l_Cookies = new System.Windows.Forms.Label();
      this.l_URL = new System.Windows.Forms.Label();
      this.tb_URL = new System.Windows.Forms.TextBox();
      this.cb_UserAgent = new System.Windows.Forms.CheckBox();
      this.gb_WebPage = new System.Windows.Forms.GroupBox();
      this.wb_MiniBrowser = new System.Windows.Forms.WebBrowser();
      this.bgw_GetAccessToken = new System.ComponentModel.BackgroundWorker();
      this.gb_Details.SuspendLayout();
      this.gb_WebPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // gb_Details
      // 
      this.gb_Details.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gb_Details.Controls.Add(this.cmb_UserAgent);
      this.gb_Details.Controls.Add(this.cb_UserAgent);
      this.gb_Details.Controls.Add(this.cb_Cookies);
      this.gb_Details.Controls.Add(this.bt_Open);
      this.gb_Details.Controls.Add(this.tb_UserAgent);
      this.gb_Details.Controls.Add(this.tb_Cookies);
      this.gb_Details.Controls.Add(this.l_UserAgent);
      this.gb_Details.Controls.Add(this.l_Cookies);
      this.gb_Details.Controls.Add(this.l_URL);
      this.gb_Details.Controls.Add(this.tb_URL);
      this.gb_Details.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.gb_Details.Location = new System.Drawing.Point(11, 5);
      this.gb_Details.Margin = new System.Windows.Forms.Padding(5);
      this.gb_Details.Name = "gb_Details";
      this.gb_Details.Size = new System.Drawing.Size(712, 125);
      this.gb_Details.TabIndex = 0;
      this.gb_Details.TabStop = false;
      // 
      // cmb_UserAgent
      // 
      this.cmb_UserAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cmb_UserAgent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmb_UserAgent.FormattingEnabled = true;
      this.cmb_UserAgent.Location = new System.Drawing.Point(636, 84);
      this.cmb_UserAgent.Name = "cmb_UserAgent";
      this.cmb_UserAgent.Size = new System.Drawing.Size(59, 21);
      this.cmb_UserAgent.TabIndex = 6;
      this.cmb_UserAgent.SelectedIndexChanged += new System.EventHandler(this.CMB_UserAgent_SelectedIndexChanged);
      // 
      // cb_Cookies
      // 
      this.cb_Cookies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cb_Cookies.AutoSize = true;
      this.cb_Cookies.Checked = true;
      this.cb_Cookies.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cb_Cookies.Location = new System.Drawing.Point(573, 63);
      this.cb_Cookies.Name = "cb_Cookies";
      this.cb_Cookies.Size = new System.Drawing.Size(53, 17);
      this.cb_Cookies.TabIndex = 5;
      this.cb_Cookies.Text = "Use it";
      this.cb_Cookies.UseVisualStyleBackColor = true;
      // 
      // bt_Open
      // 
      this.bt_Open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.bt_Open.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.bt_Open.Location = new System.Drawing.Point(636, 30);
      this.bt_Open.Name = "bt_Open";
      this.bt_Open.Size = new System.Drawing.Size(59, 23);
      this.bt_Open.TabIndex = 4;
      this.bt_Open.Text = "Open";
      this.bt_Open.UseVisualStyleBackColor = true;
      this.bt_Open.Click += new System.EventHandler(this.BT_Open_Click);
      // 
      // tb_UserAgent
      // 
      this.tb_UserAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tb_UserAgent.Location = new System.Drawing.Point(105, 86);
      this.tb_UserAgent.Name = "tb_UserAgent";
      this.tb_UserAgent.Size = new System.Drawing.Size(455, 20);
      this.tb_UserAgent.TabIndex = 3;
      // 
      // tb_Cookies
      // 
      this.tb_Cookies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tb_Cookies.Location = new System.Drawing.Point(105, 60);
      this.tb_Cookies.Name = "tb_Cookies";
      this.tb_Cookies.Size = new System.Drawing.Size(455, 20);
      this.tb_Cookies.TabIndex = 2;
      // 
      // l_UserAgent
      // 
      this.l_UserAgent.AutoSize = true;
      this.l_UserAgent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_UserAgent.Location = new System.Drawing.Point(16, 86);
      this.l_UserAgent.Name = "l_UserAgent";
      this.l_UserAgent.Size = new System.Drawing.Size(69, 13);
      this.l_UserAgent.TabIndex = 0;
      this.l_UserAgent.Text = "User agent";
      // 
      // l_Cookies
      // 
      this.l_Cookies.AutoSize = true;
      this.l_Cookies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_Cookies.Location = new System.Drawing.Point(16, 60);
      this.l_Cookies.Name = "l_Cookies";
      this.l_Cookies.Size = new System.Drawing.Size(52, 13);
      this.l_Cookies.TabIndex = 0;
      this.l_Cookies.Text = "Cookies";
      // 
      // l_URL
      // 
      this.l_URL.AutoSize = true;
      this.l_URL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_URL.Location = new System.Drawing.Point(16, 33);
      this.l_URL.Name = "l_URL";
      this.l_URL.Size = new System.Drawing.Size(32, 13);
      this.l_URL.TabIndex = 0;
      this.l_URL.Text = "URL";
      // 
      // tb_URL
      // 
      this.tb_URL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tb_URL.Location = new System.Drawing.Point(105, 33);
      this.tb_URL.Name = "tb_URL";
      this.tb_URL.Size = new System.Drawing.Size(455, 20);
      this.tb_URL.TabIndex = 1;
      this.tb_URL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_URL_KeyDown);
      // 
      // cb_UserAgent
      // 
      this.cb_UserAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cb_UserAgent.AutoSize = true;
      this.cb_UserAgent.Location = new System.Drawing.Point(573, 88);
      this.cb_UserAgent.Name = "cb_UserAgent";
      this.cb_UserAgent.Size = new System.Drawing.Size(53, 17);
      this.cb_UserAgent.TabIndex = 6;
      this.cb_UserAgent.Text = "Use it";
      this.cb_UserAgent.UseVisualStyleBackColor = true;
      // 
      // gb_WebPage
      // 
      this.gb_WebPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gb_WebPage.Controls.Add(this.wb_MiniBrowser);
      this.gb_WebPage.Location = new System.Drawing.Point(12, 149);
      this.gb_WebPage.Name = "gb_WebPage";
      this.gb_WebPage.Size = new System.Drawing.Size(714, 356);
      this.gb_WebPage.TabIndex = 1;
      this.gb_WebPage.TabStop = false;
      // 
      // wb_MiniBrowser
      // 
      this.wb_MiniBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.wb_MiniBrowser.Location = new System.Drawing.Point(3, 16);
      this.wb_MiniBrowser.MinimumSize = new System.Drawing.Size(20, 20);
      this.wb_MiniBrowser.Name = "wb_MiniBrowser";
      this.wb_MiniBrowser.ScriptErrorsSuppressed = true;
      this.wb_MiniBrowser.Size = new System.Drawing.Size(708, 337);
      this.wb_MiniBrowser.TabIndex = 7;
      // 
      // bgw_GetAccessToken
      // 
      this.bgw_GetAccessToken.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_GetAccessToken_DoWork);
      // 
      // Browser
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(740, 517);
      this.Controls.Add(this.gb_WebPage);
      this.Controls.Add(this.gb_Details);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.Name = "Browser";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "MiniBrowser";
      this.gb_Details.ResumeLayout(false);
      this.gb_Details.PerformLayout();
      this.gb_WebPage.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Details;
        private System.Windows.Forms.TextBox tb_URL;
        private System.Windows.Forms.TextBox tb_UserAgent;
        private System.Windows.Forms.TextBox tb_Cookies;
        private System.Windows.Forms.Label l_UserAgent;
        private System.Windows.Forms.Label l_Cookies;
        private System.Windows.Forms.Label l_URL;
        private System.Windows.Forms.GroupBox gb_WebPage;
        private System.Windows.Forms.WebBrowser wb_MiniBrowser;
        private System.Windows.Forms.Button bt_Open;
        private System.Windows.Forms.CheckBox cb_UserAgent;
        private System.Windows.Forms.CheckBox cb_Cookies;
        private System.ComponentModel.BackgroundWorker bgw_GetAccessToken;
        private System.Windows.Forms.ComboBox cmb_UserAgent;
    }
}