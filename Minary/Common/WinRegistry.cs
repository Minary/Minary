namespace Minary.Common
{
  using Microsoft.Win32;
  using System;


  public class WinRegistry
  {

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
