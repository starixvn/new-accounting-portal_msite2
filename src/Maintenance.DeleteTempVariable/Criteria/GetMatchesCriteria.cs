using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class GetMatchesCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_MatchLockingSel_{DbServer}";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}