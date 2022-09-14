namespace Maintenance.DeleteTempVariable.Requests;

public class ParlayLockingRequest
{
    public int? MatchId { get; set; }

    public int TimeRange { get; set; }
}