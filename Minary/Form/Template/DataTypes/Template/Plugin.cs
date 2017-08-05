namespace Minary.Form.Template.DataTypes.Template
{
  using MinaryLib.DataTypes;
  using System;


  [Serializable]
  public class Plugin
  {

    #region MEMBERS

    private string pluginName;
    private TemplatePluginData pluginData;

    #endregion MEMBERS


    #region PROPERTIES

    public string Name { get { return this.pluginName; } set { this.pluginName = value; } }
 
    public TemplatePluginData Data { get { return this.pluginData; } set { this.pluginData = value; } }

    #endregion PROPERTIES


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="Plugin"/> class.
    ///
    /// </summary>
    public Plugin()
    {
      this.pluginName = string.Empty;
      this.pluginData = new TemplatePluginData();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="pluginData"></param>
    public Plugin(string pluginName, TemplatePluginData pluginData)
    {
      this.pluginName = pluginName;
      this.pluginData = pluginData;
    }

    #endregion PUBLIC

  }
}
