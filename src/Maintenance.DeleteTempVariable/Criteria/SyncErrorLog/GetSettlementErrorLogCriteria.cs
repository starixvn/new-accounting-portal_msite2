using Fanex.Data;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Constants;

namespace Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;

public class GetSettlementProcessingErrorLogCriteria : CriteriaBase
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; } = DataBaseServers.DBCentral;

    public string MatchId { get; set; }

    public string Mode { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDB_SettlementProcessingSel_{DbServer}";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}

public class GetSettlementLockErrorLogCriteria : CriteriaBase
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; } = DataBaseServers.DBCentral;

    public string MatchId { get; set; }

    public string Mode { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDB_SettlementLockSel_{DbServer}";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}

public class GetSiteDbSSBErrorLogCriteria : CriteriaBase
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; } = DataBaseServers.DBCentral;

    public string Mode { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDB_SSBErrSel_{DbServer}";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}