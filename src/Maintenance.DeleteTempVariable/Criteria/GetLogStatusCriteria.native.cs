using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class GetLogStatusCriteria : CriteriaBase
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public long SessionId { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDBServiceLog_Sel_{DbServer}";

    public override bool IsValid() => true;
}