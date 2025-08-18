namespace Maintenance.DeleteTempVariable.Requests;

public class AmendmentResultUpdateRequest
{
    public List<AmendmentResultItem> Results { get; set; }
}

public class AmendmentResultItem
{
    public int MatchId { get; set; }
    public string SessionId { get; set; }
    public string DbServer { get; set; }
    public int StatusId { get; set; }
}