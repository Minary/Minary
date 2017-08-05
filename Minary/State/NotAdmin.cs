namespace Minary.State
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using System;
  using System.Windows.Forms;


  public class NotAdmin : IMinaryState
  {

    #region PUBLIC

    public NotAdmin(MinaryState state)
    {
      this.CurrentState = state;
    }

    #endregion


    #region INTERFACE: IMinaryState

    public MinaryState CurrentState { get; set; }


    public void Bt_Attack_Click(object sender, EventArgs e)
    {
      string message = string.Format("Can't start Minary because of missing admin privileges.");
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }


    public void Bt_ScanLan_Click(object sender, EventArgs e)
    {
      string message = string.Format("Can't scan network because because of missing admin privileges.");
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    #endregion
  }
}

