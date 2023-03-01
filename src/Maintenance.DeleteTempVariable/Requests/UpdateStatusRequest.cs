namespace Maintenance.DeleteTempVariable.Requests;

public class UpdateStatusRequest
{
    public bool Status { get; set; }
    public string CacheKey { get; set; }
}
