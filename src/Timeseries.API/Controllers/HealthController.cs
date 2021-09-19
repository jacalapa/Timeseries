using Microsoft.AspNetCore.Mvc;

namespace Timeseries.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Check()
        {
            return this.Ok();
        }
    }
}
