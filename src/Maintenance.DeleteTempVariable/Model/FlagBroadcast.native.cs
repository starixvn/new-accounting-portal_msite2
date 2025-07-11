namespace Maintenance.DeleteTempVariable.Model;

public class FlagBroadcast
{
    public bool CurrentStatus { get; set; }

    public string ProductType { get; set; }

    public string QueueNumber { get; set; }

    public string QueueWaiting { get; set; }

    public bool IsUnderMaintenance { get; set; }

    public bool SettleUnderMaintenanceStatus { get; set; }
}