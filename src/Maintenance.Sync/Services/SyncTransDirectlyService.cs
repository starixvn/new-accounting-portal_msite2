using Maintenance.Core.Constants;
using Maintenance.Core.Models;
using Maintenance.Sync.Criteria;
using Maintenance.Sync.Models;
using Maintenance.Sync.Requests;

namespace Maintenance.Sync.Services;

public interface SyncTransDirectlyService
{
    Task<Result> UpdateTransId(UpdateTransIdsRequest request);

    DbInfo GetDbInfo();
}

public class SyncTransDirectlyServiceImp(IOptions<AppSettingsKey> appSettings, IDynamicRepository repository)
    : SyncTransDirectlyService
{
    private readonly AppSettingsKey appSettings = appSettings.Value;

    public DbInfo GetDbInfo() => new()
    {
        Tables = new List<string> { "bettrans ", "bettransm" },
        DbSites = appSettings.SiteDbServer.Where(s => !s.Equals(DataBaseServers.DBCentral)).ToList(),
        SubDbSites = [DataBaseServers.DBCentral, .. appSettings.SubDbServer],
    };

    public async Task<Result> UpdateTransId(UpdateTransIdsRequest request)
    {
        var (selResult, transInfo) = await GetTransIds(request);

        if (!selResult.IsSuccess) return selResult;

        var result = await UpdSyncTransIdAllSite(request, transInfo);

        return result;
    }

    private async Task<(Result, byte[])> GetTransIds(UpdateTransIdsRequest request)
    {
        var criteria = new SyncTransIdSelCriteria
        {
            TransIds = request.TransIds,
            TableName = request.TableName,
            ServerName = request.ServerName,
        };

        await repository.ExecuteAsync(criteria);

        var selResult = new Result
        {
            ErrCode = criteria.ErrCode,
            ErrMsg = criteria.ErrMsg
        };

        return (selResult, criteria.TransInfoJson);
    }

    private async Task<Result> UpdSyncTransIdAllSite(UpdateTransIdsRequest request, byte[] transInfo)
    {
        var criteria = new SyncTransIdUpdCriteria
        {
            TableName = request.TableName,
            TransInfoJson = transInfo,
        };

        var tasks = request.ToServerNames.Select(s => UpdSyncTransIdOnSite(s, criteria));
        var results = await Task.WhenAll(tasks);

        var errorResults = results.Where(x => !x.IsSuccess);

        if (errorResults.Any())
        {
            return new Result
            {
                ErrCode = errorResults.First().ErrCode,
                ErrMsg = string.Join('|', errorResults.Select(x => x.ErrMsg)),
            };
        }

        return new Result();
    }

    private async Task<Result> UpdSyncTransIdOnSite(string db, SyncTransIdUpdCriteria criteria)
    {
        criteria.ServerName = db;

        await repository.ExecuteAsync(criteria);

        return new Result
        {
            ErrCode = criteria.ErrCode,
            ErrMsg = criteria.ErrMsg
        };
    }
}