namespace MinaryLib.Plugin
{
  using MinaryLib.DataTypes;
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;


  /// <summary>
  /// Interface that has to be implemented by a plugin
  /// to offer full functionality to the host application.
  /// </summary>
  public interface IPlugin
  {

    Control PluginControl { get; }

    PluginProperties Config { get; set; }

    TemplatePluginData OnGetTemplateData();

    void OnLoadTemplateData(TemplatePluginData templateData);

    void OnUnloadTemplateData();

    void SetTargets(List<Tuple<string, string, string>> targetList);

    void OnNewData(string data);

    void OnResetPlugin();

    void OnInit();

    void OnStartUpdate();

    void OnShutDown();

    void OnStartAttack();

    void OnStopAttack();

  }
}
