using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria
{
    public class UpdateSystemSettingCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "NAP_Set_SystemSettingsUpd";
        }

        public override bool IsValid() => true;
    }
}