namespace Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;

public class UpdateSiteDbSSBErrorLogCriteria
{
    public string SessionId { get; set; }
    public string SiteDBGroupId { get; set; }
    public char Action { get; set; } = 'D';
}
