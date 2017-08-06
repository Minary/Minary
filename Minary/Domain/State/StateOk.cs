namespace Minary.Domain.State
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using System;
  using System.Windows.Forms;


  public class StateOk : IMinaryState
  {

    #region PUBLIC

    public StateOk(MinaryState state)
    {
      this.CurrentState = state;
      this.Bt_Attack_Click += this.Bt_Attack_Click_Event;
      this.Bt_ScanLan_Click += this.Bt_ScanLan_Click_Event;
    }

    #endregion


    #region INTERFACE: IMinaryState

    public MinaryState CurrentState { get; set; }

    public Bt_ScanLan_Click_Delegate Bt_ScanLan_Click { get; set; }

    public Bt_Attack_Click_Delegate Bt_Attack_Click { get; set; }


    public void Bt_Attack_Click_Event(object sender, EventArgs e)
    {
      string message = string.Format("Status OK");
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }


    public void Bt_ScanLan_Click_Event(object sender, EventArgs e)
    {
      string message = string.Format("Status OK");
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    #endregion
  }
}
