using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria
{
    public class GetParlayLockingCriteria : CriteriaBase
    {
        public int? MatchId { get; set; }

        public int TimeRange { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_ParlayLockingSel";
        }

        public override bool IsValid() => true;
    }
}