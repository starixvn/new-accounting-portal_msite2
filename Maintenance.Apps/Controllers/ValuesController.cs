using Fanex.NAP.Client.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
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
                    startTimeValidation =  DateTime.Now,
              }
            });
    }

    [Route("get-menu")]
    public IActionResult Menu()
    {
        var session = new NapSession(new HttpContextAccessor());
        var menuItemList = session.MenuItemList;
        var menus = menuItemList.OrderBy(m => m.DisplayOrder).ToList();
        var groupMenus = menus.GroupBy(m => m.ParentId);

        var userName = session.UserName;
        return Ok(new
        {
            menus,
            userName
        });
    }
}
