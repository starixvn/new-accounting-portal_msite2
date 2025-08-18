namespace Maintenance.DeleteTempVariable.Requests;

public class MatchTicketRequest
{
    public int MatchId { get; set; }

    public string DbServer { get; set; }

    public int KeyId { get; set; }

    public string? ActionName { get; set; }

    public string MarketId { get; set; }
}