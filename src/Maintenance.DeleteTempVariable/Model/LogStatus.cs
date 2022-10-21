using Maintenance.DeleteTempVariable.Constants;

namespace Maintenance.DeleteTempVariable.Model;

public class LogStatus
{
    public string ServerName { get; set; }

    public string DbServer { get; set; }

    public int LogId { get; set; }

    public int StatusId { get; set; }

    public string StatusName
    {
        get
        {
            return StatusId switch
            {
                StatusKey.Running => "Running",
                StatusKey.Completed => "Completed",
                StatusKey.Failed => "Failed",
                _ => "Waiting"
            };
        }
    }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime StartTimeValidation { get; set; }

    public string SystemName { get; set; }

    public string FunctionName { get; set; }

    public string TableName { get; set; }

    public string Action { get; set; }

    public int MatchId { get; set; }

    public string TransInfo { get; set; }

    public DateTime LogDate { get; set; }

    public string SettlementAction { get; set; }

    public string TicketType { get; set; }
}