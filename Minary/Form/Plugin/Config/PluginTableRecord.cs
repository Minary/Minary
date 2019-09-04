namespace Minary
{
  using System.ComponentModel;

  public class PluginTableRecord : INotifyPropertyChanged
  {

    #region MEMBERS

    private string pluginName;
    private string pluginType;
    private string attackService;
    private string pluginDescription;
    private string active;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    public PluginTableRecord(string pluginName, string pluginType, string attackService, string pluginDescription, string active)
    {
      this.pluginName = pluginName;
      this.pluginType = pluginType;
      this.attackService = attackService;
      this.pluginDescription = pluginDescription;
      this.active = active;
    }


    public string PluginName
    {
      get { return this.pluginName; }
      set
      {
        this.pluginName = value;
        this.NotifyPropertyChanged("PluginName");
      }
    }


    public string PluginType
    {
      get { return this.pluginType; }
      set
      {
        this.pluginType = value;
        this.NotifyPropertyChanged("PluginType");
      }
    }


    public string AttackService
    {
      get { return this.attackService; }
      set
      {
        this.attackService = value;
        this.NotifyPropertyChanged("AttackService");
      }
    }


    public string PluginDescription
    {
      get { return this.pluginDescription; }
      set
      {
        this.pluginDescription = value;
        this.NotifyPropertyChanged("PluginDescription");
      }
    }


    public string Active
    {
      get { return this.active; }
      set
      {
        this.active = value;
        this.NotifyPropertyChanged("Active");
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyName"></param>
    private void NotifyPropertyChanged(string name)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(name));
      }
    }
  }
}
