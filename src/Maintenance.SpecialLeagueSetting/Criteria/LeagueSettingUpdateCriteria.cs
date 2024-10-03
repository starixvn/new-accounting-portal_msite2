using Fanex.Data.Repository;

namespace Maintenance.SpecialLeagueSetting.Criteria;

public class LeagueSettingUpdateCriteria : INonQueryCommand
{
	public int LeagueId { get; set; }
	public byte AutoType { get; set; }

	public string Action { get; set; }

	public string GetSettingKey() => "NAP_Set_AutoPayout_LeagueSetting_Update";

	public bool IsValid() => LeagueId != 0 && AutoType != 0;
}
