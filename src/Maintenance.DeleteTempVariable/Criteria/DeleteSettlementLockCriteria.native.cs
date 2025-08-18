using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class DeleteSettlementLockCriteria : NonQueryCommand
{
    public long SessionId { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    [SpParam(Ignored = true)]
    public bool IsSettleLock { get; set; }

    public string Mode { get; set; }

    public override string GetSettingKey()
    {
        return IsSettleLock
            ? $"NAP_Set_SettlementLockDel_{DbServer}"
            : $"NAP_Set_SettlementProcessingDel_{DbServer}";
    }

    public override bool IsValid() => true;
}