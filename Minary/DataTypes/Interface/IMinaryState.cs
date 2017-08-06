namespace Minary.DataTypes.Interface
{
  using Minary.DataTypes.Enum;
  using System;


  public delegate void Bt_ScanLan_Click_Delegate(object sender, EventArgs e);
  public delegate void Bt_Attack_Click_Delegate(object sender, EventArgs e);

  public interface IMinaryState
  {
    MinaryState CurrentState { get; set; }
    Bt_ScanLan_Click_Delegate Bt_ScanLan_Click { get; set; }
    Bt_Attack_Click_Delegate Bt_Attack_Click { get; set; }
  }
}
