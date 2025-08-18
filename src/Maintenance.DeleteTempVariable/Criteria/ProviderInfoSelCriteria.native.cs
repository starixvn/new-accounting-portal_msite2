using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class ProviderInfoSelCriteria : CriteriaBase
{
    public int SourceType { get; set; } = 0;
    public int SportType { get; set; }
    public override string GetSettingKey() => "NAP_Set_ProviderInfoSel";

    public override bool IsValid() => true;
}
