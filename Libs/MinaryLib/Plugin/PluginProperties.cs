namespace MinaryLib
{
  using MinaryLib.DataTypes;
  using MinaryLib.Plugin;
  using System.Collections.Generic;


  /// <summary>
  /// Properties that must to be defined in a
  /// plugin.
  /// </summary>
  public class PluginProperties
  {
    public string PluginName { get; set; }

    public string PluginType { get; set; }

    public string PluginDescription { get; set; }

    public Dictionary<int, IpProtocols> Ports { get; set; }

    public string ApplicationBaseDir { get; set; }

    public string PluginBaseDir { get; set;  }

    public string PatternSubDir { get; set; }

    public IPluginHost HostApplication { get; set; }

  }
}
