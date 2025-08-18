using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class GetTicketCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    public int KeyId { get; set; }

    public string Action { get; set; }

    public string MarketId { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_MatchLockingTicketSel_{DbServer}";

    public override bool IsValid() => MatchId > 0;
}