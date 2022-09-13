using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria
{
    public class DeleteParlayLockingCriteria : NonQueryCommand
    {
        public string Session { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_ParlayLockingDel";
        }

        public override bool IsValid() => true;
    }
}