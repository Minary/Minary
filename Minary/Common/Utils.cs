namespace Minary.Common
{
  using Microsoft.Win32;
  using System;
  using System.Text.RegularExpressions;


  public class Utils
  {

    public static string IsProgrammInstalled(string appName)
    {
      var retVal = string.Empty;

      // The registry tmpKeyKey:
      var softwareDirs = new string[2] {
                                        @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                                        @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
                                       };

      foreach (var directory in softwareDirs)
      {
        using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(directory))
        {
          // Let's go through the registry keys and get the info we need:
          foreach (string subKeyName in regKey.GetSubKeyNames())
          {
            using (RegistryKey regSubKey = regKey.OpenSubKey(subKeyName))
            {
              try
              {
                if (regSubKey.GetValue("DisplayName") != null)
                {
                  var softwareName = regSubKey.GetValue("DisplayName").ToString();

                  if (Regex.Match(softwareName, appName, RegexOptions.IgnoreCase).Success)
                  {
                    retVal = softwareName;
                    goto END;
                  }
                }
              }
              catch (Exception)
              {
              }
            }
          }
        }
      }

END:

      return retVal;
    }

 
    public static string TryExecute(Func<string> action)
    {
      try
      {
        return action();
      }
      catch (Exception)
      {
      }

      return string.Empty;
    }


    public static void TryExecute2(Action action)
    {
      try
      {
        action();
      }
      catch (Exception)
      {
      }
    }

  }
}
