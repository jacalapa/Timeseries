using Microsoft.AspNetCore.Mvc;

namespace Calculation.API.Controllers
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
