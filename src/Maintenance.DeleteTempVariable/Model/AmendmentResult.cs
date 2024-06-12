namespace Maintenance.DeleteTempVariable.Model;

public class AmendmentResult
{
    public int MatchId { get; set; }
    public short StatusId { get; set; }
    public string SessionId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string DbServer { get; set; }
}