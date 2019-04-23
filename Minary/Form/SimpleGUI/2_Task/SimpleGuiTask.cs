namespace Minary.Form.SimpleGUI.Task
{
  using Minary.DataTypes.ArpScan;
  using System.Collections.Generic;


  public class SimpleGuiTask: IObservableArpResponse
  {

    #region MEMBERS

    private static SimpleGuiTask inst;
    private List<IObserverArpResponse> observers = new List<IObserverArpResponse>();

    #endregion


    #region PROPERTIES

    public static SimpleGuiTask Inst { get { return inst ?? (inst = new SimpleGuiTask()); } set { } }

    #endregion


    #region INTERFACE: IObservableArpResponse

    public void AddObserver(IObserverArpResponse observer)
    {
      this.observers.Add(observer);
    }


    public void NotifyNewRecord(SystemFound systemData)
    {
      this.observers.ForEach(elem => elem.UpdateNewRecord(systemData));
    }

    #endregion

  }
}
