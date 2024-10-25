using Fanex.Caching;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services;

public interface FlagRebateRetentionService
{
    Task<byte> GetFlagInfo();

    Task<byte> UpdateFlagInfo(UpdateFlagRebateRetentionRequest request);
}

public class FlagRebateRetentionServiceImp(ICache cache)
    : FlagRebateRetentionService
{
    private const int CacheExpiredInDays = 3650;

    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    public async Task<byte> GetFlagInfo()
    {
        return await GetStatusFromCache(CacheKey.RebateRetentionFlagCacheKey);
    }

    private async Task<byte> GetStatusFromCache(string key)
    {
        var isExistsKey = await cache.ContainAsync(key);

        if (!isExistsKey)
        {
            await cache.SetAsync(key, 1, cachePolicy);
        }

        var (_, response) = await cache.TryGetAsync<byte>(key);

        return response;
    }

    public async Task<byte> UpdateFlagInfo(UpdateFlagRebateRetentionRequest request)
    {
        await cache.SetAsync(CacheKey.RebateRetentionFlagCacheKey, request.Mode, cachePolicy);
        return request.Mode;
    }
}