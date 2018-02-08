namespace Minary.Form.Template.Presentation
{
  using Minary.DataTypes.Enum;
  using Minary.Form.Template.DataTypes.Template;
  using Minary.LogConsole.Main;
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
      this.taskLayerCreateTemplate = new Task.TemplateHandler(this.minaryMain);
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
      var newTemplateData = new MinaryTemplateData();
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
        MessageDialog.Inst.ShowError(string.Empty, ex.Message, this);
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
      this.sfd_TemplateFile.Filter = $"Minary template files|*.{Minary.Config.MinaryFileExtension}";
      this.sfd_TemplateFile.Title = $"Export current configuration to a .{Minary.Config.MinaryFileExtension} file";
      this.sfd_TemplateFile.AddExtension = true;
      DialogResult dialogResult = this.sfd_TemplateFile.ShowDialog();

      if (dialogResult == DialogResult.Cancel)
      {
        return;
      }
      else if (string.IsNullOrEmpty(this.sfd_TemplateFile.FileName))
      {
        var message = "You didn't define an output file";
        MessageDialog.Inst.ShowWarning(string.Empty, message, this);
        return;
      }

      // Save template to file system
      try
      {
        this.SaveTemplateToFile(newTemplateData);
      }
      catch (Exception ex)
      {
        LogCons.Inst.Write(LogLevel.Error, $"Error occurred while saving template : {ex.Message}");
        string message = $"Error occurred while saving template : {ex.Message}";
        MessageDialog.Inst.ShowWarning(string.Empty, message, this);
      }
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="newTemplateData"></param>
    /// <returns></returns>
    private bool TemplateParametersAreValid(MinaryTemplateData newTemplateData)
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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="newTemplateData"></param>
    private void SaveTemplateToFile(MinaryTemplateData newTemplateData)
    {
      // Add .mry extension if missing
      var minaryFileExtension = $".{Minary.Config.MinaryFileExtension}";
      if (!this.sfd_TemplateFile.FileName.ToLower().EndsWith(minaryFileExtension))
      {
        this.sfd_TemplateFile.FileName += minaryFileExtension;
      }

      // Serialize and save template
      this.taskLayerCreateTemplate.SaveAttackTemplate(newTemplateData, this.sfd_TemplateFile.FileName);
      var message = $"Template \"{newTemplateData.TemplateConfig.Name}\" was saved successfully.";
      MessageDialog.Inst.ShowInformation(string.Empty, message, this);

      base.Dispose(true);
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