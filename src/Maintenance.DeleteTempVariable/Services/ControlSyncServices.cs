using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.NAP.Client.Helpers.Web;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface ControlSyncServices
{
    bool UpdateStatusServiceInRedis(UpdateStatusRequest request);

    Task<List<LastLog>> GetInfoService();
}

public class ControlSyncServicesImp : ControlSyncServices
{
    private const string CacheKey = "Settlement-Auto-Sync-Sportsbook";
    private const int CacheExpiredInDays = 3650;
    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    private readonly ICache cache;
    private readonly ILogger logger;
    private readonly INapSession session;
    private readonly IDynamicRepository repository;
    private readonly AppSettingsKey appSettingsKey;

    public ControlSyncServicesImp(
        ICache cache,
        ILogger logger,
        INapSession session,
        IOptions<AppSettingsKey> appSettingsKey,
        IDynamicRepository repository)
    {
        this.cache = cache;
        this.logger = logger;
        this.session = session;
        this.appSettingsKey = appSettingsKey.Value;
        this.repository = repository;
    }

    public async Task<List<LastLog>> GetInfoService()
    {
        var latestLog = new List<LastLog>();
        foreach (var server in appSettingsKey.SiteDbServer.Where(n => n != DataBaseServers.DBTrans))
        {
            var criteria = new GetServiceLastLogCriteria()
            {
                DbServer = server,
            };

            var list = (await repository.FetchAsync<LastLog>(criteria)).ToList();
            list.ForEach(i => i.DbServer = server);

            latestLog.AddRange(list);
        }

        var isStartService = GetStatusFromCache();
        latestLog.ForEach(x => x.CurrentStatus = isStartService);

        return latestLog;
    }

    public bool UpdateStatusServiceInRedis(UpdateStatusRequest request)
    {
        cache.Set(CacheKey, request.Status, cachePolicy);
        WriteLogChangeStatus(request.Status);
        return request.Status;
    }

    private void WriteLogChangeStatus(bool status)
    {
        var message = $"UpdateStatus: At {DateTime.Now} {session.UserName} has id {session.UserId} update status with value {status}";
        logger.Info(message);
    }

    private bool GetStatusFromCache()
    {
        var isExistsKey = cache.Contain(CacheKey);

        if (!isExistsKey)
        {
            cache.Set(CacheKey, true, cachePolicy);
        }

        cache.TryGet(CacheKey, out bool status);

        return status;
    }
}