using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria
{
    public class GetSettlementLockCriteria : CriteriaBase
    {
        public int? MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_SSBErrorMessageSel";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}