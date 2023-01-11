using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;

public class UpdateSiteDbSSBErrorLogCriteria : INonQueryCommand
{
    public string SessionId { get; set; }
    public string SiteDBGroupId { get; set; }
    [SpParam(Ignored = true)]
    public string DbServer { get; set; }
    public string StartTime { get; set; }
    public string MessageId { get; set; }
    public string GetSettingKey() => $"NAP_Set_SSBSettlementLogUpdx_{DbServer}";

    public bool IsValid() => true;
}
