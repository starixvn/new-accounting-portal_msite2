using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface SettlementLockService
{
    Task<List<MatchSettlementLock>> GetMatches(MatchRequest request);

    Task<int> Delete(DeleteSettlementProcessingRequest request);

    Task<int> UpdateSettlementLogs(UpdateSettlementLogRequest request);
}

public class SettlementLockServiceImp : SettlementLockService
{
    private readonly IDynamicRepository repository;

    private readonly AppSettingsKey appSettingsKey;

    private readonly IMapper mapper;

    public SettlementLockServiceImp(
        IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey,
        IMapper mapper)
    {
        this.repository = repository;
        this.appSettingsKey = appSettingsKey.Value;
        this.mapper = mapper;
    }

    public async Task<List<MatchSettlementLock>> GetMatches(MatchRequest request)
    {
        var listSettlementLock = new List<MatchSettlementLock>();

        foreach (var server in appSettingsKey.SiteDbServer)
        {
            var criteria = new GetSettlementLockCriteria
            {
                MatchId = request.MatchId,
                DbServer = server
            };

            var list = (await repository.FetchAsync<MatchSettlementLock>(criteria)).ToList();
            list.ForEach(i => i.DbServer = server);

            listSettlementLock.AddRange(list);

            if (server.Equals(DataBaseServers.DBTrans))
            {
                var replicatedData = await GetDataReplicatedServer(request.MatchId);
                listSettlementLock.AddRange(replicatedData);
            }
        }

        return listSettlementLock;
    }

    private async Task<List<MatchSettlementLock>> GetDataReplicatedServer(int matchId)
    {
        var replicatedData = new List<MatchSettlementLock>();
        foreach (var server in appSettingsKey.SubDbServer)
        {
            var replicatedServerCriteria = new GetSettlementLockReplicatedServerCriteria
            {
                MatchId = matchId,
                DbServer = server
            };

            var data = (await repository.FetchAsync<MatchSettlementLock>(replicatedServerCriteria)).ToList();
            data.ForEach(m => m.DbServer = server);

            replicatedData.AddRange(data);
        }

        return replicatedData;
    }

    public async Task<int> Delete(DeleteSettlementProcessingRequest request)
    {
        var criteria = mapper.Map<DeleteSettlementLockCriteria>(request);
        return await repository.ExecuteAsync(criteria);
    }

    public async Task<int> UpdateSettlementLogs(UpdateSettlementLogRequest request)
    {
        var criteria = mapper.Map<UpdateSettlementLogCriteria>(request);
        return await repository.ExecuteAsync(criteria);
    }
}