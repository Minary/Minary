namespace Minary.Form.ArpScan.Task
{
  using Minary.DataTypes.ArpScan;
  using Minary.DataTypes.Enum;
  using Minary.Form.ArpScan.DataTypes;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Net;
  using System.Xml.Linq;


  public class ArpScan : IObservableArpRequest, IObservableArpCurrentIp, IObservableArpResponse
  {

    #region MEMBERS

    private Infrastructure.ArpScan infrastructure = new Infrastructure.ArpScan();
    private List<IObserverArpRequest> observersArpRequest = new List<IObserverArpRequest>();
    private List<IObserverArpCurrentIp> observersCurrentIp = new List<IObserverArpCurrentIp>();
    private List<IObserverArpResponse> observersArpResponse = new List<IObserverArpResponse>();

    #endregion


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="ArpScan"/> class.
    ///
    /// </summary>
    public ArpScan()
    {
    }


    /// <summary>
    ///
    /// </summary>
    public void StopArpScan()
    {
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="arpConfig"></param>
    public void StartArpScan(ArpScanConfig arpConfig)
    {
      IPAddress startIp;
      IPAddress stopIp;
      int startIpInt;
      int stopIpInt;

      // Validate input parameters.
      if (string.IsNullOrEmpty(arpConfig.InterfaceId))
      {
        throw new Exception("Something is wrong with the interface ID");
      }
      else if (!IPAddress.TryParse(arpConfig.NetworkStartIp, out startIp))
      {
        throw new Exception("Something is wrong with the start IpAddress address");
      }
      else if (!IPAddress.TryParse(arpConfig.NetworkStopIp, out stopIp))
      {
        throw new Exception("Something is wrong with the stop IpAddress address");
      }
      else if (IpHelper.Compare(startIp, stopIp) > 0)
      {
        throw new Exception("Start IpAddress address is greater than stop IpAddress address");
      }

      // If the network system range is above the defined limit change
      // the stop-address to the maximum upper limit.
      startIpInt = IpHelper.IPAddressToInt(startIp);
      stopIpInt = IpHelper.IPAddressToInt(stopIp);

      if (stopIpInt - startIpInt > arpConfig.MaxNumberSystemsToScan && 
          arpConfig.MaxNumberSystemsToScan > 0)
      {
        int tmpStopIpInt = startIpInt + arpConfig.MaxNumberSystemsToScan;
        arpConfig.NetworkStopIp = IpHelper.IntToIpString(tmpStopIpInt);
      }

      // Assign an Update and OnStop function pointers
      arpConfig.OnArpScanStopped = OnArpScanStopped;
      arpConfig.OnDataReceived = OnArpScanData;

      this.infrastructure.StartArpScan(arpConfig);
    }

    #endregion


    #region PRIVATE ARP scan events

    private delegate void OnArpScanStoppedDelegate();
    private void OnArpScanStopped()
    {
    }
    

    private delegate void OnArpScanDataDelegate(string data);
    private void OnArpScanData(string data)
    {
      var type = string.Empty;
      var ipAddress = string.Empty;
      var macAddress = string.Empty;
      var vendor = string.Empty;
      var note = string.Empty;

      try
      {
        // Extract newly detected system
        XDocument xmlContent = XDocument.Parse(data);
        var packetEntries = from service in xmlContent.Descendants("arp")
                            select new
                            {
                              Type = service.Element("type").Value,
                              IpAddress = service.Element("ip").Value,
                              MacAddress = service.Element("mac").Value
                            };

        if (packetEntries == null)
        {
          return;
        }

        // Send all newly found systems to all observers
        foreach (var tmpSystem in packetEntries)
        {
          var newSystem = new SystemFound(tmpSystem.MacAddress, tmpSystem.IpAddress);
          this.NotifyArpResponseNewRecord(newSystem);
        }
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogCons.Inst.Write(LogLevel.Error, ex.StackTrace);
      }
    }

    #endregion


    #region INTERFACE: IObservableArpNewRecord

    public void AddObserverArpResponse(IObserverArpResponse observer)
    {
      this.observersArpResponse.Add(observer);
    }


    public void NotifyArpResponseNewRecord(SystemFound newSystem)
    {
      foreach (var observer in this.observersArpResponse)
      {
        observer.UpdateNewRecord(newSystem);
      }
    }

    #endregion


    #region INTERFACE: IObservableArpRequest

    public void AddObserverArpRequest(IObserverArpRequest observer)
    {
      this.observersArpRequest.Add(observer);
    }


    public void NotifyProgressBarArpRequest(int progress)
    {
      this.observersArpRequest.ForEach(elem => elem.UpdateProgressbar(progress));
    }

    #endregion


    #region INTERFACE: IObservableCurrentIP

    public void AddObserverCurrentIp(IObserverArpCurrentIp observer)
    {
      this.observersCurrentIp.Add(observer);
    }


    public void NotifyProgressCurrentIp(string currentIp)
    {
      this.observersCurrentIp.ForEach(elem => elem.UpdateCurrentIp(currentIp));
    }

    #endregion

  }
}
