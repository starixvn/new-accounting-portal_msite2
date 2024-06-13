namespace Maintenance.Sync.Criteria;

public class SyncTransIdToCentralDbUpdCriteria : NonQueryCommand
{
    public string TransIds { get; set; }
    public string TableName { get; set; }

    [SpParam(Ignored = true)]
    public string ServerName { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDB_SyncTransIdToCentralDB_{ServerName}";

    public override bool IsValid() => !string.IsNullOrEmpty(TransIds) && !string.IsNullOrEmpty(TableName) && !string.IsNullOrEmpty(ServerName);
}