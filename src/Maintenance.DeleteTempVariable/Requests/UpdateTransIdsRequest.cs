namespace Maintenance.DeleteTempVariable.Requests;

public class UpdateTransIdsRequest
{
    public string TransIds { get; set; }
    public string TableName { get; set; }
    public string ServerName { get; set; }

    public string[] ToServerNames { get; set; }
}