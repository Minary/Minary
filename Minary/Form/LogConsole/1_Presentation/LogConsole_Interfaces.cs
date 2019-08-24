namespace Minary.LogConsole.Main
{
  using Minary.DataTypes.Interface.LogConsole;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows.Forms;


  public partial class LogCons : Form, IObserver
  {


    #region INTERFACE: IObserver

    public delegate void UpdateLogDelegate(List<string> newLogMessages);
    public void UpdateLog(List<string> newLogMessages)
    {
      if (this.tb_LogContent.InvokeRequired)
      {
        this.tb_LogContent.BeginInvoke(new UpdateLogDelegate(this.UpdateLog), new object[] { newLogMessages });
        return;
      }

      if (newLogMessages == null ||
          newLogMessages.Count <= 0)
      {
        return;
      }

      var newLogChunk = string.Join(Environment.NewLine, newLogMessages.Where(elem => elem != null && elem.Length > 0));
      if (string.IsNullOrEmpty(newLogChunk))
      {
        return;
      }

      this.tb_LogContent.AppendText(newLogChunk);
      this.tb_LogContent.SelectionStart = this.tb_LogContent.Text.Length;
      this.tb_LogContent.ScrollToCaret();
    }

    #endregion

  }
}
