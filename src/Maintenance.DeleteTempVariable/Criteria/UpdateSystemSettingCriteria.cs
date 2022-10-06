using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class UpdateSystemSettingCriteria : NonQueryCommand
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SystemSettingsUpd_{DbServer}";

    public override bool IsValid() => true;
}