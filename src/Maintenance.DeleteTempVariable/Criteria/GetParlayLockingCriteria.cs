using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class GetParlayLockingCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    public int TimeRange { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_ParlayLockingSel_{DbServer}";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}