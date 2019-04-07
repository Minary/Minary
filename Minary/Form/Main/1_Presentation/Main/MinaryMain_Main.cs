namespace Minary.Form.Main
{
  using Minary.Certificates.Presentation;
  using Minary.Common;
  using Minary.Common.Associations;
  using Minary.DataTypes.Enum;
  using Minary.DataTypes.Interface;
  using Minary.Domain.AttackService;
  using Minary.Domain.InputProcessor;
  using Minary.Domain.MacVendor;
  using Minary.Domain.Main;
  using Minary.Domain.Network;
  using Minary.Form;
  using Minary.LogConsole.Main;
  using Minary.MiniBrowser;
  using MinaryLib.AttackService.Enum;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Windows.Forms;
  using TemplateTask = Template.Task;


  public partial class MinaryMain : Form
  {

    #region MEMBERS

    public BindingList<string> targetList = new BindingList<string>(); // Must be public because Calls() ref params! 
    private static IMinaryState minaryState;
    private string[] commandLineArguments;
    private BindingList<PluginTableRecord> usedPlugins = new BindingList<PluginTableRecord>();
    private TemplateTask.TemplateHandler templateTaskLayer;
    private int currentInterfaceIndex;
    private Browser miniBrowser;
    private bool attackStarted;
    private Minary.Form.Main.TaskFacade minaryTaskFacade;
    private Dictionary<string, PictureBox> attackServiceMap = new Dictionary<string, PictureBox>();
    private Minary.Form.SimpleGUI.SimpleGuiUserControl simpleGui;

    // GUI handlers
    private ArpScan.Presentation.ArpScan arpScanHandler;

    // Service handlers
    private AttackServiceHandler attackServiceHandler;
    private PluginHandler pluginHandler;
    private TabPageHandler tabPageHandler;
    private IInputProcessor inputProcessorHandler;
    private MacVendorHandler macVendorHandler;
    private NetworkInterfaceHandler nicHandler;
    private ManageServerCertificates caCertificateHandler;
    private MinaryProcess minaryProcessHandler;

    #endregion


    #region PROPERTIES

    public bool AttackStarted { get { return this.attackStarted; } }

    public string CurrentLocalIp { get; set; } = string.Empty;

    public string CurrentLocalMac { get; set; } = string.Empty;

    public string CurrentInterfaceId { get; set; } = string.Empty;

    public BindingList<PluginTableRecord> UsedPlugins { get { return this.usedPlugins; } }


    // Proxy properties
    public string CurrentGatewayIp { get { return this.tb_GatewayIp.Text ?? string.Empty; } }

    public string NetworkStartIp { get { return this.tb_NetworkStartIp.Text; } set { } }

    public string NetworkStopIp { get { return this.tb_NetworkStopIp.Text; } set { } }


    // Handlers
    public Minary.Form.Main.TaskFacade MinaryTaskFacade { get { return this.minaryTaskFacade; } }

    public PluginHandler PluginHandler { get { return this.pluginHandler; } }

    public TabPageHandler MinaryTabPageHandler { get { return this.tabPageHandler; } }

    public AttackServiceHandler MinaryAttackServiceHandler { get { return this.attackServiceHandler; } set { } }

    public ArpScan.Presentation.ArpScan ArpScanHandler { get { return this.arpScanHandler; } set { } }

    public MacVendorHandler MacVendorHandler { get { return this.macVendorHandler; } set { } }

    public NetworkInterfaceHandler NetworkHandler { get { return this.nicHandler; } set { } }

    #endregion


    #region PUBLIC

    public MinaryMain(string[] args)
    {
      this.InitializeComponent();

      this.commandLineArguments = args;
    }


    public void StartAllHandlers()
    {
      this.attackServiceHandler = new AttackServiceHandler(this);
      this.pluginHandler = new PluginHandler(this);
      this.tabPageHandler = new TabPageHandler(this.tc_Plugins, this);
      this.inputProcessorHandler = new HandlerNamedPipe(this);
 //     this.inputProcessorHandler = new HandlerMessageQueue(this);
      this.nicHandler = new NetworkInterfaceHandler(this);
      this.caCertificateHandler = new ManageServerCertificates(this);
      this.macVendorHandler = new MacVendorHandler();
      this.minaryProcessHandler = new MinaryProcess();
    }


    public void StartBackgroundThreads()
    {
      // Start data input thread.
      try
      {
        this.inputProcessorHandler.StartInputProcessing();
      }
      catch (Exception ex)
      {
        var message = $"An error occurred while starting the input processor NamedPipe : {ex.Message}" +
                      "\r\n\r\nAborting Minary now.";
        MessageDialog.Inst.ShowError(string.Empty, message, this);
        this.ShutDownMinary();
      }

      // Check if new Minary version is available
      Task.Run(() =>
      {
        string autoupdateStateStr = WinRegistry.GetValue("Updates", "Autoupdate");
        int autoupdateState = Convert.ToInt32(autoupdateStateStr);

        if (autoupdateState > 0)
        {
          Updates.Presentation.FormCheckNewVersion newVersionCheck = new Updates.Presentation.FormCheckNewVersion();
          newVersionCheck.ShowDialog();
        }
      });
    }


    public void LoadAllFormElements()
    {
      DataGridViewTextBoxColumn columnPluginName = new DataGridViewTextBoxColumn();
      columnPluginName.DataPropertyName = "PluginName";
      columnPluginName.Name = "PluginName";
      columnPluginName.HeaderText = "Plugin name";
      columnPluginName.ReadOnly = true;
      columnPluginName.Width = 200;
      this.dgv_MainPlugins.Columns.Add(columnPluginName);

      DataGridViewCheckBoxColumn columnActivated = new DataGridViewCheckBoxColumn();
      columnActivated.DataPropertyName = "Active";
      columnActivated.Name = "Active";
      columnActivated.HeaderText = "Activated";
      columnActivated.FalseValue = "0";
      columnActivated.TrueValue = "1";
      columnActivated.Visible = true;
      this.dgv_MainPlugins.Columns.Add(columnActivated);

      DataGridViewTextBoxColumn columnType = new DataGridViewTextBoxColumn();
      columnType.DataPropertyName = "PluginType";
      columnType.Name = "PluginType";
      columnType.HeaderText = "Type";
      columnType.Visible = true;
      this.dgv_MainPlugins.Columns.Add(columnType);

      DataGridViewTextBoxColumn columnPluginDescription = new DataGridViewTextBoxColumn();
      columnPluginDescription.DataPropertyName = "PluginDescription";
      columnPluginDescription.HeaderText = "Description";
      columnPluginDescription.ReadOnly = true;
      columnPluginDescription.Width = 120;
      columnPluginDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.dgv_MainPlugins.Columns.Add(columnPluginDescription);
      this.dgv_MainPlugins.DataSource = this.usedPlugins;

      // Set AttackStart events
      this.bgw_OnStartAttack = new BackgroundWorker() { WorkerSupportsCancellation = true };
      this.bgw_OnStartAttack.DoWork += new DoWorkEventHandler(this.BGW_OnStartAttack);
      this.bgw_OnStartAttack.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BGW_OnStartAttackCompleted);

      // Instantiate (own + foreign) application layers
      this.minaryTaskFacade = new TaskFacade(this, this.dgv_MainPlugins);
      this.templateTaskLayer = new Template.Task.TemplateHandler(this);

      // Create SimpleGUI
      this.simpleGui = new Minary.Form.SimpleGUI.SimpleGuiUserControl();
      this.simpleGui.Visible = true;
      this.Controls.Add(simpleGui);
    }


    public void PreRun()
    {
      // Set current Debugging mode in GUI
      if (Debugging.IsDebuggingOn)
      {
        this.tsmi_Debugging.Text = "Debugging (is on)";
        this.SetAppTitle("Debugging");
      }
      else
      {
        this.tsmi_Debugging.Text = "Debugging (is off)";
        this.SetAppTitle(string.Empty);
      }

      // Set current SimpleGUI mode in MainForm
      this.tsmi_Debugging.Text = string.Format("Debugging (is {0})", Config.IsSimpleGuiOn?"on":"off");

      // Populate network interface.
      this.LoadNicSettings();

      // Init ArpScan console
      this.arpScanHandler = new ArpScan.Presentation.ArpScan(this);
      this.attackStarted = false;

      // Check if an other instance is running.
      this.minaryProcessHandler.HandleRunningInstances();

      // Load and initialize all plugins
      this.pluginHandler.LoadPlugins();

      // Load and initialize all attack services
      this.attackServiceHandler.LoadAttackServicePlugins();

      // Initialize logging
      Config.CollectSystemInformation();
      LogCons.Inst.DumpSystemInformation();

      // Set .mry file extension association
      MryFiles.InstallMryFileAssociation();
    }


    public void SetMinaryState()
    {
      minaryState = SystemStateCheck.GetMinaryEventBase(this);
      minaryState.LoadState();
    }


    public delegate string GetCurrentInterfaceDelegate();
    public string GetCurrentInterface()
    {
      if (this.InvokeRequired == true)
      {
        this.BeginInvoke(new GetCurrentInterfaceDelegate(this.GetCurrentInterface), new object[] { });
        return string.Empty;
      }

      string retVal = this.nicHandler.GetNetworkInterfaceIdByIndex(this.cb_Interfaces.SelectedIndex);
      return retVal;
    }


    public void PassNewTargetListToPlugins()
    {
      var newTargetList = new List<Tuple<string, string, string>>();
      var reachableTargetSystems = this.arpScanHandler.TargetList.ToList();

      if (reachableTargetSystems == null || 
          reachableTargetSystems.Count <= 0)
      {
        return;
      }

      foreach (var targetSystem in reachableTargetSystems)
      {
        try
        {
          newTargetList.Add(new Tuple<string, string, string>(targetSystem.IpAddress, targetSystem.MacAddress, targetSystem.Vendor));
        }
        catch (Exception)
        {
        }
      }
      
      foreach (var tmpKey in this.pluginHandler.TabPagesCatalog.Keys)
      {
        try
        {
          LogCons.Inst.Write(LogLevel.Debug, $"Minary: Passing new target list to plugin \"{tmpKey}\". Total no. targets={newTargetList.Count()}");
          this.pluginHandler.TabPagesCatalog[tmpKey].PluginObject.SetTargets(newTargetList.ToList());
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write(LogLevel.Error, $"Minary: {ex.Message}\r\n{ex.StackTrace}");
        }
      }
    }


    public delegate void OnServiceExicedUnexpectedlyDelegate(string serviceName);
    public void OnServiceExicedUnexpectedly(string serviceName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new OnServiceExicedUnexpectedlyDelegate(this.OnServiceExicedUnexpectedly), new object[] { serviceName });
        return;
      }

      // Reset GUI
      this.EnableGuiElements();
      this.StopAttack();
      this.SetNewAttackServiceState(serviceName, ServiceStatus.Error);

      // Report service failure
      var message = $"The attack service \"{serviceName}\" failed unexpectedly";
      MessageDialog.Inst.ShowWarning("Attack service error", message, this);
    }


    public delegate void SetNewAttackServiceStateDelegate(string serviceName, ServiceStatus newStatus);
    public void SetNewAttackServiceState(string serviceName, ServiceStatus newStatus)
    {
      if (this.InvokeRequired == true)
      {
        this.BeginInvoke(new SetNewAttackServiceStateDelegate(this.SetNewAttackServiceState), new object[] { serviceName, newStatus });
        return;
      }

      if (string.IsNullOrEmpty(serviceName) ||
          this.attackServiceHandler?.AttackServices == null)
      {
        return;
      }

      if (this.attackServiceHandler.AttackServices.ContainsKey(serviceName) == false)
      {
        LogCons.Inst.Write(LogLevel.Warning, $"AttackServiceHandler.SetNewState(): Attack service \"{serviceName}\" was never registered");
        return;
      }

      int tmpNewServiceStatus = (newStatus >= 0) ? (int)newStatus : (int)ServiceStatus.NotRunning;

      this.attackServiceMap[serviceName].Image = this.il_AttackServiceStat.Images[tmpNewServiceStatus];
      LogCons.Inst.Write(LogLevel.Info, $"AttackServiceHandler.SetNewState(): {serviceName} has new state \"{newStatus.ToString()}\"");
    }


    public void RegisterAttackService(string attackServiceName)
    {
      Label tmpLabel = new Label() { Name = attackServiceName, Text = attackServiceName, Tag = attackServiceName };
      tmpLabel.AutoSize = true;

      PictureBox tmpPictureBox = new PictureBox() { Name = attackServiceName, Text = attackServiceName, Tag = attackServiceName };
      tmpPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

      // Append new attack service to (flow layout) panel
      this.flp_AttackServices.Controls.Add(tmpPictureBox);
      this.flp_AttackServices.Controls.Add(tmpLabel);

      // Cache new attack service record
      this.attackServiceMap.Add(attackServiceName, tmpPictureBox);

      // Set neutral attack service state
      this.SetNewAttackServiceState(attackServiceName, ServiceStatus.NotRunning);
    }

    #endregion

  }
}
