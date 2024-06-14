namespace Maintenance.DeleteTempVariable.Requests;

public class DeleteSettlementProcessingRequest
{
    public long SessionId { get; set; }

    public string DbServer { get; set; }

    public bool IsSettleLock { get; set; }
    public string Mode { get; set; }
}