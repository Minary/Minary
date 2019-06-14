namespace Minary.Form.GuiAdvanced
{
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.DataTypes.Struct;
  using System;


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
    public void Bt_ScanLan_Click_Event(object sender, EventArgs e)
    {
      MinaryConfig minaryConfig = this.minaryTaskFacade.GetCurrentMinaryConfig();
      this.arpScanHandler.ShowArpScanGui(this.targetList, minaryConfig);
      this.PassNewTargetListToPlugins();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Bt_Attack_Click_Event(object sender, EventArgs e)
    {
      this.StartAttacksOnBackground();
    }


    public void LoadState()
    {
    }

    #endregion

  }
}
