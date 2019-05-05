namespace Minary.Common
{
  using System;


  public class NPcap
  {

    #region PUBLIC


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string GetNPcapVersion()
    {
      var retVal = string.Empty;

      try
      {
        retVal = Config.NPcap = Utils.IsProgrammInstalled("npcap").ToLower();
      }
      catch (Exception)
      {
        throw new Exception($"NPcap is not present and { Config.ApplicationName} probably won't work as expected. You can download NPcap under https://nmap.org/npcap/#download");
      }

      if (Config.NPcap == null || Config.NPcap.Length <= 0)
      {
        throw new Exception($"NPcap is not present and {Config.ApplicationName} probably won't work as expected. You can download NPcap under https://nmap.org/npcap/#download");
      }

      return retVal;
    }

    #endregion

  }
}
