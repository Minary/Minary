namespace Minary.DataTypes
{
  using MinaryLib.Plugin;
  using System.Windows.Forms;


  public class MinaryExtension
  {

    #region PROPERTIES

    public IPlugin PluginObject { get; set; }

    public TabPage PluginTabPage { get; set; }

    public bool IsActive { get; set; }

    #endregion


    #region PUBLIC

    public MinaryExtension()
    {
    }

    #endregion

  }
}
