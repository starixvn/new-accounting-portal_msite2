using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class UpdateSettlementLogCriteria : NonQueryCommand
{
    public Int64 SessionId { get; set; }

    public DateTime StartTime { get; set; }

    public string MessageId { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SSBSettlementLogUpd_{DbServer}";

    public override bool IsValid() => true;
}