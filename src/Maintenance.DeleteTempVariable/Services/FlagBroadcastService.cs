using Fanex.Caching;
using Fanex.Logging;
using Fanex.NAP.Client.Helpers.Web;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Maintenance.DeleteTempVariable.Services;

public interface FlagBroadcastService
{
    Task<FlagBroadcast> GetFlagInfo();

    Task<bool> UpdateFlagInfo(UpdateFlagBroadcastRequest request);

    uint GetMessageWaitingCount(string queue);
}

public class FlagBroadcastServiceImp : FlagBroadcastService
{
    private readonly ILogger logger;

    private readonly INapSession session;

    private const int CacheExpiredInDays = 3650;

    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    private readonly ICache cache;

    private readonly AppSettingsKey appSettingsKey;

    public FlagBroadcastServiceImp(
        ICache cache,
        ILogger logger,
        INapSession session,
        IOptions<AppSettingsKey> appSettingsKey)
    {
        this.cache = cache;
        this.logger = logger;
        this.session = session;
        this.appSettingsKey = appSettingsKey.Value;
    }

    public async Task<FlagBroadcast> GetFlagInfo()
    {
        var processing = GetMessageWaitingCount("broadcast-api-processing-event");
        var waiting = GetMessageWaitingCount("broadcast-api-waiting-event");
        var queueNumber = $"Processing: {processing} - Waiting: {waiting}";

        return new FlagBroadcast
        {
            CurrentStatus = await GetStatusFromCache(CacheKey.BroadcastFlagCacheKey),
            ProductType = await GetProductTypeFromCache(CacheKey.BroadcastProductCacheKey),
            QueueNumber = queueNumber
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
        await WriteLogChangeStatus(request.Status);
        return request.Status;
    }

    private async Task WriteLogChangeStatus(bool status)
    {
        var message = $"Update flag deploy on Broadcast API: At {DateTime.Now} {session.UserName} has id {session.UserId} update flag with value {status}";
        await logger.InfoAsync(message);
    }

    public uint GetMessageWaitingCount(string queue)
    {
        var rabbitMq = appSettingsKey.RabbitMq;
        var connectionFactory = new ConnectionFactory
        {
            UserName = rabbitMq.Username,
            Password = rabbitMq.Password,
            HostName = rabbitMq.Hosting,
            VirtualHost = rabbitMq.VirtualHost
        };

        using var connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        return channel.MessageCount(queue);
    }
}