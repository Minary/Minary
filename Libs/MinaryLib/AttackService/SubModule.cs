namespace MinaryLib.AttackService
{
  public class SubModule
  {

    #region MEMBERS

    public string ModuleName { get; set; }
    public string WorkingDirectory { get; set; }
    public string ConfigFilePath { get; set; }

    #endregion


    #region PUBLIC

    public SubModule(string moduleName, string workingDirectory, string configFilePath)
    {
      ModuleName = moduleName;
      WorkingDirectory = workingDirectory;
      ConfigFilePath = configFilePath;
    }

    #endregion

  }
}
