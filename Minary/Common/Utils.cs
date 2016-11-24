namespace Minary.Common
{
  using Microsoft.Win32;
  using System;
  using System.Text.RegularExpressions;

  public class Utils
  {


    /// <summary>
    ///
    /// </summary>
    /// <param name="pAppName"></param>
    /// <returns></returns>
    public static string IsProgrammInstalled(string appName)
    {
      string retVal = string.Empty;

      // The registry tmpKeyKey:
      string[] softwareDirs = new string[2] {
                                              @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                                              @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
                                           };

      foreach (string directory in softwareDirs)
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
                  string softwareName = regSubKey.GetValue("DisplayName").ToString();

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



    /// <summary>
    ///
    /// </summary>
    /// <param name="regKeyName"></param>
    /// <param name="regSubValueName"></param>
    /// <returns></returns>
    public static string GetRegistryValue(string regKeyName, string regSubValueName)
    {
      RegistryKey regKey = Registry.CurrentUser;
      string retVal = string.Empty;

      try
      {
        regKey = regKey.CreateSubKey(string.Format("Software\\{0}\\{1}", Minary.Config.ApplicationName, regKeyName));
        retVal = (string)regKey.GetValue(regSubValueName);
        regKey.Close();
      }
      catch (Exception)
      {
      }

      return retVal;
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="parentKey"></param>
    /// <param name="regKeyName"></param>
    /// <returns></returns>
    public static bool CreateRegistryKey(string parentKey, string regKeyName)
    {
      RegistryKey regKey = Registry.CurrentUser;
      bool retVal = false;

      try
      {
        regKey = regKey.CreateSubKey(string.Format("Software\\{0}\\{1}", parentKey, regKeyName));
        regKey.Close();
        retVal = true;
      }
      catch (Exception)
      {
      }

      return retVal;
    }



    /// <summary>
    ///
    /// </summary>
    /// <param name="regKeyName"></param>
    /// <param name="regSubValueName"></param>
    /// <param name="regValueContent"></param>
    public static void SetRegistryValue(string regKeyName, string regSubValueName, string regValueContent)
    {
      RegistryKey regKey = Registry.CurrentUser;

      try
      {
        regKey = regKey.CreateSubKey(string.Format("Software\\{0}\\{1}", Minary.Config.ApplicationName, regKeyName));
        regKey.SetValue(regSubValueName, regValueContent, RegistryValueKind.String);
        regKey.Close();
      }
      catch (Exception)
      {
      }
    }
  }
}
