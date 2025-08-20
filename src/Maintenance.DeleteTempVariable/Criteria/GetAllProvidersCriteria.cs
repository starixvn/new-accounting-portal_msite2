using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class GetAllProvidersCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Nap_mSite2_ProviderInfoSel";

    public override bool IsValid() => true;
}
