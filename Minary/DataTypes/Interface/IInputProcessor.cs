namespace Minary.DataTypes.Interface
{

  public interface IInputProcessor
  {

    #region PROPERTIES

    bool IsBeepOn { get; set; }

    #endregion


    #region METHODS

    void StartInputProcessing();

    void StopInputProcessing();

    #endregion

  }
}
