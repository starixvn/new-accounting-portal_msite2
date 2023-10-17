namespace Maintenance.DeleteTempVariable.Model;

public class AppSettingsKey
{
    public const string MultiDbServer = "MultiDbServer";

    public List<string> SiteDbServer { get; set; }

    public List<string> SubDbServer { get; set; }

    public RabbitMq RabbitMq { get; set; }
}