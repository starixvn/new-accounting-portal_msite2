using Fanex.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlSyncService;

[Route("api/control-sync-service")]
[Authorize]
[ApiController]
public class ControlSyncServiceController : ControllerBase
{
    private readonly ICache cache;

    public ControlSyncServiceController(ICache cache)
    {
        this.cache = cache;
    }

    [HttpGet("GetInfo")]
    public IActionResult GetInfo()
    {
        return Ok();
    }

    [HttpGet("ControlService")]
    public IActionResult ControlService()
    {
        const int cacheExpiredInDays = 1095;
        var cachePolicy = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(cacheExpiredInDays));
        cache.Set("", true, cachePolicy);
        return Ok();
    }
}
