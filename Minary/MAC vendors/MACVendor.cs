using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;



namespace Simsang.MACVendors
{
  public class MACVendor
  {

    #region MEMBERS

    private static MACVendor cInstance;
    private String cMACVendorList = @"data\MACVendors.txt";
    private Hashtable cMACVendorMap;

    #endregion


    #region PUBLIC

    /// <summary>
    /// 
    /// </summary>
    public MACVendor()
    {
      /*
       * Load MAC vendor list
       */
      cMACVendorMap = new Hashtable();
      loadMACVendorList();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static MACVendor getInstance()
    {
      if (cInstance == null)
        cInstance = new MACVendor();


      return (cInstance);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMAC"></param>
    /// <returns></returns>
    public String getVendorByMAC(String pMAC)
    {
      String lRetVal = String.Empty;
      String lVendor = String.Empty;
      Match lMatch;

      if (!String.IsNullOrEmpty(pMAC))
      {

        // Determine vendor
        lVendor = String.Empty;
        if ((lMatch = Regex.Match(pMAC, @"([\da-f]{1,2})[:\-]{1}([\da-f]{1,2})[:\-]{1}([\da-f]{1,2})[:\-]{1}.*", RegexOptions.IgnoreCase)).Success)
        {
          String lOct1 = lMatch.Groups[1].Value.ToString();
          String lOct2 = lMatch.Groups[2].Value.ToString();
          String lOct3 = lMatch.Groups[3].Value.ToString();
          String lTmp = String.Format("{0}{1}{2}", lOct1, lOct2, lOct3).ToLower();

          lRetVal = cMACVendorMap.ContainsKey(lTmp) ? cMACVendorMap[lTmp].ToString() : String.Empty;
        } // if ((lMatch...     
      } // if (!String.IsNull...

      return (lRetVal);
    }

    #endregion


    #region PRIVATE

    /// <summary>
    /// 
    /// </summary>
    private void loadMACVendorList()
    {
      String lLine = String.Empty;
      StreamReader lSR = null;
      char[] lDelimiter = "\t ".ToCharArray();
      String lMAC = String.Empty;
      String lVendorName = String.Empty;

      try
      {
        lSR = new StreamReader(cMACVendorList);
        while ((lLine = lSR.ReadLine()) != null)
        {
          lLine = lLine.Trim();

          try
          {
            String[] lSplit = lLine.Split(lDelimiter, 2);


            if (lSplit.Length == 2)
            {
              lMAC = lSplit[0].ToLower();
              lVendorName = lSplit[1];
              cMACVendorMap.Add(lMAC, lVendorName);
            }
          }
          catch (Exception)
          {
            LogConsole.Main.LogConsole.pushMsg(String.Format("Unable to load MAC/Vendor pair: {0}/{1}   ({2})", lLine, lMAC, lVendorName));
          }
        }
      }
      catch (FileNotFoundException)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("{0} not found!", cMACVendorList));
      }
      catch (Exception lEx)
      {
        LogConsole.Main.LogConsole.pushMsg(String.Format("Error occurred while opening {0}: {1}", cMACVendorList, lEx.StackTrace));
      }
      finally
      {
        if (lSR != null)
          lSR.Close();
      }
    }

    #endregion

  }
}
