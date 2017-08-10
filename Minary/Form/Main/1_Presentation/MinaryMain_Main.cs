namespace Minary.Form
{
  using Minary.Certificates.Presentation;
  using Minary.Common;
  using Minary.Common.Associations;
  using Minary.DataTypes.Interface;
  using Minary.Domain.AttackService;
  using Minary.Domain.Input;
  using Minary.Domain.MacVendor;
  using Minary.Domain.Main;
  using Minary.Form.ArpScan.DataTypes;
  using Minary.LogConsole.Main;
  using Minary.MiniBrowser;
  using MinaryLib.AttackService;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using System.Net.NetworkInformation;
  using System.Threading;
  using System.Windows.Forms;
  using TemplateTask = Template.Task;


  public partial class MinaryMain : Form
  {

    #region MEMBERS

    private string[] commandLineArguments;
    private NetworkInterface[] allAttachednetworkInterfaces;
    private BindingList<PluginTableRecord> usedPlugins;
    private BindingList<string> targetList;
    private TemplateTask.TemplateHandler templateTaskLayer;
    private string currentIpAddress;
    private Browser miniBrowser;
    private bool attackStarted;
    private Minary.Form.TaskFacade minaryTaskFacade;
    private Dictionary<string, PictureBox> attackServiceMap = new Dictionary<string, PictureBox>();

    // GUI handlers
    private ArpScan.Presentation.ArpScan arpScanHandler;

    // Service handlers
    private ManageServerCertificates caCertificateHandler;
    private InputHandler inputModuleHandler;
    private AttackServiceHandler attackServiceHandler;
    private MacVendorHandler macVendorHandler;
    private MinaryProcess minaryProcessHandler;
    private PluginHandler pluginHandler;
    private TabPageHandler tabPageHandler;

    #endregion


    #region PROPERTIES

    public string[] CommandLineArguments { get { return this.commandLineArguments; } }

    public string CurrentLocalIp { get { return this.currentIpAddress ?? string.Empty; } }

    public string CurrentGatewayIp { get { return this.tb_GatewayIp.Text; } }

    public string NetworkStartIp { get { return this.tb_NetworkStartIp.Text; } set { } }

    public string NetworkStopIp { get { return this.tb_NetworkStopIp.Text; } set { } }

    public Minary.Form.TaskFacade MinaryTaskFacade { get { return this.minaryTaskFacade; } }

    public PluginHandler PluginHandler { get { return this.pluginHandler; } }

    public BindingList<PluginTableRecord> UsedPlugins { get { return this.usedPlugins; } }

    public TabPageHandler MinaryTabPageHandler { get { return this.tabPageHandler; } }

    public AttackServiceHandler MinaryAttackServiceHandler { get { return this.attackServiceHandler; } set { } }

    public Minary.Form.ArpScan.Presentation.ArpScan ArpScan { get { return this.arpScanHandler; } set { } }

    public MacVendorHandler MacVendor { get { return this.macVendorHandler; } set { } }

    #endregion


    #region PUBLIC

    public MinaryMain(string[] args)
    {
      this.InitializeComponent();

      // Initialize logging
      Config.CollectSystemInformation();
      LogCons.Inst.DumpSystemInformation();

      this.usedPlugins = new BindingList<PluginTableRecord>();
      this.targetList = new BindingList<string>();

      this.commandLineArguments = args;

      // Set .mry file extension association
      MryFiles.InstallMryFileAssociation();
    }


    public void StartAllHandlers()
    {
      this.attackServiceHandler = new AttackServiceHandler(this);
      this.pluginHandler = new PluginHandler(this);
      this.inputModuleHandler = new InputHandler(this);
      this.caCertificateHandler = new ManageServerCertificates(this);
      this.tabPageHandler = new TabPageHandler(this.tc_Plugins, this);
      this.macVendorHandler = new MacVendorHandler();
      this.minaryProcessHandler = new MinaryProcess();
      
    }


    public void StartBackgroundThreads()
    {
      // Start data input thread.
      this.inputModuleHandler.StartInputThread();

      // Check if new Minary version is available
      Thread updateThread = new Thread(delegate ()
      {
        Minary.Common.Updates.CheckForMinaryUpdates();
      });
      updateThread.Start();

      // Download attack pattern updates
      Thread syncThread = new Thread(delegate ()
      {
        Minary.Common.Updates.SyncAttackPatterns();
      });
      syncThread.Start();
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
      this.bgwOnStartAttack = new BackgroundWorker() { WorkerSupportsCancellation = true };
      this.bgwOnStartAttack.DoWork += new DoWorkEventHandler(this.BGW_OnStartAttack);
      this.bgwOnStartAttack.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BGW_OnStartAttackCompleted);
      
      // Set AttackStop events
      this.bgwOnStopAttack = new BackgroundWorker() { WorkerSupportsCancellation = true };
      this.bgwOnStopAttack.DoWork += new DoWorkEventHandler(this.BGW_OnStopAttack);
      this.bgwOnStopAttack.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BGW_OnStopAttackCompleted);

      // Instantiate (own + foreign) application layers
      this.minaryTaskFacade = new TaskFacade(this, this.dgv_MainPlugins);
      this.templateTaskLayer = new Template.Task.TemplateHandler(this);
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

      // Populate network interface.
      this.LoadNicSettings();

      // Init ArpScan console
      this.arpScanHandler = new ArpScan.Presentation.ArpScan(this);
      this.attackStarted = false;

      // Load and initialize all plugins
      this.pluginHandler.LoadPlugins();

      // Check if an other instance is running.
      this.minaryProcessHandler.HandleRunningInstances();
    }


    public void SetInitialMinaryState()
    {
      // The click event handlers need an initial value
      this.Bt_Attack_Click = this.Bt_Attack_Click_Event;
      this.Bt_ScanLan_Click = this.Bt_ScanLan_Click_Event;
 
      this.InjectMinaryStateDependency();
    }


    public string GetCurrentInterface()
    {
      string retVal = string.Empty;

      try
      {
        retVal = NetworkFunctions.GetNetworkInterfaceIdByIndexNumber(this.cb_Interfaces.SelectedIndex);
      }
      catch (Exception)
      {
      }

      return retVal;
    }


    public void PassNewTargetListToPlugins()
    {
      List<Tuple<string, string, string>> newTargetList = new List<Tuple<string, string, string>>();
      List<TargetRecord> reachableTargetSystems = this.arpScanHandler.TargetList.ToList();

      if (reachableTargetSystems == null || reachableTargetSystems.Count <= 0)
      {
        return;
      }

      foreach (TargetRecord targetSystem in reachableTargetSystems)
      {
        try
        {
          newTargetList.Add(new Tuple<string, string, string>(targetSystem.IpAddress, targetSystem.MacAddress, targetSystem.Vendor));
        }
        catch (Exception)
        {
        }
      }

      // newTargetList.Add("0.0.0.0");
      foreach (string tmpKey in this.pluginHandler.TabPagesCatalog.Keys)
      {
        try
        {
          LogCons.Inst.Write("Minary: Passing new target list to plugin \"{0}\". Total no. targets={1}", tmpKey, newTargetList.Count());
          this.pluginHandler.TabPagesCatalog[tmpKey].PluginObject.SetTargets(newTargetList.ToList());
        }
        catch (Exception ex)
        {
          LogCons.Inst.Write("Minary: {0}\r\n{1}", ex.Message, ex.StackTrace);
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

      // Set service status on GUI footer
      ////      this.SetNewState(serviceName, Status.Error);
      //// this.attackServiceHandler.AttackServices[serviceName].Status = MinaryLib.AttackService.ServiceStatus.Error;

//      this.Cursor = Cursors.WaitCursor;
      this.EnableGuiElements();

      if (!this.bgwOnStopAttack.IsBusy)
      {
        this.bgwOnStopAttack.RunWorkerAsync();
      }

      // Set service status
//      this.attackStarted = false;
//  this.bt_Attack.BackgroundImage = (System.Drawing.Image)Minary.Properties.Resources.StartBig;
//      this.Cursor = Cursors.Default;

      this.SetNewAttackServiceState(serviceName, ServiceStatus.Error);


      // Report service failure
      string message = string.Format("The attack service \"{0}\" failed unexpectedly", serviceName);
      MessageDialog.ShowWarning("Attack service error", message, this);
    }


    public void SetNewAttackServiceState(string serviceName, ServiceStatus newStatus)
    {
      if (string.IsNullOrEmpty(serviceName) ||
          this.attackServiceHandler?.AttackServices == null)
      {
        return;
      }
      
      if (this.attackServiceHandler.AttackServices.ContainsKey(serviceName) == false)
      {
        LogCons.Inst.Write("AttackServiceHandler.SetNewState(): Attack service \"{0}\" was never registered", serviceName);
        return;
      }

      int tmpNewServiceStatus = (newStatus >= 0) ? (int)newStatus : (int)MinaryLib.AttackService.ServiceStatus.NotRunning;

      this.attackServiceMap[serviceName].Image = this.il_AttackServiceStat.Images[tmpNewServiceStatus];
      LogCons.Inst.Write("AttackServiceHandler.SetNewState(): {0} has new state \"{1}\"", serviceName, newStatus.ToString());
    }


    public void RegisterService(string serviceName)
    {
      if (string.IsNullOrEmpty(serviceName))
      {
        return;
      }

      foreach (PictureBox guiElement in this.Controls.OfType<PictureBox>())
      {
        if (guiElement.Tag != null && guiElement.Tag.ToString() == serviceName)
        {
          this.attackServiceMap.Add(serviceName, guiElement);
          LogCons.Inst.Write("AttackServiceHandler.RegisterService(): Registered attack service {0}, linked to label {1}", serviceName, guiElement.Name);
          break;
        }
      }
    }

    #endregion


    #region PRIVATE

    private void InjectMinaryStateDependency()
    {
      IMinaryState minaryState = MinaryFactory.GetMinaryEventBase(this);
      this.bt_Attack.Click += new EventHandler(minaryState.Bt_Attack_Click);
      this.bt_ScanLan.Click += new EventHandler(minaryState.Bt_ScanLan_Click);
    }

    #endregion

  }
}