namespace Minary.Form.Template.Presentation
{
  using Minary.Domain.DSL;
  using Minary.Form;
  using Minary.Form.Template.DataTypes.Template;
  using System;
  using System.IO;
  using System.Text;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows.Forms;


  public partial class LoadTemplate : Form
  {

    #region MEMBERS

    private const int MAX_RELOAD_ATTEMPS = 10;
    private MinaryMain minaryMain;
    private Infrastructure.TemplateHandler infrastructure;
    private StringBuilder rtfData;
    private RecordMinaryTemplate templateData;
    private string templateFileName;
    private Calls callObj;
    private int noReloadAttemps;
    private string templateFile;

    #endregion


    #region PROPERTIES

    public RecordMinaryTemplate TemplateData { get { return this.templateData; } set { } }

    #endregion


    #region PUBLIC

    public LoadTemplate(MinaryMain minaryMain, string templateFileName)
    {
      this.InitializeComponent();

      this.minaryMain = minaryMain;
      this.templateFileName = templateFileName;
      this.infrastructure = new Infrastructure.TemplateHandler();
      this.rtfData = new StringBuilder();
      this.noReloadAttemps = 0;
    }

    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="templateFile"></param>
    private void LoadAttackTemplate(string templateFile)
    {
      this.templateFile = templateFile;

      // Verify if input file is  correct.
      if (string.IsNullOrEmpty(templateFile))
      {
        throw new Exception("Something is wrong with the template file");
      }

      if (!File.Exists(templateFile))
      {
        throw new Exception("The template file does not exist");
      }

      if (Path.GetExtension(templateFile).ToLower() != string.Format(".{0}", Minary.Config.MinaryFileExtension))
      {
        throw new Exception("The defined file is no Minary template file");
      }

      // Load tepmlate data into object
      this.templateData = this.infrastructure.LoadAttackTemplate(templateFile);
      this.callObj = new Calls(this.minaryMain, this.templateData);

      // Activate relevant plugins. Deactivate non-relevant plugins
      this.HideAllTabPages();
      this.LoadPlugins();
      this.ExecuteArpScan();
    }


    private void HideAllTabPages()
    {
      this.AddMessage("Hiding all plugins", "Template");
      this.callObj.HideAllTabPages();
    }


    private void LoadPlugins()
    {
      foreach (Plugin tmpPluginObj in this.templateData.Plugins)
      {
        try
        {
          this.AddMessage(string.Format("Loading plugin \"{0}\"", tmpPluginObj.Name), "Plugin");
          this.callObj.ActivatePlugin(tmpPluginObj.Name);
          this.callObj.LoadPluginData(tmpPluginObj.Name, tmpPluginObj.Data);
        }
        catch (Exception ex)
        {
          this.AddMessage($"Error: {ex.Message}", "Plugin");
        }
      }
    }


    private delegate void ExecuteArpScanDelegate();
    private void ExecuteArpScan()
    {
      if (this.InvokeRequired == true)
      {
        this.BeginInvoke(new ExecuteArpScanDelegate(this.ExecuteArpScan), new object[] { });
        return;
      }

      // Scan network
      if (this.templateData == null || this.templateData.AttackConfig == null)
      {
        throw new Exception("The template is invalid");
      }

      if (this.templateData.AttackConfig.ScanNetwork == 1)
      {
        this.AddMessage("ARP scanning network", "ARP");
        this.callObj.ScanNetwork(this.POSTArpScan_StartAttack);
        return;
      }

      this.AddMessage("ARP scanning disabled", "ARP");
      this.AddMessage("Loading template done.", "Template");
      this.AddMessage("Closing this view in 5 seconds ...", "Template");
      this.CloseFormInXSeconds(5);
    }


