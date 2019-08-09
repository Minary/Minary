namespace Minary.Domain.ArpScan
{
  using Minary.DataTypes.ArpScan;
  using Minary.Form.ArpScan.DataTypes;
  using System.Collections.Generic;


  public class ReplyListener : IObservableArpResponse
  {

    #region MEMBERS

    private List<IObserverArpResponse> observers = new List<IObserverArpResponse>();

    #endregion


    #region PUBLIC

    public ReplyListener()
    {
    }

    #endregion


    #region INTERFACE: IObservableArpResponse

    public void AddObserverArpResponse(IObserverArpResponse observer)
    {
      this.observers.Add(observer);
    }


    public void NotifyArpResponseNewRecord(SystemFound systemData)
    {
      this.observers.ForEach(elem => elem.UpdateNewRecord(systemData));
    }

    #endregion

  }
}
