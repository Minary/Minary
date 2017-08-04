namespace Minary.Template.Presentation
{
  using Minary.Template.DataTypes.Template;
  using MinaryLib.DataTypes;
  using System;
  using System.IO;
  using System.Windows.Forms;


  public partial class CreateTemplate : Form
  {

    #region MEMBERS

    private MinaryMain minaryMain;
    private Task.TemplateHandler taskLayerCreateTemplate;

    #endregion


    #region PUBLIC

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTemplate"/> class.
    ///
    /// </summary>
    public CreateTemplate(MinaryMain minaryMain)
    {
      this.InitializeComponent();
      this.minaryMain = minaryMain;
      this.taskLayerCreateTemplate = new Minary.Template.Task.TemplateHandler(this.minaryMain);
    }

    #endregion
 

    #region EVENTS

    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BT_Cancel_Click(object sender, EventArgs e)
    {
      base.Dispose(true);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Bt_Save_Click(object sender, EventArgs e)
    {
      // Create and populate temlate object
      RecordMinaryTemplate newTemplateData = new RecordMinaryTemplate();
      newTemplateData.TemplateConfig.Name = this.tb_TemplateName.Text;
      newTemplateData.TemplateConfig.Description = this.tb_TemplateDescription.Text;
      newTemplateData.TemplateConfig.Reference = this.tb_TemplateReferenceLink.Text;
      newTemplateData.TemplateConfig.Author = this.tb_AuthorName.Text;
      newTemplateData.TemplateConfig.Version = this.tb_Version.Text;

      // Configure attack settings
      int tmpNumberSelectedTargetSystems = -1;
      int.TryParse(this.tb_MaxNoTargetSystems.Text, out tmpNumberSelectedTargetSystems);
      newTemplateData.AttackConfig.NumberSelectedTargetSystems = tmpNumberSelectedTargetSystems;
      newTemplateData.AttackConfig.ScanNetwork = this.cb_ArpScan.Checked ? 1 : 0;
      newTemplateData.AttackConfig.StartAttack = this.cb_StartAttackingTargets.Checked ? 1 : 0;

      try
      {
        this.TemplateParametersAreValid(newTemplateData);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Invalid template data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      // Plugin data
      foreach (TabPage tmpTabPage in this.minaryMain.MinaryTabPageHandler.VisibleTabPages)
      {
        Minary.DataTypes.MinaryExtension tmpExtension = this.minaryMain.PluginHandler.TabPagesCatalog[tmpTabPage.Text];
        TemplatePluginData tmpPluginData = tmpExtension.PluginObject.OnGetTemplateData();
        newTemplateData.Plugins.Add(new Plugin(tmpTabPage.Text, tmpPluginData));
      }

      // Show save file dialog
      this.sfd_TemplateFile.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), Minary.Config.CustomTemplatesDir);
      this.sfd_TemplateFile.Filter = string.Format("Minary template files|*.{0}", Minary.Config.MinaryFileExtension);
      this.sfd_TemplateFile.Title = string.Format("Export current configuration to a .{0} file", Minary.Config.MinaryFileExtension);
      this.sfd_TemplateFile.AddExtension = true;
      DialogResult dialogResult = this.sfd_TemplateFile.ShowDialog();

      if (dialogResult == DialogResult.Cancel)
      {
        return;
      }
      else if (string.IsNullOrEmpty(this.sfd_TemplateFile.FileName))
      {
        MessageBox.Show("You didn't define an output file", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      // Save template to file system
      try
      {
        // Add .mry extension if missing
        string minaryFileExtension = string.Format(".{0}", Minary.Config.MinaryFileExtension);
        if (!this.sfd_TemplateFile.FileName.ToLower().EndsWith(minaryFileExtension))
        {
          this.sfd_TemplateFile.FileName += minaryFileExtension;
        }

        // Serialize and save template
        this.taskLayerCreateTemplate.SaveAttackTemplate(newTemplateData, this.sfd_TemplateFile.FileName);
        MessageBox.Show(string.Format("Template \"{0}\" was saved successfully.", newTemplateData.TemplateConfig.Name), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        base.Dispose(true);
      }
      catch (Exception ex)
      {
        LogConsole.Main.LogConsole.LogInstance.LogMessage("Error occurred while saving template : {0}", ex.Message);
        MessageBox.Show(string.Format("Error occurred while saving template : {0}", ex.Message), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="newTemplateData"></param>
    /// <returns></returns>
    private bool TemplateParametersAreValid(RecordMinaryTemplate newTemplateData)
    {
      if (newTemplateData == null)
      {
        throw new Exception("Template object is invalid");
      }

      if (newTemplateData.TemplateConfig == null)
      {
        throw new Exception("Config section is invalid");
      }

      if (string.IsNullOrEmpty(newTemplateData.TemplateConfig.Name))
      {
        throw new Exception("The template name is invalid");
      }

      if (string.IsNullOrEmpty(newTemplateData.TemplateConfig.Description))
      {
        throw new Exception("The template description is invalid");
      }

      if (string.IsNullOrEmpty(newTemplateData.TemplateConfig.Version))
      {
        throw new Exception("The tmeplate version is invalid");
      }

      if (string.IsNullOrEmpty(newTemplateData.TemplateConfig.Reference))
      {
        throw new Exception("The template reference URL is invalid");
      }

      Uri uriResult;
      if (!Uri.TryCreate(newTemplateData.TemplateConfig.Reference, UriKind.Absolute, out uriResult) ||
          !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
      {
        throw new Exception("The template reference URL is invalid");
      }

      if (string.IsNullOrEmpty(newTemplateData.TemplateConfig.Author))
      {
        throw new Exception("The template author is invalid");
      }

      if (newTemplateData.AttackConfig.NumberSelectedTargetSystems <= 0)
      {
        throw new Exception("The \"Maximum number target systems\" is invalid");
      }

      return true;
    }

    #endregion


    #region PROTECTED

    /// <summary>
    /// Close Sessions GUI on Escape.
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    protected override bool ProcessDialogKey(Keys keyData)
    {
      if (keyData == Keys.Escape)
      {
        this.Dispose(true);
        return true;
      }
      else
      {
        return base.ProcessDialogKey(keyData);
      }
    }

    #endregion

  }
}