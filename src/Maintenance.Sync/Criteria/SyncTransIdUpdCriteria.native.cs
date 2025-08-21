namespace Maintenance.Sync.Criteria;

public class SyncTransIdUpdCriteria : NonQueryCommand
{
    public string TableName { get; set; }
    public byte[] TransInfoJson { get; set; }

    [SpParam(Ignored = true)]
    public string ServerName { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDB_SyncTransId_Upd_{ServerName}";

    public override bool IsValid() => !string.IsNullOrEmpty(TableName) && TransInfoJson is not null;
}