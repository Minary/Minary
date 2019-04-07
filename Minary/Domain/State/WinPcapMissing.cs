namespace Minary.Domain.State
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Form.Main;
  using System;


  public class WinPcapMissing : IMinaryState
  {

    #region MEMBERS

    private MinaryMain minaryObj;

    #endregion

    #region PUBLIC

    public WinPcapMissing(MinaryMain minaryObj)
    {
      this.minaryObj = minaryObj;
      this.CurrentState = MinaryState.WinPcapMissing;
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
      throw new Exception("Can't start Minary because WinPcap is not installed.");
    }


    public void Bt_ScanLan_Click_Event(object sender, EventArgs e)
    {
      throw new Exception("Can't scan network because WinPcap is not installed.");
    }


    public void LoadState()
    {
      this.minaryObj.Bt_Attack_Click = this.Bt_Attack_Click;
      this.minaryObj.Bt_ScanLan_Click = this.Bt_ScanLan_Click;
    }

    #endregion

  }
}
