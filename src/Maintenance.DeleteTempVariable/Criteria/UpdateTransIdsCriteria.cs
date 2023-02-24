using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;
public class UpdateTransIdsCriteria : INonQueryCommand
{
    public string TransIds { get; set; }
    public string TableName { get; set; }
    public string ServerName { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string GetSettingKey() => $"NAP_Set_SiteDB_SyncTransIdToCentralDB_{ServerName}";
    public bool IsValid() => !string.IsNullOrEmpty(TransIds) && !string.IsNullOrEmpty(TableName) && !string.IsNullOrEmpty(ServerName);
}
