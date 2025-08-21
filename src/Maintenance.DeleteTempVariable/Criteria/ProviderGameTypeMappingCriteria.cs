using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class ProviderGameTypeMappingCriteria : INonQueryCommand
{
    public int SourceId { get; set; }
    public int SportType { get; set; }
    public bool IsActive { get; set; } = true;
    public string GameTypes { get; set; } = "0";

    public string ErrMsg { get; set; }
    public int ErrCode { get; set; }

    public string GetSettingKey() => "NAP_mSite2_ProviderGameTypeMapping";

    public bool IsValid() => true;
}
