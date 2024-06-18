namespace Maintenance.Sync.Criteria;

public class SyncMatchIdSelCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    public string TableName { get; set; }

    [SpParam(Ignored = true)]
    public string DbServer { get; set; }

    public override string GetSettingKey() =>
        $"NAP_Set_SiteDB_SyncMatchId_Sel_{DbServer}";

    public override bool IsValid() => MatchId > 0 && !string.IsNullOrEmpty(TableName);
}