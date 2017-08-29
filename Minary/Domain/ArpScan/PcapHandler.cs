namespace Minary.Domain.ArpScan
{
  using PcapDotNet.Core;
  using System.Collections.ObjectModel;
  using System.Linq;


  public class PcapHandler
  {

    #region MEMBERS

    private static PcapHandler instance;
    private ReadOnlyCollection<LivePacketDevice> allinterfaces;

    #endregion


    #region PROPERTIES

    public static PcapHandler Inst { get { return instance ?? (instance = new PcapHandler()); } set { } }

    #endregion


    #region PUBLIC

    public PacketCommunicator OpenPcapDevice(string deviceId, int timeout)
    {
      LivePacketDevice selectedDevice = this.allinterfaces.Where(elem => elem.Name.Contains(deviceId)).First();
      PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, timeout);

      return communicator;
    }

    #endregion


    #region PRIVATE

    private PcapHandler()
    {
      this.allinterfaces = LivePacketDevice.AllLocalMachine;
    }

    #endregion

  }
}
