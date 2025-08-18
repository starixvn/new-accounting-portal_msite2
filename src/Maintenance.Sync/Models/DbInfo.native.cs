namespace Maintenance.Sync.Models;

public class DbInfo
{
    public List<string> Tables { get; set; }
    public List<string> DbSites { get; set; }
    public List<string> SubDbSites { get; set; }
}