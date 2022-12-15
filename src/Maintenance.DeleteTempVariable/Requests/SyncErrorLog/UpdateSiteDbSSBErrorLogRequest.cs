namespace Maintenance.DeleteTempVariable.Requests.SyncErrorLog;

public class UpdateSiteDbSSBErrorLogRequest
{
    public string SessionId { get; set; }
    public string SiteDBGroupId { get; set; }
    public string DbServer { get; set; }
    public string StartTime { get; set; }
}
