using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria
{
    public class GetTicketCriteria : CriteriaBase
    {
        public int MatchId { get; set; }

        public int KeyId { get; set; }

        public string Action { get; set; }

        public string MarketId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_MatchLockingTicketSel";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}