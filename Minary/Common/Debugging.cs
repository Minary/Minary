namespace Minary.Common
{


  public class Debugging
  {

    #region MEMBERS

    private static bool isDebuggingOn = false;

    #endregion


    #region PROPERTIES

    public static bool IsDebuggingOn { get { return isDebuggingOn; } set { isDebuggingOn = value; } }

    #endregion

  }
}
