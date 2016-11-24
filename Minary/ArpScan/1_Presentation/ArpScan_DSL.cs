namespace Minary.ArpScan.Presentation
{
  using Minary.ArpScan.DataTypes;
  using Minary.Common;
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;


  public partial class ArpScan
  {

    #region DSL

    public static void StartArpScanInBackground(Action onArpScanStopped, int maxNumberSystemsToScan = -1)
    {
      if (arpScan.isScanStarted == true)
      {
        throw new Exception("Another ArpScan instance is already running");
      }
      else
      {
        string startIp = string.Empty;
        string stopIp = string.Empty;

        arpScan.targetRecords.Clear();

        // User defined net range
        if (arpScan.rb_Netrange.Checked == true)
        {
          startIp = arpScan.tb_Netrange1.Text.ToString();
          stopIp = arpScan.tb_Netrange2.Text.ToString();
        }
        else
        {
          startIp = arpScan.tb_Subnet1.Text.ToString();
          stopIp = arpScan.tb_Subnet2.Text.ToString();
        }

        ArpScanConfig arpConf = new ArpScanConfig()
        {
          InterfaceId = arpScan.interfaceId,
          GatewayIp = arpScan.gatewayIp,
          LocalIp = arpScan.localIp,
          NetworkStartIp = startIp,
          NetworkStopIp = stopIp,
          MaxNumberSystemsToScan = maxNumberSystemsToScan,

          OnDataReceived = arpScan.UpdateTextBox,
          OnArpScanStopped = onArpScanStopped,
          IsDebuggingOn = Minary.Common.Debugging.IsDebuggingOn()
        };

        try
        {
          arpScan.isScanStarted = true;
          arpScan.arpScanTask.StartArpScan(arpConf);
        }
        catch (Exception ex)
        {
          arpScan.arpScanTask.StopArpScan();
          arpScan.isScanStarted = false;

          throw;
        }
      }
    }


    public static int NumberTargetSystems()
    {
      return arpScan.dgv_Targets.Rows.Count;
    }


    public static void StopRunningArpScan()
    {
      arpScan.SetArpScanGuiOnStopped();
    }


    public static void SelectRandomSystems(int noTargetSystems)
    {
      for (int i = 0; i < arpScan.dgv_Targets.Rows.Count && i < noTargetSystems; i++)
      {
        try
        {
          arpScan.dgv_Targets.Rows[i].Cells["status"].Value = true;
        }
        catch
        {
        }
      }
    }

    #endregion

  }
}
