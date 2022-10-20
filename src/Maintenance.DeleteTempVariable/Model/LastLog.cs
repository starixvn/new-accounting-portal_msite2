namespace Maintenance.DeleteTempVariable.Model;

public class LastLog
{
    public string ServerName { get; set; }
    public string DBServer { get; set; }
    public int LastLogId { get; set; }
    public int LastStatusId { get; set; }
    public bool CurrentStatus { get; set; }
    public int WaitingCount { get; set; }
}
