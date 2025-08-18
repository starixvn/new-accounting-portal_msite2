using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class DeleteMatchCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public int KeyId { get; set; }

    public string MarketId { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_MatchLockingDel_{DbServer}";

    public override bool IsValid() => MatchId > 0;
}