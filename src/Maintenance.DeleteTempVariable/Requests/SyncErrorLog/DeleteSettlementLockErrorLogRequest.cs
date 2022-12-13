namespace Maintenance.DeleteTempVariable.Requests.SyncErrorLog;

public class DeleteSettlementLockErrorLogRequest
{
    public string DbServer { get; set; }
    public string SessionId { get; set; }
    public string SiteDBGroupId { get; set; }
}
