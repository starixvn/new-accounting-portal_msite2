namespace Maintenance.Sync.Criteria;

public class SyncTransIdInRangeSelCriteria : INonQueryCommand
{
    public int MatchId { get; set; }
    public long FromTransId { get; set; }
    public long ToTransId { get; set; }
    public byte[] TransInfoJson { get; set; }
    public string TableName { get; set; }

    [SpParam(Ignored = true)]
    public string ServerName { get; set; }

    public string GetSettingKey() => $"NAP_Set_SiteDB_SyncTransIdInRange_Sel_{ServerName}";

    public bool IsValid() => true;
}