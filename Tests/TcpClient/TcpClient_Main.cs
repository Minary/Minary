using System;
using System.Windows.Forms;

namespace TcpClient
{
  public partial class TcpClient_Main : Form
  {

    #region MEMBERS

    private TcpClient client = new TcpClient();

    #endregion


    #region PUBLIC

    public TcpClient_Main()
    {
      this.InitializeComponent();

      this.l_TimestampServerResponse.Text = string.Empty;
    }

    #endregion


    #region PRIVATE

    private void BT_send_Click(object sender, EventArgs e)
    {
      string host = this.tb_Host.Text;
      string port = this.tb_Port.Text;
      string data = this.tb_Data.Text;
      int realPort = -1;

      if (string.IsNullOrEmpty(host) || string.IsNullOrWhiteSpace(host))
      {
        MessageBox.Show("Host is invalid");
      }

      if (string.IsNullOrEmpty(port) || string.IsNullOrWhiteSpace(port) || !int.TryParse(port, out realPort))
      {
        MessageBox.Show("Port is invalid");
      }

      if (string.IsNullOrEmpty(data))
      {
        MessageBox.Show("Data is invalid");
      }

      this.Cursor = Cursors.WaitCursor;
      try
      {
        this.tb_Output.Text = string.Empty;
        string serverResponse = this.client.SendRequest(host, realPort, data);
        this.tb_Output.Text = serverResponse;
      }
      catch (Exception ex)
      {
        this.tb_Output.Text = string.Format("EXCEPTION: {0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
      }

      this.Cursor = Cursors.Default;
      this.l_TimestampServerResponse.Text = DateTime.Now.ToString();

      #endregion

    }

    private void BT_Cancel_Click(object sender, EventArgs e)
    {
      base.Close();
    }
  }
}
