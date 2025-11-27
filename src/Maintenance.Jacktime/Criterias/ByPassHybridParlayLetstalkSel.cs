using Fanex.Data.Repository;

namespace Maintenance.Jacktime.Criterias;

public class ByPassHybridParlayLetstalkSel : CriteriaBase
{
    public int MatchId { get; set; }

    public override string GetSettingKey() => "NAP_Admin_ByPassHybridParlay_LetstalkSel";
    public override bool IsValid() => true;
}
