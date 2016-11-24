namespace TcpClient
{
  public partial class TcpClient_Main
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
      this.bt_send = new System.Windows.Forms.Button();
      this.tb_Data = new System.Windows.Forms.TextBox();
      this.tb_Port = new System.Windows.Forms.TextBox();
      this.tb_Host = new System.Windows.Forms.TextBox();
      this.bt_Cancel = new System.Windows.Forms.Button();
      this.l_Host = new System.Windows.Forms.Label();
      this.l_Port = new System.Windows.Forms.Label();
      this.tb_Output = new System.Windows.Forms.TextBox();
      this.l_output = new System.Windows.Forms.Label();
      this.l_TimestampServerResponse = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // bt_send
      // 
      this.bt_send.Location = new System.Drawing.Point(791, 471);
      this.bt_send.Name = "bt_send";
      this.bt_send.Size = new System.Drawing.Size(75, 23);
      this.bt_send.TabIndex = 4;
      this.bt_send.Text = "Send";
      this.bt_send.UseVisualStyleBackColor = true;
      this.bt_send.Click += new System.EventHandler(this.BT_send_Click);
      // 
      // tb_Data
      // 
      this.tb_Data.Location = new System.Drawing.Point(12, 46);
      this.tb_Data.Multiline = true;
      this.tb_Data.Name = "tb_Data";
      this.tb_Data.Size = new System.Drawing.Size(972, 240);
      this.tb_Data.TabIndex = 3;
      // 
      // tb_Port
      // 
      this.tb_Port.Location = new System.Drawing.Point(254, 20);
      this.tb_Port.Name = "tb_Port";
      this.tb_Port.Size = new System.Drawing.Size(53, 20);
      this.tb_Port.TabIndex = 2;
      // 
      // tb_Host
      // 
      this.tb_Host.Location = new System.Drawing.Point(46, 20);
      this.tb_Host.Name = "tb_Host";
      this.tb_Host.Size = new System.Drawing.Size(147, 20);
      this.tb_Host.TabIndex = 1;
      // 
      // bt_Cancel
      // 
      this.bt_Cancel.Location = new System.Drawing.Point(888, 471);
      this.bt_Cancel.Name = "bt_Cancel";
      this.bt_Cancel.Size = new System.Drawing.Size(75, 23);
      this.bt_Cancel.TabIndex = 5;
      this.bt_Cancel.Text = "Cancel";
      this.bt_Cancel.UseVisualStyleBackColor = true;
      this.bt_Cancel.Click += new System.EventHandler(this.BT_Cancel_Click);
      // 
      // l_Host
      // 
      this.l_Host.AutoSize = true;
      this.l_Host.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_Host.Location = new System.Drawing.Point(13, 23);
      this.l_Host.Name = "l_Host";
      this.l_Host.Size = new System.Drawing.Size(33, 13);
      this.l_Host.TabIndex = 0;
      this.l_Host.Text = "Host";
      // 
      // l_Port
      // 
      this.l_Port.AutoSize = true;
      this.l_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_Port.Location = new System.Drawing.Point(219, 23);
      this.l_Port.Name = "l_Port";
      this.l_Port.Size = new System.Drawing.Size(30, 13);
      this.l_Port.TabIndex = 0;
      this.l_Port.Text = "Port";
      // 
      // tb_Output
      // 
      this.tb_Output.BackColor = System.Drawing.Color.WhiteSmoke;
      this.tb_Output.Location = new System.Drawing.Point(12, 330);
      this.tb_Output.Multiline = true;
      this.tb_Output.Name = "tb_Output";
      this.tb_Output.ReadOnly = true;
      this.tb_Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tb_Output.Size = new System.Drawing.Size(972, 135);
      this.tb_Output.TabIndex = 0;
      this.tb_Output.TabStop = false;
      // 
      // l_output
      // 
      this.l_output.AutoSize = true;
      this.l_output.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_output.Location = new System.Drawing.Point(13, 309);
      this.l_output.Name = "l_output";
      this.l_output.Size = new System.Drawing.Size(49, 13);
      this.l_output.TabIndex = 0;
      this.l_output.Text = "Output:";
      // 
      // l_TimestampServerResponse
      // 
      this.l_TimestampServerResponse.AutoSize = true;
      this.l_TimestampServerResponse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.l_TimestampServerResponse.Location = new System.Drawing.Point(64, 309);
      this.l_TimestampServerResponse.Name = "l_TimestampServerResponse";
      this.l_TimestampServerResponse.Size = new System.Drawing.Size(48, 13);
      this.l_TimestampServerResponse.TabIndex = 6;
      this.l_TimestampServerResponse.Text = "DUMMY";
      // 
      // TcpClient_Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(996, 506);
      this.Controls.Add(this.l_TimestampServerResponse);
      this.Controls.Add(this.l_output);
      this.Controls.Add(this.tb_Output);
      this.Controls.Add(this.l_Port);
      this.Controls.Add(this.l_Host);
      this.Controls.Add(this.bt_Cancel);
      this.Controls.Add(this.tb_Host);
      this.Controls.Add(this.tb_Port);
      this.Controls.Add(this.tb_Data);
      this.Controls.Add(this.bt_send);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "TcpClient_Main";
      this.Text = "TcpClient";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button bt_send;
    private System.Windows.Forms.TextBox tb_Data;
    private System.Windows.Forms.TextBox tb_Port;
    private System.Windows.Forms.TextBox tb_Host;
    private System.Windows.Forms.Button bt_Cancel;
    private System.Windows.Forms.Label l_Host;
    private System.Windows.Forms.Label l_Port;
    private System.Windows.Forms.TextBox tb_Output;
    private System.Windows.Forms.Label l_output;
    private System.Windows.Forms.Label l_TimestampServerResponse;
  }
}