using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;

public class GetSettlementErrorLogDetailsCriteria : CriteriaBase
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public long SessionId { get; set; }

    public short SiteDBGroupId { get; set; }
    public string Mode { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDB_SettlementProcessingDetailSel_{DbServer}";

    public override bool IsValid() => SessionId != 0;
}