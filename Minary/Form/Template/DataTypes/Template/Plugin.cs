namespace Minary.Form.Template.DataTypes.Template
{
  using MinaryLib.DataTypes;
  using System;


  [Serializable]
  public class Plugin
  {

    #region PROPERTIES

    public string Name { get; set; } = string.Empty;
 
    public TemplatePluginData Data { get; set; } = new TemplatePluginData();

    #endregion PROPERTIES


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginName"></param>
    /// <param name="pluginData"></param>
    public Plugin(string pluginName, TemplatePluginData pluginData)
    {
      this.Name = pluginName;
      this.Data = pluginData;
    }

    #endregion PUBLIC

  }
}
