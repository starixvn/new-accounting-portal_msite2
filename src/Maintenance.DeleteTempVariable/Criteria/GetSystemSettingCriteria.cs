using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class GetSystemSettingCriteria : CriteriaBase
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SystemSettingsSel_{DbServer}";

    public override bool IsValid() => true;
}