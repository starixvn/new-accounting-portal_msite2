using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.Jacktime.Criterias;

public class ByPassHybridParlayTicketSel : CriteriaBase
{
    public int MatchId { get; set; }

    [SpParam(Ignored = true)]
    public string Server { get; set; } = string.Empty;

    public override string GetSettingKey() => $"NAP_Admin_ByPassHybridParlay_TicketSel_{Server}";

    public override bool IsValid() => true;
}
