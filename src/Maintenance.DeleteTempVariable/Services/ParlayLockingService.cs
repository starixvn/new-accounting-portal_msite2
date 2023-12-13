using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface ParlayLockingService
{
    Task<List<MatchParlayLocking>> GetMatches(ParlayLockingRequest request);

    Task<int> DeleteMatch(DeleteParlayLockingRequest request);
}

public class ParlayLockingServiceImp(IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey,
        IMapper mapper)
    : ParlayLockingService
{
    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    public async Task<List<MatchParlayLocking>> GetMatches(ParlayLockingRequest request)
    {
        var matchParlayLocking = new List<MatchParlayLocking>();

        foreach (var server in appSettingsKey.SiteDbServer)
        {
            var criteria = mapper.Map<GetParlayLockingCriteria>(request);
            criteria.DbServer = server;
            var list = (await repository.FetchAsync<MatchParlayLocking>(criteria)).ToList();
            list.ForEach(i => i.DbServer = server);

            matchParlayLocking.AddRange(list);
        }

        return matchParlayLocking;
    }

    public async Task<int> DeleteMatch(DeleteParlayLockingRequest request)
    {
        var criteria = mapper.Map<DeleteParlayLockingCriteria>(request);
        return await repository.ExecuteAsync(criteria);
    }
}