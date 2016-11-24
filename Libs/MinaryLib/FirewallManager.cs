namespace Minary.FirewallManager
{
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;


  public class NetshFirewallManager
  {

    #region PUBLIC METHODS

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="portNumber"></param>
    public void CreateFirewallRulePort(string ruleName, int portNumber)
    {
      string netshPortCommandstring = string.Format(@"advfirewall firewall add rule name=""{0}"" dir=in action=allow protocol=tcp localport={1}", ruleName, portNumber);
      this.RunNetsh(netshPortCommandstring);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="ruleName"></param>
    /// <param name="binaryPath"></param>
    public void CreateFirewallRuleApplication(string ruleName, string binaryPath)
    {
      string netshApplicationCommandstring = string.Format(@"advfirewall firewall add rule name=""{0}"" dir=in action=allow program=""{1}""", ruleName, binaryPath);
      this.RunNetsh(netshApplicationCommandstring);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="firewallRuleName"></param>
    public void DeleteFirewallRule(string firewallRuleName)
    {
      string netshApplicationCommandstring = string.Format(@"advfirewall firewall delete rule name=""{0}"" ", firewallRuleName);
      this.RunNetsh(netshApplicationCommandstring);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="firewallRuleName"></param>
    /// <returns></returns>
    public bool FirewallRuleExists(string firewallRuleName)
    {
      bool retVal = false;
      ProcessStartInfo procNetsh = new ProcessStartInfo();
      string netshArguments = string.Format(@"advfirewall firewall show rule name=""{0}""", firewallRuleName);

      // Open incoming port for application
      procNetsh.FileName = "netsh.exe";
      procNetsh.Arguments = netshArguments;
      procNetsh.UseShellExecute = false;
      procNetsh.CreateNoWindow = true;
      procNetsh.WindowStyle = ProcessWindowStyle.Normal;
      procNetsh.RedirectStandardOutput = true;

      using (Process process = Process.Start(procNetsh))
      {
        using (StreamReader reader = process.StandardOutput)
        {
          string line = string.Empty;
          List<string> outputLines = new List<string>();

          while (!process.StandardOutput.EndOfStream)
          {
            line = reader.ReadLine();
            outputLines.Add(line);
          }

          foreach (string tmpLine in outputLines)
          {
            if (tmpLine.ToLower().Contains(firewallRuleName.ToLower()))
            {
              retVal = true;
              break;
            }
          }
        }
      }

      return retVal;
    }

    #endregion


    #region PRIVATE METHODS

    /// <summary>
    ///
    /// </summary>
    /// <param name="pNetshArguments"></param>
    private void RunNetsh(string netshArguments)
    {
      Process procNetsh = new Process();

      // Open incoming port for application
      procNetsh.StartInfo.FileName = "netsh.exe";
      procNetsh.StartInfo.Arguments = netshArguments;
      procNetsh.StartInfo.UseShellExecute = false;
      procNetsh.StartInfo.RedirectStandardOutput = true;
      procNetsh.StartInfo.CreateNoWindow = true;
      procNetsh.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

      procNetsh.Start();

      if (procNetsh.WaitForExit(2000) == false)
      {
        procNetsh.Kill();
      }
    }

    #endregion

  }
}