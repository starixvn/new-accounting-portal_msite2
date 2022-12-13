using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;

public class DeleteSettlementLockErrorLogCriteria : NonQueryCommand
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; }
    public string SessionId { get; set; }
    public string SiteDBGroupId { get; set; }
    public override string GetSettingKey() => $"NAP_Set_SiteDB_SettlementLockDel_{DbServer}";
    public override bool IsValid() => true;
}
