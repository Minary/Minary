namespace Minary.Common
{
  using System;


  public class WinPcap
  {

    #region PUBLIC


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string GetWinPcapVersion()
    {
      var retVal = string.Empty;

      try
      {
        retVal = Config.WinPcap = Utils.IsProgrammInstalled("winpcap").ToLower();
      }
      catch (Exception)
      {
        throw new Exception($"WinPcap is not present and { Config.ApplicationName} probably won't work as expected. You can download WinPcap under http://www.winpcap.org");
      }

      if (Config.WinPcap == null || Config.WinPcap.Length <= 0)
      {
        throw new Exception($"WinPcap is not present and {Config.ApplicationName} probably won't work as expected. You can download WinPcap under http://www.winpcap.org");
      }

      return retVal;
    }

    #endregion

  }
}