    private void POSTArpScan_StartAttack()
    {

      // Attack systems
      if (this.templateData.AttackConfig.StartAttack == 1)
      {
        this.AddMessage("Starting attacking target systems", "Template");

        // Report and return if no target systems were found.
        if (this.callObj.GetCurrentNumberOfTargetSystems() <= 0)
        {
          this.AddMessage("No target systems were found", "Template");
          this.AddMessage("The attack was aborted", "Template");
          string message = "No target systems were found.\r\n" +
                           "Attack script aborted";

          // If user wants to reload the template again 
          // (because the previous loading procedure failed)
          // initiate the reload procedure
          if (this.noReloadAttemps >= MAX_RELOAD_ATTEMPS)
          {
            MessageDialog.Inst.ShowWarning(string.Empty, message, this, MessageBoxButtons.OK);
          }
          else if (MessageDialog.Inst.ShowWarning(string.Empty, message + "\r\n\r\nDo you want to reload the template?", this, MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            this.noReloadAttemps++;
            this.LoadAttackTemplate(this.templateFile);
          }

          return;
        }

        // Select target systems
        this.AddMessage(string.Format("Selecting the first {0} systems as targets", this.templateData.AttackConfig.NumberSelectedTargetSystems), "Template");
        this.callObj.SelectTargetSystems(this.templateData.AttackConfig.NumberSelectedTargetSystems);

        // Start actual attack
        this.callObj.StartAttack();
        this.AddMessage("The attack IS running", "Template");
      }

      this.AddMessage("Loading template done.", "Template");
      this.AddMessage("Closing this view in 5 seconds ...", "Template");

      this.CloseFormInXSeconds(5);
    }


    public delegate void AddMessageDelegate(string message, string header);
    private void AddMessage(string message, string header)
    {
      if (this.rtb_Logs.InvokeRequired)
      {
        this.BeginInvoke(new AddMessageDelegate(this.AddMessage), new object[] { message, header });
        return;
      }

      if (message == null)
      {
        return;
      }

      message = message.Trim();
      string dateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
      string formatedMessage = string.Format(@"{0}   \b {1,-10}\b0  {2}\line ", dateTime, header, message);

      this.rtfData.Append(formatedMessage);

      try
      {
        this.rtb_Logs.Rtf = @"{\rtf1\ansi " + this.rtfData.ToString() + @" }";
      }
      catch
      {
      }
    }


    private void CloseFormInXSeconds(int seconds = 5)
    {
      new Task(() =>
      {
        Thread.Sleep(seconds * 1000);
        if (this.Visible == false)
        {
          return;
        }

        this.BeginInvoke(new Action(() =>
        {
          this.Close();
        }));
      }).Start();
    }

    #endregion


    #region EVENTS

    private void Bt_Close_Click(object sender, EventArgs e)
    {
      this.Close();
    }


    private void BgwLoadTemplateDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      try
      {
        this.LoadAttackTemplate(this.templateFileName);
      }
      catch (Exception ex)
      {
        string message = string.Format("An error occurred while loading template file \"{0}\": {1}", Path.GetFileName(this.templateFileName), ex.Message);
        this.AddMessage(message, "Exception");
      }
    }


    private void BgwLoadTemplateRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
      this.Cursor = Cursors.Default;
      this.bt_Close.Enabled = true;
    }


    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        if (this.bgw_LoadTemplate.IsBusy)
        {
          MessageDialog.Inst.ShowWarning(string.Empty, "System still busy loading template.", this);
          return false;
        }
        else
        {
          this.Close();
          return false;
        }
      }
      else
      {
        return base.ProcessDialogKey(keyData);
      }
    }


    private void LoadTemplate_Shown(object sender, EventArgs e)
    {
      if (this.bgw_LoadTemplate.IsBusy == false)
      {
        this.rtfData.Clear();
        this.bt_Close.Enabled = false;
        this.Cursor = Cursors.WaitCursor;
        this.bgw_LoadTemplate.RunWorkerAsync();
      }
    }

    #endregion

  }
}
