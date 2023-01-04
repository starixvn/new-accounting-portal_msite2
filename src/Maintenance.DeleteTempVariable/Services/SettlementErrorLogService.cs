using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Requests.SyncErrorLog;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface SettlementErrorLogService
{
    Task<int> DeleteSettlementLockErrorsLog(DeleteSettlementLockErrorLogRequest request);
    Task<int> DeleteSettlementProcessingErrorsLog(DeleteSettlementProcessingErrorLogRequest request);
    Task<List<DetailSettlementErrorLog>> GetSettlementErrorsLogDetails(SettlementErrorLogDetailRequest request);
    Task<List<SettlementErrorLog>> GetSettlementErrorsLogs(SettlementErrorLogRequest request);
    Task<int> UpdateSSBSettlementErrorLog(UpdateSiteDbSSBErrorLogRequest request);
}

public class SettlementErrorLogServiceImp : SettlementErrorLogService
{
    private readonly AppSettingsKey appSettingsKey;
    private readonly IDynamicRepository repository;
    private readonly IMapper mapper;

    public SettlementErrorLogServiceImp(
        IDynamicRepository repository,
           IOptions<AppSettingsKey> appSettingsKey,
           IMapper mapper)
    {
        this.repository = repository;
        this.appSettingsKey = appSettingsKey.Value;
        this.mapper = mapper;
    }

    public async Task<List<SettlementErrorLog>> GetSettlementErrorsLogs(SettlementErrorLogRequest request)
    {
        var list = await GetSettlementProcessingLogs(request);
        var list1 = await GetSettlementLockLogs(request);
        var list2 = await GetSettlementSSBLogs();

        return list.Concat(list1).Concat(list2).ToList();
    }

    private async Task<List<SettlementErrorLog>> GetSettlementProcessingLogs(SettlementErrorLogRequest request)
    {
        // central db
        var list = new List<SettlementErrorLog>();

        var processingCriteria = new GetSettlementProcessingErrorLogCriteria
        {
            MatchId = request.MatchId,
        };
        var result = await repository.FetchAsync<SettlementErrorLog>(processingCriteria) as List<SettlementErrorLog>;

        list.AddRange(result);

        list.ForEach(data => { data.DbServer = processingCriteria.DbServer; });

        // sub db
        var subData = new List<SettlementErrorLog>();
        foreach (var server in appSettingsKey.SubDbServer)
        {
            var c = new GetSettlementProcessingErrorLogCriteria
            {
                DbServer = server,
                MatchId = request.MatchId,
            };
            var data = await repository.FetchAsync<SettlementErrorLog>(c) as List<SettlementErrorLog>;
            data.ForEach(data => { data.DbServer = server; });

            subData.AddRange(data);
        }

        list.AddRange(subData);

        return list.Select(v => { v.TableName = "SiteDB_SettlementProcessing"; return v; }).ToList();
    }


    private async Task<List<SettlementErrorLog>> GetSettlementLockLogs(SettlementErrorLogRequest request)
    {
        // central db
        var list = new List<SettlementErrorLog>();
        var criteria = new GetSettlementLockErrorLogCriteria
        {
            MatchId = request.MatchId,
        };
        var result = (await repository.FetchAsync<SettlementErrorLog>(criteria)).ToList();

        list.AddRange(result);

        list.ForEach(data => { data.DbServer = criteria.DbServer; });

        // sub db
        var subData = new List<SettlementErrorLog>();

        foreach (var server in appSettingsKey.SubDbServer)
        {
            var c = new GetSettlementLockErrorLogCriteria
            {
                MatchId = request.MatchId,
                DbServer = server,
            };
            var data = await repository.FetchAsync<SettlementErrorLog>(c) as List<SettlementErrorLog>;
            data.ForEach(data => { data.DbServer = server; });

            subData.AddRange(data);
        }

        list.AddRange(subData);

        return list.Select(v => { v.TableName = "SiteDB_SettlementLock"; return v; }).ToList();
    }


    private async Task<List<SettlementErrorLog>> GetSettlementSSBLogs()
    {
        var list = new List<SettlementErrorLog>();

        var criteria = new GetSiteDbSSBErrorLogCriteria();

        var result = (await repository.FetchAsync<SettlementErrorLog>(criteria)).ToList();

        list.AddRange(result);

        list.ForEach(data => { data.DbServer = criteria.DbServer; });

        var subData = new List<SettlementErrorLog>();

        foreach (var server in appSettingsKey.SubDbServer)
        {
            var c = new GetSiteDbSSBErrorLogCriteria
            {
                DbServer = server,
            };
            var data = await repository.FetchAsync<SettlementErrorLog>(c) as List<SettlementErrorLog>;

            data.ForEach(data => { data.DbServer = server; });

            subData.AddRange(data);
        }

        list.AddRange(subData);

        return list.Select(v => { v.TableName = "Settlement_SiteDB_SSBErr"; return v; }).ToList();
    }


    public async Task<List<DetailSettlementErrorLog>> GetSettlementErrorsLogDetails(SettlementErrorLogDetailRequest request)
    {
        var criteria = mapper.Map<GetSettlementErrorLogDetailsCriteria>(request);

        var result = await repository.FetchAsync<DetailSettlementErrorLog>(criteria);

        return result.ToList();
    }

    public async Task<int> DeleteSettlementProcessingErrorsLog(DeleteSettlementProcessingErrorLogRequest request)
    {
        var criteria = mapper.Map<DeleteSettlementProcessingErrorLogCriteria>(request);

        return await repository.ExecuteAsync(criteria);
    }

    public async Task<int> DeleteSettlementLockErrorsLog(DeleteSettlementLockErrorLogRequest request)
    {
        var criteria = mapper.Map<DeleteSettlementLockErrorLogCriteria>(request);

        return await repository.ExecuteAsync(criteria);
    }

    public async Task<int> UpdateSSBSettlementErrorLog(UpdateSiteDbSSBErrorLogRequest request)
    {
        var criteria = mapper.Map<UpdateSiteDbSSBErrorLogCriteria>(request);

        return await repository.ExecuteAsync(criteria);
    }
}
