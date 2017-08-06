namespace Minary.Domain.Main
{
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Form;
  using Minary.State;
  using System.Net.NetworkInformation;


  public class MinaryFactory
  {
    public static IMinaryState GetMinaryEventBase(MinaryMain minaryObj)
    {
      MinaryState state = DetermineSystemState();
      IMinaryState newState;

      if ((state & MinaryState.NetworkMissing) == MinaryState.NetworkMissing)
      {
        newState = new NetworkMissing(MinaryState.NetworkMissing);
        return newState;
      }
      else if ((state & MinaryState.WinPcapMissing) == MinaryState.WinPcapMissing)
      {
        newState = new WinPcapMissing(MinaryState.WinPcapMissing);
        return newState;
      }
      else if ((state & MinaryState.NotAdmin) == MinaryState.NotAdmin)
      {
        newState = new NotAdmin(MinaryState.NotAdmin);
        return newState;
      }
      else
      {
        newState = new StateOk(MinaryState.StateOk);
        newState.Bt_Attack_Click = null;
        newState.Bt_ScanLan_Click = null;
        newState.Bt_Attack_Click += minaryObj.Bt_Attack_Click;
        newState.Bt_ScanLan_Click += minaryObj.Bt_ScanLan_Click;
        return newState;
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
