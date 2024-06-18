namespace Maintenance.Sync.Models;

public class SynMatchIdInfo
{
    public int RowNumber { get; set; }
    public string MinTransId { get; set; }
    public string MaxTransId { get; set; }
    public int MatchId { get; set; }
    public string TableName { get; set; }
    public string ServerName { get; set; }
}