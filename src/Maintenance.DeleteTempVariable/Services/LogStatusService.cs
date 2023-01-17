using AutoMapper;
using Fanex.Data.Repository;
using Fanex.Settlement.ScaleOut.Services;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface LogStatusService
{
    Task<List<LogStatus>> GetLogStatus();
    Task<int> UpdateLogStatus(UpdateLogStatusRequest request);
    Task<bool> CheckLogIdsInCentral(CheckLogIdsRequest request);
}

public class LogStatusServiceImp : LogStatusService
{
    private readonly IDynamicRepository repository;
    private readonly AppSettingsKey appSettingsKey;
    private readonly IMapper mapper;
    private readonly ScaleOutSettleService scaleOutSettleService;

    public LogStatusServiceImp(
        IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey,
        IMapper mapper,
        ScaleOutSettleService scaleOutSettleService = null)
    {
        this.repository = repository;
        this.appSettingsKey = appSettingsKey.Value;
        this.mapper = mapper;
        this.scaleOutSettleService = scaleOutSettleService;
    }

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
            list.ForEach(i => i.DbServer = server);

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