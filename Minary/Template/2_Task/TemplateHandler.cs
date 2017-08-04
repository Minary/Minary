namespace Minary.Template.Task
{
  using Minary.Template.DataTypes.Template;
  using MinaryLib.Plugin;
  using System;


  public class TemplateHandler
  {

    #region MEMBERS
    
    private Infrastructure.TemplateHandler infrastructure;
    private Minary.MinaryMain minaryMain;

    #endregion


    #region PROPERTIES

    #endregion


    #region PUBLIC

    public TemplateHandler(Minary.MinaryMain minaryMain)
    {
      this.minaryMain = minaryMain;
      this.infrastructure = new Infrastructure.TemplateHandler();
    }


    public void RemoveAllTemplatePatternsFromPlugins()
    {
      foreach (string tmpPluginName in this.minaryMain.PluginHandler.TabPagesCatalog.Keys)
      {
        try
        {
          IPlugin tmpPluginObj = this.minaryMain.PluginHandler.TabPagesCatalog[tmpPluginName].PluginObject;
          tmpPluginObj.OnUnloadTemplateData();
        }
        catch (Exception ex)
        {
          this.minaryMain.LogConsole.LogMessage("TemplateHandler: {0}", ex.Message);
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="templateObj"></param>
    /// <param name="outputFilePath"></param>
    public void SaveAttackTemplate(RecordMinaryTemplate templateObj, string outputFilePath)
    {
      // Verify if objects were instantiated
      if (templateObj == null)
      {
        throw new Exception("Something is wrong with the template");
      }

      if (templateObj.TemplateConfig == null)
      {
        throw new Exception("Something is wrong with the template configuration");
      }

      if (templateObj == null || templateObj.Plugins == null)
      {
        throw new Exception("Something is wrong with the plugin data");
      }

      // Verify passed values
      if (templateObj.Plugins.Count == 0)
      {
        throw new Exception("You must activate at least one plugin to create a valid template");
      }

      if (string.IsNullOrEmpty(templateObj.TemplateConfig.Name))
      {
        throw new Exception("Something is wrong with the template name");
      }

      if (string.IsNullOrEmpty(templateObj.TemplateConfig.Description))
      {
        throw new Exception("Something is wrong with the template description");
      }

      if (string.IsNullOrEmpty(templateObj.TemplateConfig.Reference))
      {
        throw new Exception("Something is wrong with the template reference URL");
      }

      if (string.IsNullOrEmpty(templateObj.TemplateConfig.Author))
      {
        throw new Exception("Something is wrong with the author name");
      }

      if (string.IsNullOrEmpty(templateObj.TemplateConfig.Timestamp))
      {
        throw new Exception("Something is wrong with the time stamp");
      }

      if (string.IsNullOrEmpty(outputFilePath))
      {
        throw new Exception("Something is wrong with the output file");
      }

      this.infrastructure.SaveTemplate(templateObj, outputFilePath);
    }


    public bool IsFileATemplate(string filePath)
    {
      return this.infrastructure.IsFileATemplate(filePath);
    }


    #endregion

  }
}
