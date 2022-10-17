using ControlSyncService.Models;
using Fanex.Caching;

namespace ControlSyncService.Services;
public interface ControlSyncServices
{
    bool UpdateStatusServiceInRedis(UpdateStatusRequest request);
    object GetInfoService();
}


public class ControlSyncServicesImp : ControlSyncServices
{
    private readonly ICache cache;
    private const string CacheKey = "Settlement-Auto-Sync-Sportsbook";

    public ControlSyncServicesImp(ICache cache)
    {
        this.cache = cache;
    }

    public object GetInfoService()
    {
        cache.TryGet(CacheKey, out bool status);

        return new
        {
            Status = status
        };
    }

    public bool UpdateStatusServiceInRedis(UpdateStatusRequest request)
    {
        const int cacheExpiredInDays = 1095;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(cacheExpiredInDays));
        cache.Set(CacheKey, request.Status, cachePolicy);

        return request.Status;
    }
}