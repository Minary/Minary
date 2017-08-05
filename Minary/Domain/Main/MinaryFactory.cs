namespace Minary.Domain.Main
{
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.State;
  using System.Net.NetworkInformation;


  public class MinaryFactory
  {

    public static IMinaryState GetMinaryEventBase()
    {
      MinaryState state = DetermineSystemState();

      if ((state & MinaryState.NetworkMissing) == MinaryState.NetworkMissing)
      {
        return new NetworkMissing(MinaryState.NetworkMissing);
      }
      else if ((state & MinaryState.WinPcapMissing) == MinaryState.WinPcapMissing)
      {
        return new WinPcapMissing(MinaryState.WinPcapMissing);
      }
      else if ((state & MinaryState.NotAdmin) == MinaryState.NotAdmin)
      {
        return new NotAdmin(MinaryState.NotAdmin);
      }
      else
      {
        return new StateOk(MinaryState.StateOk);
      }
    }


    public static MinaryState DetermineSystemState()
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
  }
}
