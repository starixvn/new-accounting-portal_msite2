namespace Maintenance.DeleteTempVariable.Requests;

public class ProviderMappingRequest
{
    public int SportType { get; set; }
    public int SourceId { get; set; }

    public bool IsActive { get; set; }
}
