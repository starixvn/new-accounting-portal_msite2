using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.DeleteTempVariable;

[Route("api/delete-temp-variable")]
//[Authorize]
[ApiController]
public class DeleteTempController : ControllerBase
{
    private readonly ParlayLockingService parlayLockingService;
    private readonly SettlementLockService settlementLockService;
    private readonly TempVariableService tempVariableService;
    private readonly SystemSettingService systemSettingService;
    private readonly LogStatusService logStatusService;
    private readonly ControlSyncService controlSyncServices;
    private readonly SettlementErrorLogService settlementErrorLogService;


    public DeleteTempController(
        ParlayLockingService parlayLockingService,
        SettlementLockService settlementLockService,
        TempVariableService tempVariableService,
        SystemSettingService systemSettingService,
        LogStatusService logStatusService,
        ControlSyncService controlSyncServices,
        SettlementErrorLogService settlementErrorLogService)
    {
        this.parlayLockingService = parlayLockingService;
        this.settlementLockService = settlementLockService;
        this.tempVariableService = tempVariableService;
        this.systemSettingService = systemSettingService;
        this.logStatusService = logStatusService;
        this.controlSyncServices = controlSyncServices;
        this.settlementErrorLogService = settlementErrorLogService;
    }

    [HttpPost]
    [Route("get-parlay-locking")]
    public async Task<IActionResult> GetParlayLocking(ParlayLockingRequest request)
    {
        var matches = await parlayLockingService.GetMatches(request);
        return Ok(matches);
    }

    [HttpPost]
    [Route("delete-parlay-locking")]
    public async Task<IActionResult> DeleteParlayLocking(DeleteParlayLockingRequest request)
    {
        var result = await parlayLockingService.DeleteMatch(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-settlement-lock")]
    public async Task<IActionResult> GetSettlementLock(MatchRequest request)
    {
        var matches = await settlementLockService.GetMatches(request);
        return Ok(matches);
    }

    [HttpPost]
    [Route("delete-settlement-lock")]
    public async Task<IActionResult> DeleteSettlementLock(DeleteSettlementProcessingRequest request)
    {
        var result = await settlementLockService.Delete(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("update-settlement-lock")]
    public async Task<IActionResult> UpdateSettlementLock(UpdateSettlementLogRequest request)
    {
        var result = await settlementLockService.UpdateSettlementLogs(request);
        return Ok(result);
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
        return Ok(detail);
    }

    [HttpPost]
    [Route("delete-temp-variable")]
    public async Task<IActionResult> DeleteTempVariable(DeleteTempRequest request)
    {
        var result = await tempVariableService.DeleteMatch(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-system-setting")]
    public async Task<IActionResult> GetSystemSetting()
    {
        var matches = await systemSettingService.GetSystemSettings();
        return Ok(matches);
    }

    [HttpPost]
    [Route("update-system-setting")]
    public async Task<IActionResult> UpdateSystemSetting(UpdateSystemSettingRequest request)
    {
        var result = await systemSettingService.UpdateSystemSetting(request.DbServer);
        return Ok(result);
    }

    [HttpPost]
    [Route("get-log-status")]
    public async Task<IActionResult> GetLogStatus()
    {
        var matches = await logStatusService.GetLogStatus();
        return Ok(matches);
    }

    [HttpPost]
    [Route("update-log-status")]
    public async Task<IActionResult> UpdateLogStatus(UpdateLogStatusRequest request)
    {
        var result = await logStatusService.UpdateLogStatus(request);
        return Ok(result);
    }

    [HttpPost]
    [Route("check-logIds")]
    public async Task<IActionResult> CheckLogIds(CheckLogIdsRequest request) => Ok(await logStatusService.CheckLogIdsInCentral(request));

    [HttpPost("get-info-service")]
    public async Task<IActionResult> GetInfo() => Ok(await controlSyncServices.GetInfoService());


    [HttpPost("update-status")]
    public IActionResult UpdateStatus(UpdateStatusRequest request) => Ok(controlSyncServices.UpdateStatusServiceInRedis(request));

    [HttpPost("get-settlement-error-logs")]
    public async Task<IActionResult> GetSettlementErrorLogs(SettlementErrorLogRequest request) => Ok(await settlementErrorLogService.GetLogs(request));

    [HttpPost("get-details-settlement-error-log")]
    public async Task<IActionResult> GetDetailsSettlementErrorLog() => Ok(await settlementErrorLogService.GetLogDetails());
}