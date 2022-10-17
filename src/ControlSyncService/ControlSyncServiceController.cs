using ControlSyncService.Models;
using ControlSyncService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlSyncService;

[Route("api/control-sync-service")]
[Authorize]
[ApiController]
public class ControlSyncServiceController : ControllerBase
{
    private readonly ControlSyncServices controlSyncServices;

    public ControlSyncServiceController(ControlSyncServices controlSyncServices)
    {
        this.controlSyncServices = controlSyncServices;
    }

    [HttpPost("GetInfoService")]
    public IActionResult GetInfo()
    {
        return Ok(controlSyncServices.GetInfoService());
    }

    [HttpPost("UpdateStatus")]
    public IActionResult UpdateStatus(UpdateStatusRequest request)
    {
        return Ok(controlSyncServices.UpdateStatusServiceInRedis(request));
    }
}
