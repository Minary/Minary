namespace Minary.Form.Template.Presentation
{
  using Minary.Domain.DSL;
  using Minary.Form;
  using Minary.Form.Template.DataTypes.Template;
  using System;
  using System.IO;
  using System.Text;
  using System.Windows.Forms;


  public partial class LoadTemplate : Form
  {

    #region MEMBERS

    private MinaryMain minaryMain;
    private Infrastructure.TemplateHandler infrastructure;
    private StringBuilder rtfData;
    private RecordMinaryTemplate templateData;
    private string templateFileName;

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
    }


    #endregion


    #region PRIVATE

    /// <summary>
    ///
    /// </summary>
    /// <param name="templateFile"></param>
    private RecordMinaryTemplate LoadAttackTemplate(string templateFile)
    {
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
      Calls calls = new Calls(this.minaryMain, this.templateData);

      // Activate relevant plugins. Deactivate non-relevant plugins
      this.AddMessage("Hiding all plugins", string.Empty);
      calls.HideAllTabPages();

      foreach (Plugin tmpPluginObj in this.templateData.Plugins)
      {
        try
        {
          this.AddMessage(string.Format("Loading plugin \"{0}\"", tmpPluginObj.Name), string.Empty);
          calls.ActivatePlugin(tmpPluginObj.Name);
          calls.LoadPluginData(tmpPluginObj.Name, tmpPluginObj.Data);
        }
        catch (Exception ex)
        {
          string message = string.Format("\"{0}\" {1}", tmpPluginObj.Name, ex.Message);
          //throw new Exception(message);
        }
      }

      // Scan network
      this.AddMessage("Start ARP scanning network ...", string.Empty);
      //calls.ScanNetwork();
      //this.AddMessage("Arp scanning network done", string.Empty);

      //// Report and return if no target systems were found.
      //if (calls.GetCurrentNumberOfTargetSystems() <= 0)
      //{
      //  this.AddMessage("No target systems were found", string.Empty);
      //  this.AddMessage(string.Empty, "The attack is NOT running");
      //  return this.templateData;
      //}

      //// Select target systems
      //this.AddMessage(string.Format("Selecting the first {0} systems as targets", this.templateData.AttackConfig.NumberSelectedTargetSystems), string.Empty);
      //calls.SelectTargetSystems(this.templateData.AttackConfig.NumberSelectedTargetSystems);

      //// Attack systems
      //if (this.templateData.AttackConfig.StartAttack == 1)
      //{
      //  this.AddMessage(@"Starting attacking target systems", string.Empty);
      //  calls.StartAttack();
      //  this.AddMessage(string.Empty, "The attack IS running");
      //}

      return this.templateData;
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
      string formatedMessage = string.Format(@"{0}   \b {1}\b0  {2}\line ", dateTime, header, message);

      this.rtfData.Append(formatedMessage);

      try
      {
        this.rtb_Logs.Rtf = @"{\rtf1\ansi " + this.rtfData.ToString() + @" }";
      }
      catch (Exception ex)
      {
      }
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

      this.AddMessage("Loading template done.", string.Empty);
    }


    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        if (this.bgw_LoadTemplate.IsBusy)
        {
          MessageDialog.ShowWarning(string.Empty, "System still busy loading template.", this);
          return false;
        }
        else
        {
          this.Hide();
          this.minaryMain.Activate();
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
