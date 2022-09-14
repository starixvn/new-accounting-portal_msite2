namespace Maintenance.DeleteTempVariable.Requests;

public class UpdateSettlementLogRequest
{
    public Int64 SessionId { get; set; }

    public DateTime StartTime { get; set; }

    public string DbServer { get; set; }
}