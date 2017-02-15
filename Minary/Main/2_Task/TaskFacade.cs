namespace Minary
{
  using System.Windows.Forms;


  public class TaskFacade : Control
  {

    #region MEMBERS

    private static TaskFacade instance;
    private MinaryMain minaryMain;
    private DataGridView dgvPlugins;

    #endregion


    #region PUBLIC

    /// <summary>
    ///
    /// </summary>
    /// <param name="pParent"></param>
    /// <param name="dgvPlugins"></param>
    /// <returns></returns>
    public static TaskFacade GetInstance(MinaryMain parent, DataGridView dgvPlugins)
    {
      return instance ?? (instance = new TaskFacade(parent, dgvPlugins));
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="TaskFacade"/> class.
    ///
    /// </summary>
    /// <param name="pParent"></param>
    /// <param name="dgvPlugins"></param>
    private TaskFacade(MinaryMain parent, DataGridView dgvPlugins)
    {
      this.minaryMain = parent;
      this.dgvPlugins = dgvPlugins;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="pluginName"></param>
    private delegate void ActivatePluginDelegate(string pluginName);
    public void ActivatePlugin(string pluginName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ActivatePluginDelegate(this.ActivatePlugin), new object[] { pluginName });
        return;
      }

      TabPage tabPage;
      int pluginRowDGV = this.GetPluginDGVRowByName(pluginName);

      if (this.minaryMain.UsedPlugins[pluginRowDGV].Active != "0")
      {
        return;
      }

      if ((tabPage = this.minaryMain.PluginHandler.FindTabPageInCatalog(pluginName)) == null)
      {
        return;
      }

      // Set new status in the main GUI DGV
      this.minaryMain.UsedPlugins[pluginRowDGV].Active = "1";
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="pluginName"></param>
    public delegate void DeactivatePluginDelegate(string pluginName);
    public void DeactivatePlugin(string pluginName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new DeactivatePluginDelegate(this.DeactivatePlugin), new object[] { pluginName });
        return;
      }

      TabPage tabPage;
      int pluginRowDGV = this.GetPluginDGVRowByName(pluginName);

      if (this.minaryMain.UsedPlugins[pluginRowDGV].Active != "1")
      {
        return;
      }

      if ((tabPage = this.minaryMain.PluginHandler.FindTabPageInCatalog(pluginName)) == null)
      {
        return;
      }

      // Set new status in the registry (to survive the application stop)
      // Utils.SetRegistryValue(minaryMain.UsedPlugins[pluginRowDGV].PluginName, "state", "off");

      // Set new status in the main GUI DGV
      this.minaryMain.UsedPlugins[pluginRowDGV].Active = "0";
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="pluginName"></param>
    /// <returns></returns>
    public int GetPluginDGVRowByName(string pluginName)
    {
      int retVal = -1;

      for (int i = 0; i < this.dgvPlugins.Rows.Count; i++)
      {
        if (this.dgvPlugins.Rows[i].Cells[0].Value.ToString() == pluginName)
        {
          retVal = i;
          break;
        }
      }

      return retVal;
    }

    #endregion

  }
}
