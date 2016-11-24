namespace Minary.Common
{
  using System;
  using System.IO;

  public class Debugging
  {

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static bool IsDebuggingOn()
    {
      bool retVal = false;

      if (File.Exists(Config.DebuggingFile))
      {
        retVal = true;
      }

      return retVal;
    }




    /// <summary>
    ///
    /// </summary>
    /// <param name="debugStatus"></param>
    public static void EnableDebugging(bool debugStatus)
    {
      try
      {
        if (!debugStatus)
        {
          File.Delete(Config.DebuggingFile);
        }
        else
        {
          File.Create(Config.DebuggingFile);
        }
      }
      catch (Exception)
      {
      }
    }
  }
}
