namespace Minary.Domain.SystemComponents
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Configuration.Install;
  using System.Linq;
  using System.Runtime.InteropServices;
  using System.ServiceProcess;


  public class MSMQHandler : Installer
  {

    #region IMPORTS

    [DllImport("kernel32")]
    public static extern IntPtr LoadLibrary(string lpFileName);


    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool FreeLibrary(IntPtr hModule);

    #endregion


    #region MEMBERS

    private ServiceController msQue;

    #endregion


    #region PUBLIC

    public MSMQHandler()
    {
      // Find Message Queue service
      List<ServiceController> services = ServiceController.GetServices().ToList();
      this.msQue = services.Find(o => o.ServiceName == "MSMQ");
    }


    public bool IsMSMQInstalled()
    {
      return this.msQue != null;
    }


    public void StartService()
    {
      if (this.msQue == null)
      {
        throw new Exception("MSMQ service is not installed");
      }

      if (this.msQue.Status == ServiceControllerStatus.Running ||
          this.msQue.Status == ServiceControllerStatus.StartPending ||
          this.msQue.Status == ServiceControllerStatus.ContinuePending)
      {
        return;
      }

      this.msQue.Start();
    }


    public void StopService()
    {
      if (this.msQue == null)
      {
        throw new Exception("MSMQ service is not installed");
      }

      if (this.msQue.Status == ServiceControllerStatus.Stopped ||
          this.msQue.Status == ServiceControllerStatus.StopPending)
      {
        return;
      }

      this.msQue.Stop();
    }


    public bool IsMSMQRunning()
    {
      if (this.msQue != null &&
          this.msQue.Status == ServiceControllerStatus.Running)
      {
        return true;
      }

      return false;
    }


    public void Install()
    {
      this.Install(new System.Collections.Hashtable());
    }


    public override void Install(IDictionary stateSaver)
    {
      base.Install(stateSaver);
      bool loaded;

      try
      {
        IntPtr handle = LoadLibrary("Mqrt.dll");

        if (handle == IntPtr.Zero || handle.ToInt32() == 0)
        {
          loaded = false;
        }
        else
        {
          loaded = true;

          FreeLibrary(handle);
        }
      }
      catch
      {
        loaded = false;
      }

      if (!loaded)
      {
        if (Environment.OSVersion.Version.Major < 6) // Windows XP or earlier
        {
          string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "MSMQAnswer.ans");

          using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName))
          {
            writer.WriteLine("[Version]");
            writer.WriteLine("Signature = \"$Windows NT$\"");
            writer.WriteLine();
            writer.WriteLine("[Global]");
            writer.WriteLine("FreshMode = Custom");
            writer.WriteLine("MaintenanceMode = RemoveAll");
            writer.WriteLine("UpgradeMode = UpgradeOnly");
            writer.WriteLine();
            writer.WriteLine("[Components]");
            writer.WriteLine("msmq_Core = ON");
            writer.WriteLine("msmq_LocalStorage = ON");
          }

          using (System.Diagnostics.Process p = new System.Diagnostics.Process())
          {
            System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo("sysocmgr.exe", "/i:sysoc.inf /u:\"" + fileName + "\"");

            p.StartInfo = start;
            p.Start();
            p.WaitForExit();
          }
        }
        else // Vista or later
        {
          using (System.Diagnostics.Process p = new System.Diagnostics.Process())
          {
            System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo("ocsetup.exe", "MSMQ-Container;MSMQ-Server /passive");

            p.StartInfo = start;

            p.Start();
            p.WaitForExit();
          }
        }
      }
    }

    #endregion

  }
}
