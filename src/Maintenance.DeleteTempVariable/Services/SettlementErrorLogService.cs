using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface SettlementErrorLogService
{
    Task<SettlementErrorLog> GetLogDetails();
    Task<List<SettlementErrorLog>> GetLogs(SettlementErrorLogRequest request);
}

public class SettlementErrorLogServiceImp : SettlementErrorLogService
{
    private readonly AppSettingsKey appSettingsKey;
    private readonly IDynamicRepository repository;

    public SettlementErrorLogServiceImp(
        IDynamicRepository repository,
           IOptions<AppSettingsKey> appSettingsKey)
    {
        this.repository = repository;
        this.appSettingsKey = appSettingsKey.Value;

    }

    public async Task<List<SettlementErrorLog>> GetLogs(SettlementErrorLogRequest request)
    {
        var logList = new List<SettlementErrorLog>();
        var siteDbServer = appSettingsKey.SiteDbServer.Where(n => n != DataBaseServers.DBTrans);

        foreach (var server in siteDbServer)
        {
            var criteria = new GetLogStatusCriteria
            {
                DbServer = server
            };
            var list = (await repository.FetchAsync<SettlementErrorLog>(criteria)).ToList();
            list.ForEach(i => i.DbServer = server);

            logList.AddRange(list);
        }

        return logList;
    }

    public async Task<SettlementErrorLog> GetLogDetails()
    {
        return new SettlementErrorLog();
    }
}
