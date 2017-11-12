namespace Minary.Domain.State
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Form;
  using System;


  public class StateOk : IMinaryState
  {

    #region MEMBERS

    private MinaryMain minaryObj;

    #endregion


    #region PUBLIC

    public StateOk(MinaryMain minaryObj)
    {
      this.minaryObj = minaryObj;
      this.CurrentState = MinaryState.StateOk;
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
      MessageDialog.Inst.ShowInformation(string.Empty, message, this.minaryObj);
    }


    public void Bt_ScanLan_Click_Event(object sender, EventArgs e)
    {
      string message = string.Format("Status OK");
      MessageDialog.Inst.ShowInformation(string.Empty, message, this.minaryObj);
    }


    public void LoadState()
    {
      this.minaryObj.Bt_Attack_Click = this.minaryObj.Bt_Attack_Click_Event;
      this.minaryObj.Bt_ScanLan_Click = this.minaryObj.Bt_ScanLan_Click_Event;
      this.minaryObj.NetworkHandler.LastState = LastConnectionState.Connected;
    }

    #endregion
  }
}
