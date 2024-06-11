using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface SyncTransIdDirectlyService
{
    DbInfo GetDbInfo();

    Task<Result> UpdateTransId(UpdateTransIdsRequest request);
}

public class SyncTransIdDirectlyServiceImp(IOptions<AppSettingsKey> appSettings, IDynamicRepository repository,
        IMapper mapper)
    : SyncTransIdDirectlyService
{
    private readonly AppSettingsKey appSettings = appSettings.Value;

    public DbInfo GetDbInfo()
    {
        return new DbInfo
        {
            Tables = new List<string> { "bettrans ", "bettransm" },
            DbSites = appSettings.SiteDbServer.Where(s => !s.Equals(DataBaseServers.DBCentral)).ToList(),
            SubDbSites = [DataBaseServers.DBCentral, .. appSettings.SubDbServer],
        };
    }

    public async Task<Result> UpdateTransId(UpdateTransIdsRequest request)
    {
        var selCriteria = new SyncTransIdSelCriteria
        {
            TransIds = request.TransIds,
            TableName = request.TableName,
            ServerName = request.ServerName,
        };

        await repository.ExecuteAsync(selCriteria);

        var selResult = new Result
        {
            ErrCode = selCriteria.ErrCode,
            ErrMsg = selCriteria.ErrMsg
        };

        if (selResult.ErrCode > 0)
        {
            return selResult;
        }

        var criteria = new SyncTransIdUpdCriteria
        {
            TableName = request.TableName,
            TransInfoJson = selCriteria.TransInfoJson,
        };

        var tasks = request.ToServerNames.Select(s => UpdSyncTransIdOnSite(s, criteria));
        var results = await Task.WhenAll(tasks);

        if (results.Any(x => x.ErrCode > 0))
        {
            var errorResult = results.FirstOrDefault(x => x.ErrCode > 0);
            return errorResult;
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