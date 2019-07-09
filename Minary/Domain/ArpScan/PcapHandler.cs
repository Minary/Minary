namespace Minary.Domain.ArpScan
{    
  using SharpPcap;
  using SharpPcap.LibPcap;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Net.NetworkInformation;


    public class PcapHandler
  {

    #region MEMBERS

    private static PcapHandler instance;
    //private ReadOnlyCollection<LibPcapLiveDevice> allinterfaces;
    private CaptureDeviceList allinterfaces;

    #endregion
       

    #region PROPERTIES

    public static PcapHandler Inst { get { return instance ?? (instance = new PcapHandler()); } set { } }

    #endregion


    #region PUBLIC

    public ICaptureDevice OpenPcapDevice(string deviceId, int timeout)
    {
      var selectedDevice = this.allinterfaces.Where(elem => elem.Name.Contains(deviceId)).First();
      selectedDevice.Open(DeviceMode.Promiscuous);

      return selectedDevice;
    }

    #endregion


    #region PRIVATE

    private PcapHandler()
    {
            this.allinterfaces = CaptureDeviceList.Instance; // LivePacketDevice.AllLocalMachine;
    }

    #endregion

  }
}
