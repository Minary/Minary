namespace NamedPiper
{
  public partial class NamedPiper
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
      this.bt_Send = new System.Windows.Forms.Button();
      this.bt_Close = new System.Windows.Forms.Button();
      this.gb_NamedPipes = new System.Windows.Forms.GroupBox();
      this.lab_PipeName = new System.Windows.Forms.Label();
      this.lab_Data = new System.Windows.Forms.Label();
      this.tb_PipeName = new System.Windows.Forms.TextBox();
      this.cb_Data = new System.Windows.Forms.ComboBox();
      this.gb_NamedPipes.SuspendLayout();
      this.SuspendLayout();
      // 
      // bt_Send
      // 
      this.bt_Send.Location = new System.Drawing.Point(438, 351);
      this.bt_Send.Name = "bt_Send";
      this.bt_Send.Size = new System.Drawing.Size(75, 23);
      this.bt_Send.TabIndex = 0;
      this.bt_Send.Text = "Send";
      this.bt_Send.UseVisualStyleBackColor = true;
      this.bt_Send.Click += new System.EventHandler(this.BT_Send_Click);
      // 
      // bt_Close
      // 
      this.bt_Close.Location = new System.Drawing.Point(555, 351);
      this.bt_Close.Name = "bt_Close";
      this.bt_Close.Size = new System.Drawing.Size(75, 23);
      this.bt_Close.TabIndex = 1;
      this.bt_Close.Text = "Close";
      this.bt_Close.UseVisualStyleBackColor = true;
      this.bt_Close.Click += new System.EventHandler(this.BT_Close_Click);
      // 
      // gb_NamedPipes
      // 
      this.gb_NamedPipes.Controls.Add(this.cb_Data);
      this.gb_NamedPipes.Controls.Add(this.tb_PipeName);
      this.gb_NamedPipes.Controls.Add(this.lab_Data);
      this.gb_NamedPipes.Controls.Add(this.lab_PipeName);
      this.gb_NamedPipes.Location = new System.Drawing.Point(22, 13);
      this.gb_NamedPipes.Name = "gb_NamedPipes";
      this.gb_NamedPipes.Size = new System.Drawing.Size(637, 323);
      this.gb_NamedPipes.TabIndex = 2;
      this.gb_NamedPipes.TabStop = false;
      // 
      // lab_PipeName
      // 
      this.lab_PipeName.AutoSize = true;
      this.lab_PipeName.Location = new System.Drawing.Point(23, 33);
      this.lab_PipeName.Name = "lab_PipeName";
      this.lab_PipeName.Size = new System.Drawing.Size(57, 13);
      this.lab_PipeName.TabIndex = 0;
      this.lab_PipeName.Text = "Pipe name";
      // 
      // lab_Data
      // 
      this.lab_Data.AutoSize = true;
      this.lab_Data.Location = new System.Drawing.Point(23, 84);
      this.lab_Data.Name = "lab_Data";
      this.lab_Data.Size = new System.Drawing.Size(30, 13);
      this.lab_Data.TabIndex = 1;
      this.lab_Data.Text = "Data";
      // 
      // tb_PipeName
      // 
      this.tb_PipeName.Location = new System.Drawing.Point(118, 25);
      this.tb_PipeName.Name = "tb_PipeName";
      this.tb_PipeName.Size = new System.Drawing.Size(490, 20);
      this.tb_PipeName.TabIndex = 2;
      this.tb_PipeName.Text = "Minary";
      // 
      // cb_Data
      // 
      this.cb_Data.FormattingEnabled = true;
      this.cb_Data.Location = new System.Drawing.Point(118, 84);
      this.cb_Data.Name = "cb_Data";
      this.cb_Data.Size = new System.Drawing.Size(490, 21);
      this.cb_Data.TabIndex = 3;
      // 
      // NamedPiper
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(671, 386);
      this.Controls.Add(this.gb_NamedPipes);
      this.Controls.Add(this.bt_Close);
      this.Controls.Add(this.bt_Send);
      this.Name = "NamedPiper";
      this.Text = "NamedPiper";
      this.gb_NamedPipes.ResumeLayout(false);
      this.gb_NamedPipes.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button bt_Send;
    private System.Windows.Forms.Button bt_Close;
    private System.Windows.Forms.GroupBox gb_NamedPipes;
    private System.Windows.Forms.ComboBox cb_Data;
    private System.Windows.Forms.TextBox tb_PipeName;
    private System.Windows.Forms.Label lab_Data;
    private System.Windows.Forms.Label lab_PipeName;
  }
}