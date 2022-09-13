using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria
{
    public class DeleteMatchCriteria : NonQueryCommand
    {
        public int? MatchId { get; set; }

        public int KeyId { get; set; }

        public string MarketId { get; set; }

        public override string GetSettingKey() => "NAP_Set_MatchLockingDel";

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}