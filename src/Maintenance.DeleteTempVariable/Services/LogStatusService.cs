using AutoMapper;
using Fanex.Data.Repository;
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
}

public class LogStatusServiceImp : LogStatusService
{
    private readonly IDynamicRepository repository;

    private readonly AppSettingsKey appSettingsKey;

    private readonly IMapper mapper;

    public LogStatusServiceImp(
        IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey,
        IMapper mapper)
    {
        this.repository = repository;
        this.appSettingsKey = appSettingsKey.Value;
        this.mapper = mapper;
    }

    public async Task<List<LogStatus>> GetLogStatus()
    {
        var logStatusList = new List<LogStatus>();
        var siteDbServer = appSettingsKey.SiteDbServer.Where(n => n != DataBaseServers.DBTrans);
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