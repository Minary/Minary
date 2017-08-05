namespace Minary.State
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using System;
  using System.Windows.Forms;


  public class NetworkMissing : IMinaryState
  {

    #region PUBLIC

    public NetworkMissing(MinaryState state)
    {
      this.CurrentState = state;
    }

    #endregion


    #region INTERFACE: IMinaryState

    public MinaryState CurrentState { get; set; }


    public void Bt_Attack_Click(object sender, EventArgs e)
    {
      string message = string.Format("Can't start Minary because no network interface was found.");
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }


    public void Bt_ScanLan_Click(object sender, EventArgs e)
    {
      string message = string.Format("Can't scan network because no network interface was found.");
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    #endregion
  }
}

