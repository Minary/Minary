namespace MinaryLib.Plugin
{
  using MinaryLib.AttackService;
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;


  /// <summary>
  /// The interface a host application hast to implement to
  /// offer the required functionality to the loaded plugins.
  /// </summary>
  public interface IPluginHost
  {

    #region PROPERTIES

    bool IsDebuggingOn { get; }

    string CurrentInterface { get; }

    string StartIP { get; }

    string StopIP { get; }

    string CurrentIP { get; }

    List<Tuple<string, string, string>> ReachableSystemsList { get; } // MAC, IP, Vendor

    string HostWorkingDirectory { get; }

    Dictionary<string, IAttackService> AttackServiceList { get; }

    Form MainWindowForm { get; }

    #endregion


    #region PUBLIC

    void LogMessage(string message, params object[] formatArgs);

    void Register(IPlugin ipi);

    void ReportPluginSetStatus(object pluginObj, MinaryLib.Plugin.Status status);

    #endregion

  }
}