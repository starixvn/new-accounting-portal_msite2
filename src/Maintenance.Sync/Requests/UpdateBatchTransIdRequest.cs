namespace Maintenance.Sync.Requests;

public class UpdateBatchTransIdRequest
{
    public string TableName { get; set; }
    public string ServerName { get; set; }
    public int MatchId { get; set; }
    public long FromTransId { get; set; }
    public long ToTransId { get; set; }
    public string[] ToServerNames { get; set; }
}