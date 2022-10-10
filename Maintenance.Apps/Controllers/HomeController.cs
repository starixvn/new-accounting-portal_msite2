namespace Maintenance.Apps.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        return Redirect("/delete-temp/index.html");
    }
}
