using System.Collections.Generic;

namespace MinaryLib.AttackService
{
  public interface IAttackService
  {

    string ServiceName { get; set; }

    string WorkingDirectory { get; set; }

    Dictionary<string, SubModule> SubModules { get; set; }

    ServiceStatus Status { get; set; }

    ServiceStatus StartService(ServiceParameters serviceParameters);

    ServiceStatus StopService();

  }
}
