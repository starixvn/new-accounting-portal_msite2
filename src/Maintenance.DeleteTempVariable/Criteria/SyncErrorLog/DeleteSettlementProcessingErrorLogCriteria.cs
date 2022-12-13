using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria.SyncErrorLog
{
    public class DeleteSettlementProcessingErrorLogCriteria : NonQueryCommand
    {
        [SpParam(Ignored = true)]
        public string DbServer { get; set; }
        public string SessionId { get; set; }
        public string SiteDBGroupId { get; set; }
        public char Action { get; set; } = 'D';
        public override string GetSettingKey() => $"NAP_Set_SiteDB_SettlementProcessingDel_{DbServer}";
        public override bool IsValid() => true;
    }
}
