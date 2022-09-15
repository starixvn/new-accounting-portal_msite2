using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Maintenance.DeleteTempVariable;

[Route("api/delete-temp-variable")]
// [Authorize]
[AllowAnonymous]
[ApiController]
public class DeleteTempController : ControllerBase
{
    private readonly ParlayLockingService parlayLockingService;

    private readonly SettlementLockService settlementLockService;

    private readonly TempVariableService tempVariableService;

    private readonly SystemSettingService systemSettingService;

    public DeleteTempController(
        ParlayLockingService parlayLockingService,
        SettlementLockService settlementLockService,
        TempVariableService tempVariableService,
        SystemSettingService systemSettingService)
    {
        this.parlayLockingService = parlayLockingService;
        this.settlementLockService = settlementLockService;
        this.tempVariableService = tempVariableService;
        this.systemSettingService = systemSettingService;
    }

    [HttpPost]
    [Route("get-parlay-locking")]
    public async Task<IActionResult> GetParlayLocking(ParlayLockingRequest request)
    {
        var matches = await parlayLockingService.GetMatches(request);
        return Ok(JsonConvert.SerializeObject(matches));
    }

    [HttpPost]
    [Route("delete-parlay-locking")]
    public async Task<IActionResult> DeleteParlayLocking(DeleteParlayLockingRequest request)
    {
        var result = await parlayLockingService.DeleteMatch(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Route("get-settlement-lock")]
    public async Task<IActionResult> GetSettlementLock(MatchRequest request)
    {
        var matches = await settlementLockService.GetMatches(request);
        return Ok(JsonConvert.SerializeObject(matches));
    }

    [HttpPost]
    [Route("delete-settlement-lock")]
    public async Task<IActionResult> DeleteSettlementLock(DeleteSettlementProcessingRequest request)
    {
        var result = await settlementLockService.Delete(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Route("update-settlement-lock")]
    public async Task<IActionResult> UpdateSettlementLock(UpdateSettlementLogRequest request)
    {
        var result = await settlementLockService.UpdateSettlementLogs(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Route("get-temp-variable")]
    public async Task<IActionResult> GetTempVariable(MatchRequest request)
    {
        var matches = await tempVariableService.GetMatches(request.MatchId);
        return Ok(matches);
    }

    [HttpPost]
    [Route("get-detail-temp-variable")]
    public async Task<IActionResult> DetailTempVariable(MatchTicketRequest request)
    {
        var ticketCount = await tempVariableService.TicketCount(request);
        var detail = await tempVariableService.GetMatchDetail(request);
        detail.TicketCount = ticketCount;
        detail.IsDelete = true; // Need get current permission for deleting
        detail.DbServer = request.DbServer;
        return Ok(JsonConvert.SerializeObject(detail));
    }

    [HttpPost]
    [Route("delete-temp-variable")]
    public async Task<IActionResult> DeleteTempVariable(DeleteTempRequest request)
    {
        var result = await tempVariableService.DeleteMatch(request);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpPost]
    [Route("get-system-setting")]
    public async Task<IActionResult> GetSystemSetting()
    {
        var matches = await systemSettingService.GetSystemSettings();
        return Ok(JsonConvert.SerializeObject(matches));
    }

    [HttpPost]
    [Route("update-system-setting")]
    public async Task<IActionResult> UpdateSystemSetting(UpdateSystemSettingRequest request)
    {
        var result = await systemSettingService.UpdateSystemSetting(request.DbServer);
        return Ok(JsonConvert.SerializeObject(result));
    }
}