using Fanex.Caching;
using Fanex.Logging;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;
using System.Web;

namespace Maintenance.DeleteTempVariable.Services;

using Core.Models;
using Fanex.NAP.Client;
using ONEbook.UM.Services;

public interface FlagBroadcastService
{
    Task<FlagBroadcast> GetFlagInfo();

    Task<bool> UpdateFlagInfo(UpdateFlagBroadcastRequest request);

    Task<bool> UpdateFlagInfo(bool status);
}

public class FlagBroadcastServiceImp(IConnectionMultiplexer connectionMultiplexer,
        ICache cache,
        ILogger logger,
        INapSession session,
        IOptions<AppSettingsKey> appSettingsKey,
        IMaintenanceService maintenanceService)
    : FlagBroadcastService
{
    private const int CacheExpiredInDays = 3650;

    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    private IDatabase Database => connectionMultiplexer.GetDatabase();

    public async Task<FlagBroadcast> GetFlagInfo()
    {
        var processing = await GetNumberMessageProcessingQueueAsync();
        var waiting = await GetNumberMessageWaitingQueueAsync();
        var queueWaitingOnService = GetListMessageWaiting(CacheKey.BroadcastServiceProductWaitingCacheKey);
        var queueWaitingOnApi = GetListMessageWaiting(CacheKey.BroadcastApiProductWaitingCacheKey);
        var queueNumber = $"Processing: {processing} - Waiting: {waiting}";

        return new FlagBroadcast
        {
            CurrentStatus = await GetStatusFromCache(CacheKey.BroadcastFlagCacheKey),
            ProductType = await GetProductTypeFromCache(CacheKey.BroadcastProductCacheKey),
            QueueNumber = queueNumber,
            QueueWaiting = $"{queueWaitingOnService} {queueWaitingOnApi}",
            IsUnderMaintenance = await maintenanceService.IsUnderMaintenanceAsync(),
            SettleUnderMaintenanceStatus = await GetStatusFromCache(CacheKey.BroadcastSettleUnderMaintenanceCacheKey),
        };
    }

    private async Task<bool> GetStatusFromCache(string key)
    {
        var isExistsKey = await cache.ContainAsync(key);

        if (!isExistsKey)
        {
            await cache.SetAsync(key, false, cachePolicy);
        }

        var (_, response) = await cache.TryGetAsync<bool>(key);

        return response;
    }

    private async Task<string> GetProductTypeFromCache(string key)
    {
        var isExistsKey = await cache.ContainAsync(key);

        if (!isExistsKey)
        {
            await cache.SetAsync(key, false, cachePolicy);
        }

        var (_, response) = await cache.TryGetAsync<string>(key);

        return response;
    }

    public async Task<bool> UpdateFlagInfo(UpdateFlagBroadcastRequest request)
    {
        var productType = string.Empty;
        if (request.Status)
        {
            productType = string.Join(",", request.ProductType);
        }

        await cache.SetAsync(CacheKey.BroadcastProductCacheKey, productType, cachePolicy);
        await cache.SetAsync(CacheKey.BroadcastFlagCacheKey, request.Status, cachePolicy);

        var message = $"Update flag deploy on Broadcast API: At {DateTimeOffset.Now} {session.UserName} has id {session.UserId} update flag with value {request.Status}";
        await logger.InfoAsync(message);

        return request.Status;
    }

    public async Task<bool> UpdateFlagInfo(bool status)
    {
        await cache.SetAsync(CacheKey.BroadcastSettleUnderMaintenanceCacheKey, status, cachePolicy);

        var message = $"Update flag settle on UM for Broadcast service: At {DateTimeOffset.Now} {session.UserName} has id {session.UserId} update flag with value {status}";
        await logger.InfoAsync(message);
        return status;
    }

    private async Task<uint> GetNumberMessageProcessingQueueAsync()
    {
        var rabbitMq = appSettingsKey.RabbitMq;
        var connectionFactory = new ConnectionFactory
        {
            UserName = rabbitMq.Username,
            Password = rabbitMq.Password,
            HostName = rabbitMq.Hosting,
            VirtualHost = rabbitMq.VirtualHost
        };

        await using var connection = await connectionFactory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        return await channel.MessageCountAsync("broadcast-api-processing-event");
    }

    private async Task<long> GetNumberMessageWaitingQueueAsync()
    {
        return await Database.ListLengthAsync(CacheKey.BroadcastServiceProductWaitingCacheKey)
            + await Database.ListLengthAsync(CacheKey.BroadcastApiProductWaitingCacheKey);
    }

    private string GetListMessageWaiting(string key)
    {
        var index = 0;
        var data = new StringBuilder();
        const string specifiedWord = ":space:";
        var list = connectionMultiplexer.GetDatabase().ListRange(key);
        foreach (var item in list)
        {
            index++;
            var value = ((string)item)?.Split(specifiedWord);
            if (value is { Length: 2 })
            {
                data.Append($"<div>{index}. Product Type: {value[0]} -  {HttpUtility.HtmlEncode(value[1])}</div>");
            }
        }

        return data.ToString();
    }
}