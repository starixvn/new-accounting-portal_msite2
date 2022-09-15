namespace Maintenance.DeleteTempVariable.Requests;

public class DeleteSettlementProcessingRequest
{
    public Int64 SessionId { get; set; }

    public string DbServer { get; set; }

    public bool IsSettleLock { get; set; }
}