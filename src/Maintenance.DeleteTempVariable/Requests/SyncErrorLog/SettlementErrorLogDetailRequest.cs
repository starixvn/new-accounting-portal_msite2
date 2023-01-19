namespace Maintenance.DeleteTempVariable.Requests;

public class SettlementErrorLogDetailRequest
{
    public long SessionId { get; set; }
    public short SiteDBGroupId { get; set; }
    public string DbServer { get; set; }
}
