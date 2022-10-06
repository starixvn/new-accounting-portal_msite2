using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class UpdateLogStatusCriteria : NonQueryCommand
{
    public int LogId { get; set; }

    public string Action { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDBServiceLog_Del_{DbServer}";

    public override bool IsValid() => LogId > 0 && !string.IsNullOrWhiteSpace(Action);
}