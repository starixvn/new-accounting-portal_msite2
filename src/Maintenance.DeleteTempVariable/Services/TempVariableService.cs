using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

using Core.Models;

public interface TempVariableService
{
    Task<int> TicketCount(MatchTicketRequest request);

    Task<Match> GetMatchDetail(MatchTicketRequest request);

    Task<List<Match>> GetMatches(int matchId);

    Task<int> DeleteMatch(DeleteTempRequest request);
}

public class TempVariableServiceImp(IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey,
        IMapper mapper)
    : TempVariableService
{
    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    public async Task<int> TicketCount(MatchTicketRequest request)
    {
        var criteria = mapper.Map<GetTicketCriteria>(request);
        var model = await repository.GetAsync<MatchTicket>(criteria);
        return model.TicketCount;
    }

    public async Task<List<Match>> GetMatches(int matchId)
    {
        var matches = new List<Match>();

        foreach (var server in appSettingsKey.SiteDbServer)
        {
            var criteria = new GetMatchesCriteria
            {
                DbServer = server,
                MatchId = matchId
            };

            var list = (await repository.FetchAsync<Match>(criteria)).ToList();
            list.ForEach(i => i.DbServer = server);

            matches.AddRange(list);
        }

        return matches;
    }

    public async Task<Match> GetMatchDetail(MatchTicketRequest request)
    {
        var criteria = mapper.Map<GetMatchesCriteria>(request);
        return await repository.GetAsync<Match>(criteria);
    }

    public async Task<int> DeleteMatch(DeleteTempRequest request)
    {
        var criteria = mapper.Map<DeleteMatchCriteria>(request);
        return await repository.ExecuteAsync(criteria);
    }
}