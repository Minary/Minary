namespace Minary.Form.Template.DataTypes.Template
{
  using System;


  [Serializable]
  public class AttackConfig
  {

    #region PROPERTIES

    public int ScanNetwork { get; set; } = 1;

    public int NumberSelectedTargetSystems { get; set; } = 5;

    public int StartAttack { get; set; } = 1;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public AttackConfig()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="scanNetwork"></param>
    /// <param name="numberSelectedTargetSystems"></param>
    /// <param name="startAttack"></param>
    public AttackConfig(int scanNetwork, int numberSelectedTargetSystems, int startAttack)
    {
      this.ScanNetwork = scanNetwork;
      this.NumberSelectedTargetSystems = numberSelectedTargetSystems;
      this.StartAttack = startAttack;
    }

    #endregion

  }
}
