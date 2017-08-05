namespace Minary.DataTypes.Interface
{
  using Minary.DataTypes.Enum;
  using System;


  public interface IMinaryState
  {
    MinaryState CurrentState { get; set; }

    void Bt_ScanLan_Click(object sender, EventArgs e);
    void Bt_Attack_Click(object sender, EventArgs e);
  }
}
