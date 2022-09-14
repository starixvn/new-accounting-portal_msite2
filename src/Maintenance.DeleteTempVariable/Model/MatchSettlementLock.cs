namespace Maintenance.DeleteTempVariable.Model;

public class MatchSettlementLock
{
    public int MatchId { get; set; }

    public string SessionId { get; set; }

    public DateTime CreatedTime { get; set; }

    public string HomeTeam { get; set; }

    public string AwayTeam { get; set; }

    public int TotalSettlementProcessing { get; set; }

    public string DBServer { get; set; }

    public string ErrorMessage { get; set; }
}