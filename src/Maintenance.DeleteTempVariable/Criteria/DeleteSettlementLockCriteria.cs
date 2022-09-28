using Fanex.Data;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Constants;

namespace Maintenance.DeleteTempVariable.Criteria;

public class DeleteSettlementLockCriteria : NonQueryCommand
{
    public long SessionId { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    [SpParam(Ignored = true)]
    public bool IsSettleLock { get; set; }

    public override string GetSettingKey()
    {
        return IsSettleLock
            ? GetSPName("NAP_Set_SettlementLockDel")
            : GetSPName("NAP_Set_SettlementProcessingDel");
    }

    private string GetSPName(string name)
    {
        return string.Equals(DbServer, DataBaseServers.DBTrans)
            ? name
            : $"{name}_{DbServer}";
    }

    public override bool IsValid() => true;
}