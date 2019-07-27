namespace Minary.Domain.ArpScan
{
    using Minary.DataTypes.ArpScan;
    using Minary.DataTypes.Enum;
    using Minary.Form.ArpScan.DataTypes;
    using Minary.LogConsole.Main;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;


    public class ArpScanner : IObservableArpRequest, IObservableArpCurrentIp
    {

        #region MEMBERS

        private ArpScanConfig arpScanConfig;

        private List<IObserverArpRequest> observersArpRequest = new List<IObserverArpRequest>();
        private List<IObserverArpCurrentIp> observersCurrentIp = new List<IObserverArpCurrentIp>();

        #endregion


        #region PROPERTIES

        public ArpScanConfig Config { get; set; }

        #endregion


        #region PUBLIC

        public ArpScanner(ArpScanConfig arpScanConfig)
        {
            this.arpScanConfig = arpScanConfig;
        }


        public void StartScanning()
        {
            int percentageCounter = 10;
            // this.arpScanConfig.NetworkStartIp);
            // this.arpScanConfig.NetworkStopIp);

            // this.NotifyProgressBarArpRequest(currentPercentage);
            // this.NotifyProgressCurrentIp(currIpStr);
        }

        #endregion


        #region PRIVATE

        private int CalculatePercentage(long totalIps, int counter)
        {
            var percentage = (double)100 / totalIps * counter;
            var roundedPercentage = (int)Math.Round(percentage, MidpointRounding.AwayFromZero);

            return roundedPercentage;
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