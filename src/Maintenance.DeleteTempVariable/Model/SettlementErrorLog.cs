namespace Maintenance.DeleteTempVariable.Model;

public class SettlementErrorLog
{
    public string ServerName { get; set; }
    public string DbServer { get; set; }
    public short? SiteDBGroupId { get; set; }
    public long? SessionId { get; set; }
    public string? MessageId { get; set; }
    public int? MatchId { get; set; }
    public string ErrorMessage { get; set; }
    public string TableName { get; set; }
    public string TempTableName { get; set; }
    public string FunctionName { get; set; }
    public string TicketType { get; set; }
    public string SettlementAction { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public DateTime? CreatedTime { get; set; }
    public string EventName => !string.IsNullOrEmpty(HomeTeam) && !string.IsNullOrEmpty(AwayTeam) ? HomeTeam + " - " + AwayTeam : string.Empty;
    public string[] ErrorMessages => !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage.Split("\\n") : default;

    public string TicketTypeExplicit =>
    TicketType switch
    {
        "s" => "Single",
        "p" => "Parlay",
        _ => string.Empty
    };
}

public class DetailSettlementErrorLog
{
    public long Id { get; set; }
    public int TicketCount { get; set; }

    //public string TicketType { get; set; }
    //public string SettlementAction { get; set; }
    public int StatusId { get; set; }

    public string TransIds { get; set; }
    //public string FunctionName { get; set; }
    //public string TableName { get; set; }

    //public string TicketTypeExplicit =>
    //    TicketType switch
    //    {
    //        "s" => "Single",
    //        "p" => "Parlay",
    //        _ => string.Empty
    //    };
}