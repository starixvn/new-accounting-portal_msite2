namespace Maintenance.DeleteTempVariable.Requests.SyncErrorLog;

public class DeleteSettlementProcessingErrorLogRequest
{
    public string DbServer { get; set; }
    public string SessionId { get; set; }
    public string SiteDBGroupId { get; set; }
    public char Action { get; set; } = 'D';
    public string Mode { get; set; }
}