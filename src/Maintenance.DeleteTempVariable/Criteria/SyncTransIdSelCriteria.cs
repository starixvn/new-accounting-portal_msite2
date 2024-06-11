using Fanex.Data;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class SyncTransIdSelCriteria : NonQueryCommand
{
    public string TransIds { get; set; }
    public string TableName { get; set; }
    public byte[] TransInfoJson { get; set; }
    public string ErrMsg { get; set; }
    public int ErrCode { get; set; }

    [SpParam(Ignored = true)]
    public string ServerName { get; set; }

    public override string GetSettingKey() => $"NAP_Set_SiteDB_SyncTransId_Sel_{ServerName}";

    public override bool IsValid() => !string.IsNullOrEmpty(TransIds)
        && !string.IsNullOrEmpty(TableName);
}