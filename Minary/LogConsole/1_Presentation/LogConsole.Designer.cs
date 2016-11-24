namespace Minary.LogConsole.Main
{
  public partial class LogConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogConsole));
            this.tb_LogContent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tb_LogContent
            // 
            this.tb_LogContent.BackColor = System.Drawing.SystemColors.Window;
            this.tb_LogContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_LogContent.Location = new System.Drawing.Point(0, 0);
            this.tb_LogContent.Multiline = true;
            this.tb_LogContent.Name = "tb_LogContent";
            this.tb_LogContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_LogContent.Size = new System.Drawing.Size(1207, 262);
            this.tb_LogContent.TabIndex = 0;
            // 
            // LogConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 262);
            this.Controls.Add(this.tb_LogContent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogConsole";
            this.Text = "Minary - LogConsole";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogConsole_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_LogContent;
    }
}