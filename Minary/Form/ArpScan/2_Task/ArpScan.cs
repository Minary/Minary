namespace Minary.Form.ArpScan.Task
{
  using Minary.DataTypes.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using System;
  using System.Collections.Generic;
  using System.Net;


  public class ArpScan : IObservableArpCurrentIp, IObservableArpResponse
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
      this.infrastructure.StopArpScan();
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

      if (stopIpInt - startIpInt > arpConfig.TotalSystemsToScan && 
          arpConfig.TotalSystemsToScan > 0)
      {
        uint tmpStopIpInt = (uint)startIpInt + arpConfig.TotalSystemsToScan;
        arpConfig.NetworkStopIp = IpHelper.UIntToIpString(tmpStopIpInt);
      }

// Assign an Update and OnStop function pointers
//arpConfig.OnArpScanStopped = OnArpScanStopped;
      arpConfig.OnReplyDataReceived = this.NotifyArpResponseNewRecord;
      arpConfig.OnRequestSent = this.NotifyProgressCurrentIp;

      this.infrastructure.StartArpScan(arpConfig);
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
