using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

using Core.Constants;
using Core.Models;

public interface LogStatusService
{
    Task<List<LogStatus>> GetLogStatus();

    Task<int> UpdateLogStatus(UpdateLogStatusRequest request);

    Task<bool> CheckLogIdsInCentral(CheckLogIdsRequest request);
}

public class LogStatusServiceImp(IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey,
        IMapper mapper,
        ScaleOutSettleService scaleOutSettleService)
    : LogStatusService
{
    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    public async Task<bool> CheckLogIdsInCentral(CheckLogIdsRequest request)
    {
        var siteGroupId = await scaleOutSettleService.GetSiteGroupId(request.DbServer);

        var criteria = new CheckLogIdsCriteria
        {
            LogIds = request.LogIds,
            SiteDBGroupId = siteGroupId,
            IsPass = true
        };

        await repository.ExecuteAsync(criteria);
        return criteria.IsPass;
    }

    public async Task<List<LogStatus>> GetLogStatus()
    {
        var logStatusList = new List<LogStatus>();
        var siteDbServer = appSettingsKey.SiteDbServer.Where(n => n != DataBaseServers.DBCentral);
        foreach (var server in siteDbServer)
        {
            var criteria = new GetLogStatusCriteria
            {
                DbServer = server
            };
            var list = (await repository.FetchAsync<LogStatus>(criteria)).ToList();
            logStatusList.AddRange(list);
        }

        return logStatusList;
    }

    public async Task<int> UpdateLogStatus(UpdateLogStatusRequest request)
    {
        var criteria = mapper.Map<UpdateLogStatusCriteria>(request);
        return await repository.ExecuteAsync(criteria);
    }
}