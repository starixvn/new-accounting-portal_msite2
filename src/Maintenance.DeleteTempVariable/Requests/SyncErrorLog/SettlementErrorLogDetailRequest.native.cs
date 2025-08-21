namespace Maintenance.DeleteTempVariable.Requests.SyncErrorLog;

public class SettlementErrorLogDetailRequest
{
    public long SessionId { get; set; }
    public short SiteDBGroupId { get; set; }
    public string DbServer { get; set; }
    public string Mode { get; set; }
}