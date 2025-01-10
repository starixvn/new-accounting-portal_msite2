using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Controllers;

using Core.Models;
using Fanex.NAP.Client;
using Fanex.SimpleMailDelivery;
using Fanex.SimpleMailDelivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

[Route("api/[controller]")]
[ApiController]
public class ValuesController(INapSession napSession, MailClient mailClient, IOptions<AppSettingsKey> appSettingsKey) : ControllerBase
{
    public IActionResult Index()
    {
        return Ok(new List<object> {
              new
              {
                    dbServer = "ABC",
                    logID= "AB",
                    statusID = "a",
                    startTime =  DateTime.Now,
                    endTime =  DateTime.Now,
                    startTimeValidation =  DateTime.Now
              }
            });
    }

    [Route("get-menu")]
    public IActionResult Menu()
    {
        var menuItemList = napSession.MenuItemList;
        var menus = menuItemList.OrderBy(m => m.DisplayOrder).ToList();

        var userName = napSession.UserName;
        return Ok(new
        {
            menus,
            userName
        });
    }

    [Route("mail")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<string> Mail()
    {
        var mail = new Mail
        {
            Subject = "Subject Test",
            Title = "Test System",
            Body = "MailBody(item)",
            MailingListId = appSettingsKey.Value.MailFunctionId
        };

        await mailClient.SendFunctionMailAsync(mail);
        return "OK test";
    }
}