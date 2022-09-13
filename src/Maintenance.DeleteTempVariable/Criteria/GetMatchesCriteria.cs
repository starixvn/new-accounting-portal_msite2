using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria
{
    public class GetMatchesCriteria : CriteriaBase
    {
        public int? MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_MatchLockingSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}