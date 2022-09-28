namespace Maintenance.DeleteTempVariable.Requests;

public class UpdateSettlementLogRequest
{
    public long SessionId { get; set; }

    public DateTime CreatedTime { get; set; }

    public string DbServer { get; set; }

    public string MessageId { get; set; }
}