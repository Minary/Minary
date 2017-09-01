namespace Minary.Domain.Main
{
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Domain.State;
  using Minary.Domain.State.Service;
  using Minary.Form;
  using System.IO;
  using System.Net.NetworkInformation;


  public class SystemStateCheck
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
      else if ((state & MinaryState.ApeBinaryMissing) == MinaryState.ApeBinaryMissing)
      {
        return new ApeNotInstalled(minaryObj);
      }
      else if ((state & MinaryState.ApeSnifferMissing) == MinaryState.ApeSnifferMissing)
      {
        return new ApeSnifferNotInstalled(minaryObj);
      }
      else if ((state & MinaryState.HttpProxyMissing) == MinaryState.HttpProxyMissing)
      {
        return new HttpReverseProxyNotInstalled(minaryObj);
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

      // Check SystemService: APE
      if (File.Exists(Config.ApeBinaryPath) == false)
      {
        retVal |= MinaryState.ApeBinaryMissing;
      }

      // Check SystemService: APESniffer
      if (File.Exists(Config.ApeSnifferBinaryPath) == false)
      {
        retVal |= MinaryState.ApeSnifferMissing;
      }

      // Check SystemService: HttpProxy
      if (File.Exists(Config.HttpReverseProxyBinaryPath) == false)
      {
        retVal |= MinaryState.HttpProxyMissing;
      }

      return retVal;
    }

    #endregion

  }
}
