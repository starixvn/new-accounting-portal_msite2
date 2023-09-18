namespace Maintenance.DeleteTempVariable.Requests;

public class UpdateFlagBroadcastRequest
{
    public bool Status { get; set; }
    public List<int> ProductType { get; set; }
}
