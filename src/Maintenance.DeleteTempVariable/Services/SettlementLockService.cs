using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services;

public interface SettlementLockService
{
    Task<List<MatchSettlementLock>> GetMatches(DeleteTempRequest request);

    Task<int> Delete(DeleteSettlementProcessingRequest request);

    Task<int> UpdateSettlementLogs(UpdateSettlementLogRequest request);
}

public class SettlementLockServiceImp : SettlementLockService
{
    private readonly IDynamicRepository repository;

    public SettlementLockServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<MatchSettlementLock>> GetMatches(DeleteTempRequest request)
    {
        var criteria = new GetSettlementLockCriteria
        {
            MatchId = request.MatchId,
        };

        var dbtransData = (await repository.FetchAsync<MatchSettlementLock>(criteria)).ToList();
        dbtransData.ForEach(m => m.DBServer = DataBaseServers.DBTrans);

        var replicatedData = await GetDataReplicatedServer(request.MatchId);

        if (replicatedData.Any())
        {
            dbtransData.AddRange(replicatedData);
        }

        return dbtransData;
    }

    private async Task<List<MatchSettlementLock>> GetDataReplicatedServer(int? matchId)
    {
        var replicatedData = new List<MatchSettlementLock>();
        var listReplicatedServer = new List<string> { "DB052", "DB053", "DB054", "DB055", "DB06", "DB061", "DBTP" };
        foreach (var server in listReplicatedServer)
        {
            var replicatedServerCriteria = new GetSettlementLockReplicatedServerCriteria
            {
                MatchId = matchId,
                DbServer = server
            };

            var data = (await repository.FetchAsync<MatchSettlementLock>(replicatedServerCriteria)).ToList();
            data.ForEach(m => m.DBServer = server);

            if (data.Any())
            {
                replicatedData.AddRange(data);
            }
        }

        return replicatedData;
    }

    public async Task<int> Delete(DeleteSettlementProcessingRequest request)
    {
        var criteria = new DeleteSettlementLockCriteria
        {
            SessionId = request.SessionId,
            DbServer = request.DbServer,
            IsSettleLock = request.isSettleLock
        };

        return await repository.ExecuteAsync(criteria);
    }

    public async Task<int> UpdateSettlementLogs(UpdateSettlementLogRequest request)
    {
        var row = 0;
        var criteria = new UpdateSettlementLogCriteria
        {
            SessionId = request.SessionId,
            DbServer = request.DbServer,
            StartTime = request.StartTime
        };

        return await repository.ExecuteAsync(criteria);
    }
}