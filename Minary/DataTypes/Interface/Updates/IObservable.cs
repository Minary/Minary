namespace Minary.DataTypes.Interface.Updates
{
  using Minary.Form.Updates.Config;


  public interface IObservable
  {
    void AddObserver(IObserver observer);

    void Notify(UpdateData updateData);
  }
}