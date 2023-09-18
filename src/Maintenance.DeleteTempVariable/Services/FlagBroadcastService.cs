using Fanex.Caching;
using Fanex.Logging;
using Fanex.NAP.Client.Helpers.Web;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services;

public interface FlagBroadcastService
{
    Task<FlagBroadcast> GetFlagInfo();

    Task<bool> UpdateFlagInfo(UpdateFlagBroadcastRequest request);
}

public class FlagBroadcastServiceImp : FlagBroadcastService
{
    private readonly ILogger logger;

    private readonly INapSession session;

    private const int CacheExpiredInDays = 3650;

    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    private readonly ICache cache;

    public FlagBroadcastServiceImp(
        ICache cache,
        ILogger logger,
        INapSession session)
    {
        this.cache = cache;
        this.logger = logger;
        this.session = session;
    }

    public async Task<FlagBroadcast> GetFlagInfo()
    {

        return new FlagBroadcast
        {
            CurrentStatus = await GetStatusFromCache(CacheKey.BroadcastFlagCacheKey),
            ProductType = await GetProductTypeFromCache(CacheKey.BroadcastProductCacheKey)
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

}