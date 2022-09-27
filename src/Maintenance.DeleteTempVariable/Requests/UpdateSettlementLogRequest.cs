namespace Maintenance.DeleteTempVariable.Requests;

public class UpdateSettlementLogRequest
{
    public Int64 SessionId { get; set; }

    public DateTime CreatedTime { get; set; }

    public string DbServer { get; set; }

    public string MessageId { get; set; }
}