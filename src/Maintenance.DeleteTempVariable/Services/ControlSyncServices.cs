
using Fanex.Caching;
using Fanex.Logging;
using Fanex.NAP.Client.Helpers.Web;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services;
public interface ControlSyncServices
{
    bool UpdateStatusServiceInRedis(UpdateStatusRequest request);
    object GetInfoService();
}


public class ControlSyncServicesImp : ControlSyncServices
{
    private readonly ICache cache;
    private const string CacheKey = "Settlement-Auto-Sync-Sportsbook";
    private const int CacheExpiredInDays = 3650;
    private readonly CacheItemOptions cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheExpiredInDays));

    private readonly ILogger logger;
    private readonly INapSession session;

    public ControlSyncServicesImp(ICache cache, ILogger logger, INapSession session)
    {
        this.cache = cache;
        this.logger = logger;
        this.session = session;
    }

    public object GetInfoService()
    {
        var isExistsKey = cache.Contain(CacheKey);

        if (!isExistsKey)
        {
            cache.Set(CacheKey, true, cachePolicy);
        }

        cache.TryGet(CacheKey, out bool status);

        return new
        {
            Status = status
        };
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
}