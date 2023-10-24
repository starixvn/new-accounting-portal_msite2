namespace Maintenance.Apps.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text;

public class AppController : Controller
{
    private readonly IWebHostEnvironment env;

    public AppController(IWebHostEnvironment env)
    {
        this.env = env;
    }

    [Route("delete-temp/{*path}")]
    public IActionResult Index()
    {
        var html = System.IO.File.ReadAllText($"{env.WebRootPath}/delete-temp/index.html");

        return Content(html, "text/html", Encoding.UTF8);
    }
}