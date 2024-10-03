using Fanex.Data.Repository;
using Fanex.RemotingRef;
using Maintenance.SpecialLeagueSetting.Criteria;
using Maintenance.SpecialLeagueSetting.Models;
using Maintenance.SpecialLeagueSetting.Request;

namespace Maintenance.SpecialLeagueSetting.Services;

public interface LeagueSettingService
{
	Task<List<League>> GetLeagueSetting(GetLeagueSetting request);
	Task<bool> UpdateLeagueSetting(UpdateSettingRequest request);

	Task<bool> RemoveLeagueSetting(RemoveSettingRequest request);

}

public class LeagueSettingServiceImp : LeagueSettingService
{
	private readonly IDynamicRepository repository;
	private readonly IRemotingRefService remotingRefService;

	public LeagueSettingServiceImp(IDynamicRepository repository, IRemotingRefService remotingRefService)
	{
		this.repository = repository;
		this.remotingRefService = remotingRefService;
	}

	public async Task<List<League>> GetLeagueSetting(GetLeagueSetting request)
	{
		var criteria = new LeagueSettingSelCriteria
		{
			AutoType = request.AutoType,
			LeagueId = request.LeagueId
		};

		var result = await repository.FetchAsync<League>(criteria);

		foreach (var item in result)
		{
			item.LeagueName = remotingRefService.GetLeagueName(item.LeagueId.ToString(), "en", string.Empty);
			item.SportName = remotingRefService.GetSportName(item.SportType.ToString(), "en", string.Empty);
		}

		return result.ToList();
	}

	public async Task<bool> RemoveLeagueSetting(RemoveSettingRequest request)
	{
		var criteria = new LeagueSettingUpdateCriteria
		{
			AutoType = request.AutoType,
			LeagueId = request.LeagueId,
			Action = "Delete"

		};

		await repository.ExecuteAsync(criteria);

		return true;
	}

	public async Task<bool> UpdateLeagueSetting(UpdateSettingRequest request)
	{
		var criteria = new LeagueSettingUpdateCriteria
		{
			AutoType = request.AutoType,
			LeagueId = request.LeagueId,
			Action = "Update"

		};

		await repository.ExecuteAsync(criteria);

		return true;
	}

}
