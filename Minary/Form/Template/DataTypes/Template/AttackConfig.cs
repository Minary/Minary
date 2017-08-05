namespace Minary.Form.Template.DataTypes.Template
{
  using System;


  [Serializable]
  public class AttackConfig
  {

    #region MEMBERS

    private int scanNetwork;
    private int numberSelectedTargetSystems;
    private int startAttack;

    #endregion


    #region PROPERTIES

    public int ScanNetwork { get { return this.scanNetwork; } set { this.scanNetwork = value; } }

    public int NumberSelectedTargetSystems { get { return this.numberSelectedTargetSystems; } set { this.numberSelectedTargetSystems = value; } }

    public int StartAttack { get { return this.startAttack; } set { this.startAttack = value; } }

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public AttackConfig()
    {
      this.scanNetwork = 1;
      this.numberSelectedTargetSystems = 5;
      this.startAttack = 1;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="scanNetwork"></param>
    /// <param name="numberSelectedTargetSystems"></param>
    /// <param name="startAttack"></param>
    public AttackConfig(int scanNetwork, int numberSelectedTargetSystems, int startAttack)
    {
      this.scanNetwork = scanNetwork;
      this.numberSelectedTargetSystems = numberSelectedTargetSystems;
      this.startAttack = startAttack;
    }

    #endregion

  }
}
