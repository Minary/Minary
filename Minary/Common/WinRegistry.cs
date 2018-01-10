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
    public static string GetValue(string regKeyName, string regSubValueName)
    {
      RegistryKey regKey = Registry.CurrentUser;
      var retVal = string.Empty;

      try
      {
        regKey = regKey.CreateSubKey($"Software\\{Minary.Config.ApplicationName}\\{regKeyName}");
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
    public static bool CreateKey(string parentKey, string regKeyName)
    {
      RegistryKey regKey = Registry.CurrentUser;
      var retVal = false;

      try
      {
        regKey = regKey.CreateSubKey($"Software\\{parentKey}\\{regKeyName}");
        regKey.Close();
        retVal = true;
      }
      catch (Exception)
      {
      }

      return retVal;
    }


    public static bool CreateOrUpdateValue(string keyName, string subValueName, string valueContent)
    {
      var retVal = false;
      RegistryKey regKey = null;

      try
      {
        regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(keyName);
        regKey.SetValue(subValueName, valueContent);
        retVal = true;
      }
      catch (Exception)
      {
      }
      finally
      {
        if (regKey != null)
        {
          regKey.Close();
        }
      }

      return retVal;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="regKeyName"></param>
    /// <param name="regSubValueName"></param>
    /// <param name="regValueContent"></param>
    public static void SetValue(string regKeyName, string regSubValueName, string regValueContent)
    {
      RegistryKey regKey = Registry.CurrentUser;

      try
      {
        regKey = regKey.CreateSubKey($"Software\\{Minary.Config.ApplicationName}\\{regKeyName}");
        regKey.SetValue(regSubValueName, regValueContent, RegistryValueKind.String);
        regKey.Close();
      }
      catch (Exception)
      {
      }
    }

  }
}
