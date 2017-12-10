namespace Minary.DataTypes.Interface.Updates
{
  using Minary.Form.Updates.Config;


  public interface IObserver
  {
    void Update(UpdateData updateData);
  }
}
