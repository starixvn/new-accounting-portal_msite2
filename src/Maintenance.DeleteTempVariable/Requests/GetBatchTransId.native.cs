namespace Maintenance.DeleteTempVariable.Requests;

public class GetBatchTransIdRequest
{
    public int MatchId { get; set; }
    public string TableName { get; set; }
    public string TypeUpdate { get; set; }
    public string ServerName { get; set; }
}