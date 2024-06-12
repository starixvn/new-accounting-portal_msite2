using Maintenance.DeleteTempVariable.Constants;

namespace Maintenance.DeleteTempVariable.Model;

public class Match
{
    public string ServerName { get; set; }

    public string DbServer { get; set; }

    public int MatchId { get; set; }

    public string HomeTeam { get; set; }

    public string AwayTeam { get; set; }

    public string LeagueName { get; set; }

    public string EventName => KeyId is ProductNameKey.Outright or ProductNameKey.Racing ? LeagueName : $"{HomeTeam} - vs - {AwayTeam}";

    public DateTime CreatedDateTime { get; set; }

    public string SportName { get; set; }

    public string Action { get; set; }

    public string Message { get; set; }

    public string ProductName =>
        KeyId switch
        {
            ProductNameKey.Sportbook => $"{KeyId}-Sportbook, Numbergame",
            ProductNameKey.Outright => $"{KeyId}-Outright",
            ProductNameKey.MetricGaming => $"{KeyId}-Metric Gaming",
            ProductNameKey.Racing => $"{KeyId}-VS Racing",
            ProductNameKey.BetBuilder => $"{KeyId}-BetBuilder",
            ProductNameKey.FastMarket => $"{KeyId}-Fast Market",
            ProductNameKey.FastMarketSABA => $"{KeyId}-Fast Market SABA Time Machine",
            _ => string.Empty
        };

    public int Value { get; set; }

    public int TicketCount { get; set; }

    public bool IsDelete { get; set; }

    public bool WrongBalance { get; set; }

    public int IsCalculating { get; set; }

    public string Status =>
           IsCalculating switch
           {
               StatusKey.Running => "Running",

               StatusKey.Failed => "Failed",

               _ => string.Empty
           };

    public int KeyId { get; set; }

    public string MarketId { get; set; }

    public string Remark { get; set; }
}