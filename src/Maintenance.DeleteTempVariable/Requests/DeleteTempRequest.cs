namespace Maintenance.DeleteTempVariable.Requests;

public class DeleteTempRequest
{
    public int? MatchId { get; set; }

    public int KeyId { get; set; }

    public string MarketId { get; set; }
}