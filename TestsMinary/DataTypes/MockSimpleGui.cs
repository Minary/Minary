using Minary.DataTypes.ArpScan;
using Minary.Form.ArpScan.DataTypes;
using Minary.Form.SimpleGUI.Presentation;
using System.ComponentModel;


namespace TestsMinary.DataTypes
{

  // SimpleGui mock class
  public class MockSimpleGui : SimpleGuiUserControl
  {
    public MockSimpleGui(Minary.Form.Main.MinaryMain minaryMain) : base(minaryMain)
    {
      base.arpScanConfig = new ArpScanConfig()
      {
        InterfaceId = "MOCK_IFC",
        GatewayIp = "MOCK_255.255.255.255",
        LocalIp = "MOCK_192.168.1.1"
      };
    }

    public BindingList<SystemFoundSimple> TargetStringList { get { return base.targetStringList; } set { } }
  }
}
