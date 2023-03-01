using Maintenance.DeleteTempVariable.Constants;

namespace Maintenance.DeleteTempVariable.Model;

public class LastLog
{
    public string ServerName { get; set; }

    public string DbServer { get; set; }

    public int LastLogId { get; set; }

    public int LastStatusId { get; set; }

    public string StatusName => LastStatusId switch
    {

        StatusKey.Running => "Running",
        StatusKey.Completed => "Completed",
        StatusKey.Failed => "Failed",
        _ => "Waiting"
    };

    public bool CurrentStatus { get; set; }

    public int WaitingCount { get; set; }
    public QueueType QueueType { get; set; }
}