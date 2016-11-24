namespace MinaryLib.AttackService
{
  public class ServiceParameters
  {

    #region MEMBERS

    private int selectedIfcIndex;
    private string selectedIfcId;

    #endregion


    #region PROPERTIES

    public int SelectedIfcIndex { get { return this.selectedIfcIndex; } set { this.selectedIfcIndex = value; } }
    public string SelectedIfcId { get { return this.selectedIfcId; } set { this.selectedIfcId = value; } }

    #endregion

  }
}
