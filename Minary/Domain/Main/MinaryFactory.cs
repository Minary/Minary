namespace Minary.Domain.Main
{
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Domain.State;
  using Minary.Form;
  using System.Net.NetworkInformation;


  public class MinaryFactory
  {

    #region PUBLIC

    public static IMinaryState GetMinaryEventBase(MinaryMain minaryObj)
    {
      MinaryState state = DetermineSystemState();
 
      if ((state & MinaryState.NetworkMissing) == MinaryState.NetworkMissing)
      {
        return new NetworkMissing(minaryObj);
      }
      else if ((state & MinaryState.WinPcapMissing) == MinaryState.WinPcapMissing)
      {
        return new WinPcapMissing(minaryObj);
      }
      else if ((state & MinaryState.NotAdmin) == MinaryState.NotAdmin)
      {
        return new NotAdmin(minaryObj);
      }

      return new StateOk(minaryObj);
    }

    #endregion


    #region PRIVATE

    private static MinaryState DetermineSystemState()
    {
      MinaryState retVal = MinaryState.StateOk;

      // Check WinPcap
      if (string.IsNullOrEmpty(Utils.TryExecute(WinPcap.GetWinPcapVersion)) == true)
      {
        retVal |= MinaryState.WinPcapMissing;
      }

      // Check interfaces
      if (NetworkInterface.GetIsNetworkAvailable() == false ||
          NetworkInterface.GetAllNetworkInterfaces().Length <= 0)
      {
        retVal |= MinaryState.NetworkMissing;
      }

      return retVal;
    }

    #endregion

  }
}
