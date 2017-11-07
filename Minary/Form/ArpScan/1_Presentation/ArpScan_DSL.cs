namespace Minary.Form.ArpScan.Presentation
{
  using Minary.DataTypes.Struct;
  using Minary.Form.ArpScan.DataTypes;
  using System;


  public partial class ArpScan
  {

    #region DSL
    
    public int NumberTargetSystems()
    {
      return this.dgv_Targets.Rows.Count;
    }


    public void StopRunningArpScan()
    {
      this.SetArpScanGuiOnStopped();
    }


    public void SelectRandomSystems(int noTargetSystems)
    {
      for (int i = 0; i < this.dgv_Targets.Rows.Count && i < noTargetSystems; i++)
      {
        try
        {
          this.dgv_Targets.Rows[i].Cells["Attack"].Value = true;
        }
        catch
        {
        }
      }
    }

    #endregion

  }
}
