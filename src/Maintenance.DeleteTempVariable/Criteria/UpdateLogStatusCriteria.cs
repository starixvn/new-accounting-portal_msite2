using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class UpdateLogStatusCriteria : NonQueryCommand
{
    public string LogIds { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDBServiceLastLog_Udp_{DbServer}";

    public override bool IsValid() => true;
}