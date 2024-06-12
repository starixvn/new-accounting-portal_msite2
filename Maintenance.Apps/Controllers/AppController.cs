namespace Maintenance.Apps.Controllers;

using System.Text;
using Microsoft.AspNetCore.Mvc;

public class AppController(IWebHostEnvironment env) : Controller
{
    [Route("delete-temp")]
    public IActionResult Index()
    {
        var html = System.IO.File.ReadAllText($"{env.WebRootPath}/delete-temp/index.html");

        return Content(html, "text/html", Encoding.UTF8);
    }
}