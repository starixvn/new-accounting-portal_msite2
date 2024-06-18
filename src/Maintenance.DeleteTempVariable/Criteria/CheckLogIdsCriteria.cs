using Fanex.Data;
using Fanex.Data.Repository;
using Maintenance.Core.Constants;

namespace Maintenance.DeleteTempVariable.Criteria;

public class CheckLogIdsCriteria : INonQueryCommand
{
    [SpParam(Ignored = true)]
    public string DbServer { get; set; } = DataBaseServers.DBCentral;

    public string GetSettingKey() => $"NAP_Set_SiteDBServiceLog_Checking_{DbServer}";

    public string LogIds { get; set; }
    public int SiteDBGroupId { get; set; }
    public bool IsPass { get; set; }

    public bool IsValid() => !string.IsNullOrWhiteSpace(DbServer);
}