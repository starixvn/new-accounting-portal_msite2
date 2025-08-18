using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Criteria;

public class GetAllSportCriteria : CriteriaBase
{
    public int ProductId { get; set; } = 1; // Sportbook
    public override string GetSettingKey() => "Nap_Set_Sporttype_Sel";

    public override bool IsValid() => true;
}
