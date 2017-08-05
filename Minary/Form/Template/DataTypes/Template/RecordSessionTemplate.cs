namespace Minary.Form.Template.DataTypes.Template
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Runtime.Serialization;
  using System.Runtime.Serialization.Formatters.Binary;


  [Serializable]
  public class RecordMinaryTemplate
  {

    #region MEMBERS

    private TemplateConfig templateConfig;
    private List<Plugin> plugins;
    private AttackConfig attackConfig;

    #endregion MEMBERS


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public RecordMinaryTemplate()
    {
      this.templateConfig = new TemplateConfig();
      this.plugins = new List<Plugin>();
      this.attackConfig = new AttackConfig();
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
      this.templateConfig = new TemplateConfig(name, description, author, reference, version);
      this.plugins = new List<Plugin>();
      this.attackConfig = new AttackConfig(scanNetwork, numberSelectedTargetSystems, startAttacking);
    }

    #endregion PUBLIC


    #region PROPERTIES

    public TemplateConfig TemplateConfig
    {
      get { return this.templateConfig; }
      set { this.templateConfig = value; }
    }


    public AttackConfig AttackConfig
    {
      get { return this.attackConfig; }
      set { this.attackConfig = value; }
    }


    public List<Plugin> Plugins
    {
      get { return this.plugins; }
      set { this.plugins = value; }
    }

    #endregion PROPERTIES

  }
}