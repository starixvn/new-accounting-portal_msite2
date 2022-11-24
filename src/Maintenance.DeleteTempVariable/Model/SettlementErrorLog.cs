namespace Maintenance.DeleteTempVariable.Model;

public class SettlementErrorLog
{
    public string ServerName { get; set; }
    public string DbServer { get; set; }
    public int LogId { get; set; }
    public int MatchId { get; set; }
    public string TransInfo { get; set; }
    public string EventName { get; set; }
    public string SessionId { get; set; }
    public string MessageId { get; set; }
    public string ErrorMessage { get; set; }
    public string TableName { get; set; }
}
