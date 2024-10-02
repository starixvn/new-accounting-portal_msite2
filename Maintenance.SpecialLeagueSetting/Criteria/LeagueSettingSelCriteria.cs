using Fanex.Data.Repository;

namespace Maintenance.SpecialLeagueSetting.Criteria;

public class LeagueSettingSelCriteria : CriteriaBase
{
	public int? LeagueId { get; set; }
	public short? AutoType { get; set; }
	public override string GetSettingKey() => "NAP_Set_AutoPayout_LeagueSetting_HardCode_Sel";

	public override bool IsValid() => true;
}
