namespace Minary.Form.ArpScan.Presentation
{
  using Minary.Form.ArpScan.DataTypes;
  using Minary.Common;
  using System;


  public partial class ArpScan
  {

    #region DSL

    public void StartArpScanInBackground(Action onArpScanStopped, int maxNumberSystemsToScan = -1)
    {
      if (this.bgw_ArpScanSender.IsBusy == true)
      {
        throw new Exception("Another ArpScan instance is already running");
      }
      
      string startIp = string.Empty;
      string stopIp = string.Empty;

      this.targetRecords.Clear();

      // User defined net range
      if (this.rb_Netrange.Checked == true)
      {
        startIp = this.tb_Netrange1.Text.ToString();
        stopIp = this.tb_Netrange2.Text.ToString();
      }
      else
      {
        startIp = this.tb_Subnet1.Text.ToString();
        stopIp = this.tb_Subnet2.Text.ToString();
      }

      ArpScanConfig arpConf = new ArpScanConfig()
      {
        InterfaceId = this.interfaceId,
        GatewayIp = this.gatewayIp,
        LocalIp = this.localIp,
        NetworkStartIp = startIp,
        NetworkStopIp = stopIp,
        MaxNumberSystemsToScan = maxNumberSystemsToScan,
        ObserverClass = this
      };

      //try
      //{
      //  this.arpScanTask.StartArpScan(arpConf);
      //}
      //catch (Exception)
      //{
      //  this.arpScanTask.StopArpScan();
      //}
    }


    public int NumberTargetSystems()
    {
      return this.dgv_Targets.Rows.Count;
    }


    public void StopRunningArpScan()
    {
      this.SetArpScanGuiOnStopped();
    }


    public void SelectRandomSystems(int noTargetSystems)
    {
      for (int i = 0; i < this.dgv_Targets.Rows.Count && i < noTargetSystems; i++)
      {
        try
        {
          this.dgv_Targets.Rows[i].Cells["status"].Value = true;
        }
        catch
        {
        }
      }
    }

    #endregion

  }
}
