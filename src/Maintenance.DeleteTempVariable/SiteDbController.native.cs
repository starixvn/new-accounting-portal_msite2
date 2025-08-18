using Fanex.SiteDbConnectionManager.Client.Models;
using Fanex.SiteDbConnectionManager.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Controllers
{
    [ApiController]
    [Route("api/sitedb")]
    public class SiteDbController(SiteDbService siteDbService)
        : ControllerBase
    {
        [HttpGet("get-all-sitedb")]
        public async Task<IActionResult> GetAllSiteDBs() => Ok((await siteDbService.GetAllSiteDbAsync()));

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateSiteDBStatus(UpdateSiteDbStatus request)
            => Ok(await siteDbService.UpdateSiteDbStatusAsync(request));

        [HttpPost("add-sitedb")]
        public async Task<IActionResult> AddSiteDB(UpdateSiteDbStatus request)
            => Ok(await siteDbService.AddSiteDbAsync(request));

        [HttpPost("remove-sitedb")]
        public async Task<IActionResult> RemoveSiteDb(UpdateSiteDbStatus request)
            => Ok(await siteDbService.RemoveSiteDbAsync(request.SiteDbGroupId));
    }
}