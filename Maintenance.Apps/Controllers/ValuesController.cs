using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Controllers;

using Fanex.NAP.Client.Helpers.Web;

[Route("api/[controller]")]
[ApiController]
public class ValuesController(INapSession napSession) : ControllerBase
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
}
