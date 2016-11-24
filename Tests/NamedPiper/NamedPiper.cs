namespace NamedPiper
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.IO.Pipes;
  using System.Windows.Forms;

  public partial class NamedPiper : Form
  {

    #region MEMBERS

    private NamedPipeClientStream namedPipeClient;
    private StreamWriter streamWriter;

    #endregion


    #region PUBLIC

    public NamedPiper()
    {
      this.InitializeComponent();

      BindingSource defaultData = new BindingSource();
      defaultData.DataSource = new List<string> {
        "TCP||00-11-22-33-44-55||192.168.10.15||4321||88.12.12.13||80||GET / HTTP/1.1..Host: www.buglist.io....",
        "DNSREQ||00-11-22-33-44-55||192.168.10.15||4321||88.12.12.13||53||www.buglist.io",
        "DNSREP||00-11-22-33-44-55||192.168.10.15||4321||88.12.12.13||53||www.buglist.io"
        };
      this.cb_Data.DataSource = defaultData;
    }

    #endregion


    #region PRIVATE

    private void BT_Close_Click(object sender, EventArgs e)
    {
      this.Dispose();
    }

    private void BT_Send_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.tb_PipeName.Text))
      {
        MessageBox.Show("No pipe name defined", "Error");
      }
      else
      {
        string data = this.cb_Data.SelectedItem.ToString();
        this.WriteToPipe(data);
      }
    }

    public bool WriteToPipe(string writeData)
    {
      bool retVal = false;

      if (writeData == null)
      {
        return false;
      }

      if (writeData.Length <= 0)
      {
        return true;
      }

      // Create Pipe
      try
      {

        // Initialize pipe
        if (this.namedPipeClient == null)
        {
          this.namedPipeClient = new NamedPipeClientStream("Minary");
        }

        if (!this.namedPipeClient.IsConnected)
        {
          this.namedPipeClient.Connect(500);
        }

        if (this.namedPipeClient == null || !this.namedPipeClient.IsConnected)
        {
          return false;
        }

        if (this.streamWriter == null)
        {
          this.streamWriter = new StreamWriter(this.namedPipeClient);
          this.streamWriter.AutoFlush = true;
        }

        if (this.streamWriter != null &&
            this.namedPipeClient.IsConnected &&
            this.namedPipeClient.CanWrite)
        {
          string writeBuffer = writeData.Trim();
          this.streamWriter.WriteLine(writeBuffer);

          retVal = true;
        }
        else
        {
          this.namedPipeClient = null;
          this.streamWriter = null;
        }
      }
      catch (System.TimeoutException tex)
      {
        MessageBox.Show(string.Format("Timeout exception: {0}", tex.Message), "Error");
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format("Exception: {0}", ex.Message), "Error");
      }

      return retVal;
    }

    #endregion

  }
}
