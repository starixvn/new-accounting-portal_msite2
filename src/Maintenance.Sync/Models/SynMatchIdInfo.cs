namespace Maintenance.Sync.Models;

public class SynMatchIdInfo
{
    public int RowNumber { get; set; }
    public long MinTransId { get; set; }
    public long MaxTransId { get; set; }
    public int MatchId { get; set; }
    public string TableName { get; set; }
    public string ServerName { get; set; }
}