namespace Minary.Form
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using System;
  using System.Windows.Forms;


  public partial class MinaryMain : IMinaryState
  {

    #region INTERFACE: IMinaryState

    public MinaryState CurrentState { get; set; }

    public Bt_ScanLan_Click_Delegate Bt_ScanLan_Click { get; set; }

    public Bt_Attack_Click_Delegate Bt_Attack_Click { get; set; }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    delegate void ScanLanClickDelegate(object sender, EventArgs e);
    private void Bt_ScanLan_Click_Event(object sender, EventArgs e)
    {
      if (this.cb_Interfaces.SelectedIndex < 0)
      {
        MessageBox.Show("No network interface selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      this.arpScanHandler.ShowArpScanGui(ref this.targetList);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    delegate void AttackClickDelegate(object sender, EventArgs e);
    private void Bt_Attack_Click_Event(object sender, EventArgs e)
    {
      try
      {
        this.StartAttacksOnBackground();
      }
      catch (Exception ex)
      {
        string message = string.Format("The following error occurred while initiating attack procedures:\r\n\r\n{0}", ex.Message);
        MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    #endregion

  }
}
