namespace Minary.Domain.Network
{
  using Minary.Form;
  using Minary.Form.GuiAdvanced;
  using Minary.Common;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Struct;
  using Minary.LogConsole.Main;
  using System;
  using System.Collections;
  using System.Linq;
  using System.Net.NetworkInformation;


  public class NetworkInterfaceHandler
  {

    #region MEMBERS

    private MinaryMain minaryMain;

    #endregion


    #region PROPERTIES

    public OperationalStatus LastNetworkOperState { get; set; }

    public ArrayList Interfaces { get; set; }

    public NetworkInterface[] AllAttachednetworkInterfaces { get; set; }

    #endregion


    #region PUBLIC

    public NetworkInterfaceHandler(MinaryMain minaryMain)
    {
      this.minaryMain = minaryMain;
      this.Interfaces = new ArrayList();
      this.LoadInterfaces();

      // Register callback to handle network address changes
      NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(this.AddressChangedCallback);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public NetworkInterfaceConfig IfcByIndex(int index)
    {
      if (index < 0 || 
          index >= this.Interfaces.Count)
      {
        throw new Exception("The interface index is invalid");
      }

      return (NetworkInterfaceConfig)this.Interfaces[index];
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetNetworkInterfaceIdByIndex(int index)
    {
      var retVal = string.Empty;

      if (index >= 0 && 
          index < this.Interfaces.Count)
      {
        retVal = ((NetworkInterfaceConfig)this.Interfaces[index]).Id;
      }

      return retVal;
    }


    public void LoadInterfaces()
    {
      this.Interfaces.Clear();
      this.AllAttachednetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

      foreach (NetworkInterface tmpInterface in this.AllAttachednetworkInterfaces)
      {
        if (tmpInterface.OperationalStatus != OperationalStatus.Up)
        {
          continue;
        }

        if (tmpInterface.GetIPProperties() == null ||
            tmpInterface.GetIPProperties().UnicastAddresses.Count <= 0)
        {
          continue;
        }

        // Find entry with valid IPv4 address
        // Continue if no valid IP address and netmask is found
        UnicastIPAddressInformation ipAddress = this.DetermineIpAddress(tmpInterface);
        if (ipAddress?.IPv4Mask == null)
        {
          continue;
        }

        // Append found interface with details to interface array
        try
        {
          var newInterface = default(NetworkInterfaceConfig);
          newInterface.IsUp = true;
          newInterface.Id = tmpInterface.Id;
          newInterface.Name = tmpInterface.Name;
          newInterface.Description = tmpInterface.Description;
          newInterface.IpAddress = ipAddress.Address.ToString();
          newInterface.MacAddress = NetworkFunctions.GetMacByIp(ipAddress.Address.ToString());
          newInterface.BroadcastAddr = NetworkFunctions.GetBroadcastAddress(ipAddress.Address, ipAddress.IPv4Mask).ToString();
          newInterface.NetworkAddr = NetworkFunctions.GetNetworkAddress(ipAddress.Address, ipAddress.IPv4Mask).ToString();
          newInterface.DefaultGw = this.DetermineGatewayIp(tmpInterface);
          newInterface.GatewayMac = NetworkFunctions.GetMacByIp(newInterface.DefaultGw);

          this.Interfaces.Add(newInterface);
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, ex.Message);
        }
      }
    }

    #endregion


    #region PRIVATE

    private UnicastIPAddressInformation DetermineIpAddress(NetworkInterface ifc)
    {
      UnicastIPAddressInformation ipAddress = null;
      foreach (UnicastIPAddressInformation tmpIPaddr in ifc.GetIPProperties().UnicastAddresses)
      {
        if (tmpIPaddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
          ipAddress = tmpIPaddr;
          break;
        }
      }

      return ipAddress;
    }


    private string DetermineGatewayIp(NetworkInterface ifc)
    {
      var defaultGwIp = string.Empty;

      if (ifc.GetIPProperties().GatewayAddresses.Count <= 0)
      {
        return defaultGwIp;
      }

      foreach (GatewayIPAddressInformation tmpAddress in ifc.GetIPProperties().GatewayAddresses)
      {
        if (!tmpAddress.Address.IsIPv6LinkLocal)
        {
          defaultGwIp = tmpAddress.Address.ToString();
          break;
        }
      }

      return defaultGwIp;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="interfaceId"></param>
    /// <returns></returns>
    private NetworkInterfaceConfig GetInterfaceById(string interfaceId)
    {
      var retVal = default(NetworkInterfaceConfig);
      foreach (NetworkInterfaceConfig tmpInterface in this.Interfaces)
      {
        LogCons.Inst.Write(LogLevel.Info, $"/{tmpInterface.Id}/{interfaceId}/");
        if (tmpInterface.Id == interfaceId)
        {
          retVal = tmpInterface;
          break;
        }
      }

      return retVal;
    }


    void AddressChangedCallback(object sender, EventArgs e)
    {
      var message = string.Empty;
      var currentNetConfig = this.GetInterfaceById(this.minaryMain.CurrentInterfaceId);

      // Network connection is up
      //if (NetworkInterface.GetIsNetworkAvailable() == true &&
      //    NetworkInterface.GetAllNetworkInterfaces().Length > 0)
      //{
      //  if (this.LastState == LastConnectionState.Connected)
      //  {
      //    return;
      //  }

      //  this.LastState = LastConnectionState.Connected;
      //  this.minaryInstance.SetMinaryState();
      //  this.minaryInstance.LoadNicSettings();
      //  MessageDialog.Inst.ShowInformation("Network connection", "A new network connection was detected", this.minaryInstance);
      //  return;
      //}

      //if (this.LastState == LastConnectionState.Disconnected)
      //{
      //  return;
      //}

      var networkIfcList = NetworkInterface.GetAllNetworkInterfaces();
      if (networkIfcList == null || networkIfcList.Count() <= 0)
      {
        LogCons.Inst.Write(LogLevel.Warning, $"AddressChangedCallback(): No network interfaces found");
        return;
      }

      var currentIfc = networkIfcList.Where(elem => elem.Id == minaryMain.CurrentInterfaceId).FirstOrDefault();
      if (currentIfc == null)
      {
        LogCons.Inst.Write(LogLevel.Warning, $"AddressChangedCallback(): No interface with the id '{minaryMain.CurrentInterfaceId}' found!");
        return;
      }

      if (currentIfc.OperationalStatus == this.LastNetworkOperState)
      {
        LogCons.Inst.Write(LogLevel.Debug, $"AddressChangedCallback(): The network operational state did not change.");
        return;
      }

      if (currentIfc.OperationalStatus == OperationalStatus.Up)
      {
        this.minaryMain.LoadNicSettings();
        MessageDialog.Inst.ShowWarning("Network connection", "The network connection is up", this.minaryMain);
      }
      else if (currentIfc.OperationalStatus == OperationalStatus.Down)
      {
        this.minaryMain.ClearCurrentNetworkState();
        this.minaryMain.StopAttack();
        MessageDialog.Inst.ShowWarning("Network connection", "The network connection is down", this.minaryMain);
      }
      else
      {
        this.minaryMain.ClearCurrentNetworkState();
        this.minaryMain.StopAttack();
        MessageDialog.Inst.ShowWarning("Network connection", $"The network connection has the state {currentIfc.OperationalStatus.ToString()}", this.minaryMain);
      }
      
      this.LastNetworkOperState = currentIfc.OperationalStatus;
      this.minaryMain.SetMinaryState();
    }

    #endregion

  }
}
