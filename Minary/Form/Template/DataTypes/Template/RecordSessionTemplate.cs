namespace Minary.Form.Template.DataTypes.Template
{
  using System;
  using System.Collections.Generic;


  [Serializable]
  public class RecordMinaryTemplate
  {

    #region PROPERTIES

    public TemplateConfig TemplateConfig { get; set; } = new TemplateConfig();

    public AttackConfig AttackConfig { get; set; } = new AttackConfig();

    public List<Plugin> Plugins { get; set; } = new List<Plugin>();

    #endregion PROPERTIES


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public RecordMinaryTemplate()
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="author"></param>
    /// <param name="reference"></param>
    /// <param name="version"></param>
    /// <param name="scanNetwork"></param>
    /// <param name="numberSelectedTargetSystems"></param>
    /// <param name="startAttacking"></param>
    public RecordMinaryTemplate(string name, string description, string author, string reference, string version, int scanNetwork, int numberSelectedTargetSystems, int startAttacking)// : base()
    {
      this.TemplateConfig = new TemplateConfig(name, description, author, reference, version);
      this.Plugins = new List<Plugin>();
      this.AttackConfig = new AttackConfig(scanNetwork, numberSelectedTargetSystems, startAttacking);
    }

    #endregion PUBLIC

  }
}