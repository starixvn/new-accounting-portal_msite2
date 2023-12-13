using Fanex.Caching;
using Fanex.Logging;
using Fanex.NAP.Client.Helpers.Web;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;
using System.Web;

namespace Maintenance.DeleteTempVariable.Services;

public interface FlagBroadcastService
{
    Task<FlagBroadcast> GetFlagInfo();

    Task<bool> UpdateFlagInfo(UpdateFlagBroadcastRequest request);
}

public class FlagBroadcastServiceImp(IConnectionMultiplexer connectionMultiplexer,
        ICache cache,
        ILogger logger,
        INapSession session,
        IOptions<AppSettingsKey> appSettingsKey)
    : FlagBroadcastService
{
    private const int CacheExpiredInDays = 3650;

    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    private IDatabase Database => connectionMultiplexer.GetDatabase();

    public async Task<FlagBroadcast> GetFlagInfo()
    {
        var processing = GetNumberMessageProcessingQueue();
        var waiting = GetNumberMessageWaitingQueue();
        var queueWaitingOnService = GetListMessageWaiting(CacheKey.BroadcastServiceProductWaitingCacheKey);
        var queueWaitingOnApi = GetListMessageWaiting(CacheKey.BroadcastApiProductWaitingCacheKey);
        var queueNumber = $"Processing: {processing} - Waiting: {waiting}";

        return new FlagBroadcast
        {
            CurrentStatus = await GetStatusFromCache(CacheKey.BroadcastFlagCacheKey),
            ProductType = await GetProductTypeFromCache(CacheKey.BroadcastProductCacheKey),
            QueueNumber = queueNumber,
            QueueWaiting = $"{queueWaitingOnService} {queueWaitingOnApi}"
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

    public uint GetNumberMessageProcessingQueue()
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
        return channel.MessageCount("broadcast-api-processing-event");
    }

    private long GetNumberMessageWaitingQueue()
    {
        return Database.ListLengthAsync(CacheKey.BroadcastServiceProductWaitingCacheKey).Result
            + Database.ListLengthAsync(CacheKey.BroadcastApiProductWaitingCacheKey).Result;
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