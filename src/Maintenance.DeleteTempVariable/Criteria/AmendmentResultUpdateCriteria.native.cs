using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class AmendmentResultUpdateCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public short StatusId { get; set; }

    public string SessionIds { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_SettlementResultHistory_Update_{DbServer}";

    public override bool IsValid() => MatchId > 0;
}