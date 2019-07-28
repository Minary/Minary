namespace Minary.Domain.ArpScan
{
    using Minary.DataTypes.ArpScan;
    using Minary.Form.ArpScan.DataTypes;
    using System;
    using System.Collections.Generic;


    public class ArpScanner : IObservableArpRequest, IObservableArpCurrentIp
    {

        #region MEMBERS
    
        private List<IObserverArpRequest> observersArpRequest = new List<IObserverArpRequest>();
        private List<IObserverArpCurrentIp> observersCurrentIp = new List<IObserverArpCurrentIp>();

        #endregion


        #region PUBLIC

        public ArpScanner()
        {
        }


        public void StartScanning(ArpScanConfig arpScanConfig)
        {
            int percentageCounter = 10;
            // arpScanConfig.NetworkStartIp);
            // arpScanConfig.NetworkStopIp);

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