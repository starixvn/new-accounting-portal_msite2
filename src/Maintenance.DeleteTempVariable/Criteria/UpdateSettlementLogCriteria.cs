using Fanex.Data;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Model;

namespace Maintenance.DeleteTempVariable.Criteria;

public class UpdateSettlementLogCriteria : NonQueryCommand
{
    public Int64 SessionId { get; set; }

    public DateTime StartTime { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey()
    {
        return string.Equals(DbServer, DataBaseServers.DBTrans)
            ? "NAP_Set_SSBSettlementLogUpd"
            : $"NAP_Set_SSBSettlementLogUpd_{DbServer}";
    }

    public override bool IsValid() => true;
}