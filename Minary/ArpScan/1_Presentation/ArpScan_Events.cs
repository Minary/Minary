namespace Minary.ArpScan.Presentation
{
  using Minary.ArpScan.DataTypes;
  using Minary.Common;
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;


  public partial class ArpScan
  {
    
    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpScan.DGV_CellClick(): Start");

      // Ignore clicks that are not on button cells.
      if (e.RowIndex < 0)
      {
        return;
      }

      string ipAddress = this.dgv_Targets.Rows[e.RowIndex].Cells[0].Value.ToString();
      string macAddress = this.dgv_Targets.Rows[e.RowIndex].Cells[1].Value.ToString();
      string vendor = this.dgv_Targets.Rows[e.RowIndex].Cells[2].Value.ToString();

      // (De)Activate targetSystem system
      if (e.ColumnIndex != 3)
      {
        return;
      }

      for (int i = 0; i < this.targetRecords.Count; i++)
      {
        if (this.targetRecords[i].MacAddress == macAddress && this.targetRecords[i].IpAddress == ipAddress)
        {
          this.targetRecords[i].Attack = this.targetRecords[i].Attack ? false : true;
          break;
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (this.dgv_Targets.IsCurrentCellDirty)
      {
        this.dgv_Targets.CommitEdit(DataGridViewDataErrorContexts.Commit);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void Dgv_CellValueChanged(object obj, DataGridViewCellEventArgs e)
    {
      //// compare to checkBox column index
      if (e.ColumnIndex != 0)
      {
        return;
      }

      DataGridViewCheckBoxCell check = this.dgv_Targets[0, e.RowIndex] as DataGridViewCheckBoxCell;
      if (Convert.ToBoolean(check.Value) == true)
      {
        // If tick is added!
        // ...
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Bt_Close_Click(object sender, EventArgs e)
    {
      // Stopping scan process
      this.arpScanTask.StopArpScan();

      // Resetting GUI elements
      this.SetArpScanGuiOnStopped();

      // Sending targetSystem list to modules
      this.minaryMain.PassNewTargetListToPlugins();

      // Hiding form
      this.Hide();
      this.minaryMain.Focus();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Bt_Scan_Click(object sender, EventArgs e)
    {
      if (arpScan.isScanStarted == true)
      {
        arpScan.SetArpScanGuiOnStopped();
      }
      else
      {
        string startIp = string.Empty;
        string stopIp = string.Empty;

        arpScan.targetRecords.Clear();
        arpScan.SetArpScanGuiOnStarted();

        try
        {
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
            MaxNumberSystemsToScan = -1,

            OnDataReceived = arpScan.UpdateTextBox,
            OnArpScanStopped = this.ArpScanStopped,
            IsDebuggingOn = Minary.Common.Debugging.IsDebuggingOn
          };

          arpScan.arpScanTask.StartArpScan(arpConf);
        }
        catch (Exception ex)
        {
          LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpScan: {0}", ex.Message);
          MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          arpScan.SetArpScanGuiOnStopped();
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ArpScan_FormClosing(object sender, FormClosingEventArgs e)
    {
      // Stopping scan process
      this.arpScanTask.StopArpScan();

      // Resetting GUI elements
      this.SetArpScanGuiOnStopped();

      // Sending targetSystem list to modules
      this.minaryMain.PassNewTargetListToPlugins();

      // Hiding form
      this.Hide();
      e.Cancel = true;

      this.minaryMain.Activate();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Rb_Subnet_CheckedChanged(object sender, EventArgs e)
    {
      this.tb_Netrange1.ReadOnly = true;
      this.tb_Netrange2.ReadOnly = true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Rb_Netrange_CheckedChanged(object sender, EventArgs e)
    {
      this.tb_Netrange1.ReadOnly = false;
      this.tb_Netrange2.ReadOnly = false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Tb_Netrange2_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
      {
        return;
      }

      this.targetRecords.Clear();
      this.SetArpScanGuiOnStarted();

      try
      {
        ArpScanConfig arpConf = new ArpScanConfig()
        {
          InterfaceId = this.interfaceId,
          GatewayIp = this.gatewayIp,
          LocalIp = this.localIp,
          NetworkStartIp = (this.rb_Netrange.Checked == true) ? this.tb_Netrange1.Text.ToString() : this.tb_Subnet1.ToString(),
          NetworkStopIp = (this.rb_Netrange.Checked == true) ? this.tb_Netrange2.Text.ToString() : this.tb_Subnet2.ToString(),
          OnDataReceived = this.UpdateTextBox,
          OnArpScanStopped = this.SetArpScanGuiOnStopped,
          IsDebuggingOn = Debugging.IsDebuggingOn
        };

        this.arpScanTask.StartArpScan(arpConf);
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpScan: {0}", ex.Message);
        MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        this.SetArpScanGuiOnStopped();
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Tb_Netrange1_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
      {
        return;
      }

      this.targetRecords.Clear();
      this.SetArpScanGuiOnStarted();

      try
      {
        ArpScanConfig arpConf = new ArpScanConfig()
        {
          InterfaceId = this.interfaceId,
          GatewayIp = this.gatewayIp,
          LocalIp = this.localIp,
          NetworkStartIp = (this.rb_Netrange.Checked == true) ? this.tb_Netrange1.Text.ToString() : this.tb_Subnet1.ToString(),
          NetworkStopIp = (this.rb_Netrange.Checked == true) ? this.tb_Netrange2.Text.ToString() : this.tb_Subnet2.ToString(),
          OnDataReceived = this.UpdateTextBox,
          OnArpScanStopped = this.SetArpScanGuiOnStopped,
          IsDebuggingOn = Debugging.IsDebuggingOn
        };

        this.arpScanTask.StartArpScan(arpConf);
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpScan: {0}", ex.Message);
        MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        this.SetArpScanGuiOnStopped();
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.targetRecords == null || this.targetRecords.Count <= 0)
      {
        return;
      }

      for (int i = 0; i < this.dgv_Targets.Rows.Count; i++)
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


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Dgv_Targets_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
      {
        return;
      }

      try
      {
        DataGridView.HitTestInfo hti = this.dgv_Targets.HitTest(e.X, e.Y);
        if (hti.RowIndex >= 0)
        {
          this.cms_ManageTargets.Show(this.dgv_Targets, e.Location);
        }
      }
      catch (Exception)
      {
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnselectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.targetRecords == null || this.targetRecords.Count <= 0)
      {
        return;
      }

      for (int i = 0; i < this.dgv_Targets.Rows.Count; i++)
      {
        try
        {
          this.dgv_Targets.Rows[i].Cells["status"].Value = false;
        }
        catch (Exception)
        {
        }
      }
    }


    /// <summary>
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.arpScanTask.StopArpScan();
        this.Close();
        return true;
      }
      else
        return base.ProcessDialogKey(keyData);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<Tuple<string, string>> targetList = new List<Tuple<string, string>>();
      string ipAddress = string.Empty;
      string macAddress = string.Empty;

      foreach (DataGridViewRow tmpRow in this.dgv_Targets.Rows)
      {
        try
        {
          ipAddress = tmpRow.Cells["IpAddress"].Value.ToString();
          macAddress = tmpRow.Cells["MacAddress"].Value.ToString();
          targetList.Add(new Tuple<string, string>(macAddress, ipAddress));
        }
        catch (Exception)
        {
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnscanedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<Tuple<string, string>> targetList = new List<Tuple<string, string>>();
      string ipAddress = string.Empty;
      string macAddress = string.Empty;

      foreach (DataGridViewRow tmpRow in this.dgv_Targets.Rows)
      {
        try
        {
          if (tmpRow.Cells["LastScanDate"] == null || tmpRow.Cells["LastScanDate"].Value == null || tmpRow.Cells["LastScanDate"].Value.ToString().Length <= 0)
          {
            ipAddress = tmpRow.Cells["IpAddress"].Value.ToString();
            macAddress = tmpRow.Cells["MacAddress"].Value.ToString();
            targetList.Add(new Tuple<string, string>(macAddress, ipAddress));
          }
        }
        catch (Exception)
        {
        }
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ThisSystemToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<Tuple<string, string>> targetList = new List<Tuple<string, string>>();
      string ipAddress = string.Empty;
      string macAddress = string.Empty;

      try
      {
        ipAddress = this.dgv_Targets.SelectedRows[0].Cells["IpAddress"].Value.ToString();
        macAddress = this.dgv_Targets.SelectedRows[0].Cells["MacAddress"].Value.ToString();
        targetList.Add(new Tuple<string, string>(macAddress, ipAddress));
      }
      catch (Exception)
      {
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Dgv_Targets_MouseDown(object sender, MouseEventArgs e)
    {
      try
      {
        DataGridView.HitTestInfo hti = this.dgv_Targets.HitTest(e.X, e.Y);

        if (hti.RowIndex >= 0)
        {
          this.dgv_Targets.ClearSelection();
          this.dgv_Targets.Rows[hti.RowIndex].Selected = true;
          this.dgv_Targets.CurrentCell = this.dgv_Targets.Rows[hti.RowIndex].Cells[0];
        }
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("ArpScan: {0}", ex.Message);
        this.dgv_Targets.ClearSelection();
      }
    }

    #endregion

  }
}