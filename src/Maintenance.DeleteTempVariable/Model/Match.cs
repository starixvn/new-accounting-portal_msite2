namespace Maintenance.DeleteTempVariable.Model;

public class Match
{
    public string DBServer { get; set; }

    public int MatchId { get; set; }

    public string HomeTeam { get; set; }

    public string AwayTeam { get; set; }

    public string LeagueName { get; set; }

    public string EventName => KeyId is 2 or 4 ? LeagueName : $"{HomeTeam} - vs - {AwayTeam}";

    public DateTime CreatedDateTime { get; set; }

    public string SportName { get; set; }

    public string Action { get; set; }

    public string Message { get; set; }

    public string ProductName
    {
        get
        {
            switch (KeyId)
            {
                case 1:
                    return $"{KeyId}-Sportbook, Numbergame";

                case 2:
                    return $"{KeyId}-Outright";

                case 3:
                    return $"{KeyId}-Metric Gaming";

                case 4:
                    return $"{KeyId}-VS Racing";

                case 5:
                    return $"{KeyId}-BetBuilder";

                case 6:
                    return $"{KeyId}-Fast Market";

                case 7:
                    return $"{KeyId}-Fast Market SABA Time Machine";

                default:
                    return string.Empty;
            }
        }
    }

    public int Value { get; set; }

    public int TicketCount { get; set; }

    public bool IsDelete { get; set; }

    public bool WrongBalance { get; set; }

    public int IsCalculating { get; set; }

    public string Status
    {
        get
        {
            switch (IsCalculating)
            {
                case 1:
                    return "Running";

                case 3:
                    return "Failed";

                default:
                    return string.Empty;
            }
        }
    }

    public int KeyId { get; set; }

    public string MarketId { get; set; }

    public string Remark { get; set; }
}