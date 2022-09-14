using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class DeleteParlayLockingCriteria : NonQueryCommand
{
    public string Session { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_ParlayLockingDel{DbServer}";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}