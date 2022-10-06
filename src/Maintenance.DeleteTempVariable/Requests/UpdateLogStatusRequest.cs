namespace Maintenance.DeleteTempVariable.Requests;

public class UpdateLogStatusRequest
{
    public string DbServer { get; set; }

    public int LogId { get; set; }

    public string Action { get; set; }
}