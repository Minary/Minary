namespace Minary.Form.Template.Task
{
  using Minary.DataTypes.Enum;
  using Minary.Form.Template.DataTypes.Template;
  using Minary.LogConsole.Main;
  using MinaryLib.Plugin;
  using System;


  public class TemplateHandler
  {

    #region MEMBERS
    
    private Infrastructure.TemplateHandler infrastructure = new Infrastructure.TemplateHandler();
    private Minary.Form.Main.MinaryMain minaryMain;

    #endregion
    

    #region PUBLIC

    public TemplateHandler(Minary.Form.Main.MinaryMain minaryMain)
    {
      this.minaryMain = minaryMain;
    }


    public void UnloadTemplatePatternsFromPlugins()
    {
      foreach (var tmpPluginName in this.minaryMain.PluginHandler.TabPagesCatalog.Keys)
      {
        try
        {
          // This could be done in one method call but 
          // it is nested too deeply :/
          var tabPageCatalog = this.minaryMain.PluginHandler.TabPagesCatalog;
          tabPageCatalog[tmpPluginName].PluginObject.OnUnloadTemplateData();
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"TemplateHandler: {ex.Message}");
        }
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="templateObj"></param>
    /// <param name="outputFilePath"></param>
    public void SaveAttackTemplate(MinaryTemplateData templateObj, string outputFilePath)
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
