namespace Maintenance.Sync.Criteria;

public class SyncTransIdInRangeUpdCriteria : INonQueryCommand
{
    public string TableName { get; set; }
    public byte[] TransInfoJson { get; set; }

    [SpParam(Ignored = true)]
    public string ServerName { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public string GetSettingKey() => $"NAP_Set_SiteDB_SyncTransIdInRange_Upd_{ServerName}";

    public bool IsValid() => !string.IsNullOrEmpty(TableName) && TransInfoJson is not null;
}