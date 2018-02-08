namespace Minary.Form.Template.DataTypes.Template
{
  using System;
  using System.Collections.Generic;


  [Serializable]
  public class MinaryTemplateData
  {

    #region PROPERTIES

    public TemplateConfig TemplateConfig { get; set; } = new TemplateConfig();

    public AttackConfig AttackConfig { get; set; } = new AttackConfig();

    public List<Plugin> Plugins { get; set; } = new List<Plugin>();

    #endregion PROPERTIES


    #region PUBLIC

    public MinaryTemplateData()
    {
    }


    public MinaryTemplateData(string name, string description, string author, string reference, string version, int scanNetwork, int numberSelectedTargetSystems, int startAttacking)// : base()
    {
      this.TemplateConfig = new TemplateConfig(name, description, author, reference, version);
      this.Plugins = new List<Plugin>();
      this.AttackConfig = new AttackConfig(scanNetwork, numberSelectedTargetSystems, startAttacking);
    }

    #endregion PUBLIC

  }
}