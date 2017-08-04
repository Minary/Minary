namespace Minary.Common
{
  using System;

  public class WinPcap
  {

    #region PUBLIC


    /// <summary>
    /// Initializes a new instance of the <see cref="WinPcap"/> class.
    ///
    /// </summary>
    public WinPcap()
    {
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetWinPcapVersion()
    {
      string retVal = string.Empty;

      try
      {
        retVal = Config.WinPcap = Utils.IsProgrammInstalled("winpcap").ToLower();
      }
      catch (Exception)
      {
        throw new Exception(string.Format("WinPcap is not present and {0} probably won't work as expected. You can download WinPcap under http://www.winpcap.org", Config.ApplicationName));
      }

      if (Config.WinPcap == null || Config.WinPcap.Length <= 0)
      {
        throw new Exception(string.Format("WinPcap is not present and {0} probably won't work as expected. You can download WinPcap under http://www.winpcap.org", Config.ApplicationName));
      }

      return retVal;
    }

    #endregion

  }
}
