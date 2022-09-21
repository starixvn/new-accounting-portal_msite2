using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Maintenance.Apps.Controllers
{
    public class AppController : Controller
    {
        private readonly IWebHostEnvironment env;

        public AppController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public IActionResult Index()
        {
            var html = System.IO.File.ReadAllText($"{env.WebRootPath}/app/index.html");

            return Content(html, "text/html", Encoding.UTF8);
        }
    }
}