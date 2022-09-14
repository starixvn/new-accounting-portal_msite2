namespace Maintenance.DeleteTempVariable.Model;

public class MatchParlayLocking
{
    public string ServerName { get; set; }

    public string DbServer { get; set; }

    public int MatchId { get; set; }

    public Int64 TotalAffectedRefno { get; set; }

    public string Session { get; set; }

    public string SPName { get; set; }

    public DateTime CreatedDate { get; set; }
}