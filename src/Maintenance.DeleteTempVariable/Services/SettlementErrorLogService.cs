using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;
using Maintenance.DeleteTempVariable.Model;
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

public class SettlementErrorLogServiceImp(IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey,
        IMapper mapper)
    : SettlementErrorLogService
{
    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    private readonly Dictionary<short, string> SiteDBs = new()
    {
        { 1, "DBSID01" },
        { 3,"DBSID02" }
    };

    public async Task<List<SettlementErrorLog>> GetSettlementErrorsLogs(SettlementErrorLogRequest request)
    {
        var list = await GetSettlementProcessingLogs(request);
        var list1 = await GetSettlementLockLogs(request);
        var list2 = await GetSettlementSSBLogs(request);

        var result = list.Concat(list1).Concat(list2).ToList();

        return result;
    }

    private async Task<List<SettlementErrorLog>> GetSettlementProcessingLogs(SettlementErrorLogRequest request)
    {
        var dbServers = new List<string> { DataBaseServers.DBCentral };
        dbServers.AddRange(appSettingsKey.SubDbServer);

        var result = new List<SettlementErrorLog>();

        foreach (var server in dbServers)
        {
            var subCriteria = new GetSettlementProcessingErrorLogCriteria
            {
                DbServer = server,
                MatchId = request.MatchId,
                Mode = request.Mode
            };
            var data = await repository.FetchAsync<SettlementErrorLog>(subCriteria) as List<SettlementErrorLog>;

            data.ForEach(d => { d.DbServer = server; });

            result.AddRange(data);
        }

        result.ForEach(x =>
        {
            x.TempTableName = "SiteDB_SettlementProcessing";
        });

        return result;
    }

    private async Task<List<SettlementErrorLog>> GetSettlementLockLogs(SettlementErrorLogRequest request)
    {
        var dbServers = new List<string> { DataBaseServers.DBCentral };
        dbServers.AddRange(appSettingsKey.SubDbServer);

        var result = new List<SettlementErrorLog>();

        foreach (var server in dbServers)
        {
            var subCriteria = new GetSettlementLockErrorLogCriteria
            {
                MatchId = request.MatchId,
                DbServer = server,
                Mode = request.Mode
            };
            var data = await repository.FetchAsync<SettlementErrorLog>(subCriteria) as List<SettlementErrorLog>;
            data.ForEach(d => { d.DbServer = server; });

            result.AddRange(data);
        }

        return result.Select(v => { v.TempTableName = "SiteDB_SettlementLock"; return v; }).ToList();
    }

    private async Task<List<SettlementErrorLog>> GetSettlementSSBLogs(SettlementErrorLogRequest request)
    {
        var dbServers = new List<string> { DataBaseServers.DBCentral };
        dbServers.AddRange(appSettingsKey.SubDbServer);

        var result = new List<SettlementErrorLog>();

        foreach (var server in dbServers)
        {
            var subCriteria = new GetSiteDbSSBErrorLogCriteria
            {
                DbServer = server,
                Mode = request.Mode
            };
            var data = await repository.FetchAsync<SettlementErrorLog>(subCriteria) as List<SettlementErrorLog>;

            data.ForEach(d => { d.DbServer = server; });

            result.AddRange(data);
        }

        return result.Select(v => { v.TempTableName = "Settlement_SiteDB_SSBErr"; return v; }).ToList();
    }

    public async Task<List<DetailSettlementErrorLog>> GetSettlementErrorsLogDetails(SettlementErrorLogDetailRequest request)
    {
        var criteria = mapper.Map<GetSettlementErrorLogDetailsCriteria>(request);

        var logs = await repository.FetchAsync<DetailSettlementErrorLog>(criteria);

        return logs.ToList();
    }

    //private static void AssignLogInfo(DetailSettlementErrorLog log, LogStatus logInfo)
    //{
    //    if (logInfo == null) return;
    //    log.TableName = logInfo.TableName;
    //    log.FunctionName = logInfo.FunctionName;
    //    log.TicketType = logInfo.TicketType;
    //    log.SettlementAction = logInfo.SettlementAction;
    //}

    //private async Task<LogStatus> GetLogStatus(long sessionId, short siteDBGroupId)
    //{
    //    var server = SiteDBs.TryGetValue(siteDBGroupId, out var value) ? value : string.Empty;
    //    var criteria = new GetLogStatusCriteria
    //    {
    //        DbServer = server,
    //        SessionId = sessionId
    //    };
    //    var result = await repository.GetAsync<LogStatus>(criteria);

    //    return result;
    //}

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