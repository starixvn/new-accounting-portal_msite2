using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.Logging;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

using Core.Constants;
using Core.Models;
using Fanex.NAP.Client;

public interface ControlSyncService
{
    bool UpdateStatusService(UpdateStatusRequest request);

    Task<List<LastLog>> GetInfoService();
}

public class ControlSyncServicesImp(ICache cache,
        ILogger logger,
        INapSession session,
        IOptions<AppSettingsKey> appSettingsKey,
        IDynamicRepository repository)
    : ControlSyncService
{
    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    private const int CacheExpiredInDays = 3650;
    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    public async Task<List<LastLog>> GetInfoService()
    {
        var latestLog = new List<LastLog>();
        foreach (var server in appSettingsKey.SiteDbServer.Where(n => n != DataBaseServers.DBCentral))
        {
            var criteria = new GetServiceLastLogCriteria
            {
                DbServer = server
            };

            var list = (await repository.FetchAsync<LastLog>(criteria)).ToList();
            list.ForEach(i => i.DbServer = server);

            latestLog.AddRange(list);
        }

        GetCurrentStatusService(latestLog);

        return latestLog;
    }

    private void GetCurrentStatusService(List<LastLog> lastLogs)
    {
        foreach (var log in lastLogs)
        {
            if (log.QueueType == QueueType.SportBook)
            {
                var isStart = GetStatusFromCache(CacheKey.SportsCacheKey);
                log.CurrentStatus = isStart;
            }
            else
            {
                var isStart = GetStatusFromCache(CacheKey.NonSportsCacheKey);
                log.CurrentStatus = isStart;
            }
        }
    }

    public bool UpdateStatusService(UpdateStatusRequest request)
    {
        cache.Set(request.CacheKey, request.Status, cachePolicy);
        WriteLogChangeStatus(request.Status);
        return request.Status;
    }

    private void WriteLogChangeStatus(bool status)
    {
        var message = $"UpdateStatus: At {DateTimeOffset.Now} {session.UserName} has id {session.UserId} update status with value {status}";
        logger.Info(message);
    }

    private bool GetStatusFromCache(string key)
    {
        var isExistsKey = cache.Contain(key);

        if (!isExistsKey)
        {
            cache.Set(key, true, cachePolicy);
        }

        cache.TryGet(key, out bool status);

        return status;
    }
}