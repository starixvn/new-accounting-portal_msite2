using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Controllers
{
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
    }
}
