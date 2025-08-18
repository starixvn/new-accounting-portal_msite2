namespace Maintenance.DeleteTempVariable.Requests;

public class DeleteParlayLockingRequest
{
    public string DbServer { get; set; }

    public string Session { get; set; }
}