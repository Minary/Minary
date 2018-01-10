namespace Minary.Common
{
  using Minary.DataTypes.Enum;
  using Minary.LogConsole.Main;
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Reflection;
  using System.Windows.Forms;


  public class MinaryProcess
  {

    #region PUBLIC

    /// <summary>
    ///
    /// </summary>
    public void HandleRunningInstances()
    {
      string[] processNameArray =
        {
                                    Config.ApeProcessName,
                                    Config.SnifferProcessName,
                                    Config.HttpReverseProxyName,
                                    Assembly.GetExecutingAssembly().GetName().Name
        };

      foreach (var tmpProcName in processNameArray)
      {
        try
        {
          this.HandleProcess(tmpProcName);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $@"An error occured while handling running instance of {tmpProcName}:\r\n{ex.Message}\r\n");
        }
      }
    }

    #endregion


    #region PRIVATE

    private void HandleProcess(string processName)
    {
      var runningMinaryInstances = Process.GetProcessesByName(processName);
      var applicationPid = Process.GetCurrentProcess().Id;

      // Abort if no process matched the search string.
      if (runningMinaryInstances == null || 
          runningMinaryInstances.Length <= 0)
      {
        return;
      }

      // Convert proccess array to process list and remove the own PID 
      // from the process list
      var processList = new List<Process>(runningMinaryInstances);
      processList = processList.FindAll(elem => elem.Id != applicationPid);

      // Abort if no process matched the search string
      if (processList == null || processList.Count <= 0)
      {
        return;
      }

      // Handle processes that matched the search string
      var message = $"An instance of {processName} instance is running. Do you want to stop that process?";
      if (MessageBox.Show(message, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
      {
        return;
      }

      foreach (Process proc in processList)
      {
        Utils.TryExecute2(proc.Kill);
      }
    }

    #endregion

  }
}
