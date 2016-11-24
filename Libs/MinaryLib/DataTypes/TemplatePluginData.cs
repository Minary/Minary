namespace MinaryLib.DataTypes
{
  using System;


  [Serializable]
  public class TemplatePluginData
  {

    #region MEMBERS

    private byte[] pluginDataSearchPatternItems;
    private byte[] pluginConfigurationItems;

    #endregion


    #region PROPERTIES

    public byte[] PluginDataSearchPatternItems { get { return this.pluginDataSearchPatternItems; } set { this.pluginDataSearchPatternItems = value; } }

    public byte[] PluginConfigurationItems { get { return this.pluginConfigurationItems; } set { this.pluginConfigurationItems = value; } }

    #endregion


    #region PUBLIC

    public TemplatePluginData()
    {
    }

    public TemplatePluginData(byte[] pluginDataSearchPatternItems, byte[] pluginConfigurationItems)
    {
      this.pluginDataSearchPatternItems = pluginDataSearchPatternItems;
      this.pluginConfigurationItems = pluginConfigurationItems;
    }

    #endregion

  }
}
