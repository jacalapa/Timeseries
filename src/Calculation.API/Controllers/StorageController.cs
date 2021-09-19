using System.Threading.Tasks;
using Calculation.API.Services;
using Microsoft.AspNetCore.Mvc;
using Timeseries.Common.Model;

namespace Calculation.API.Controllers
{
    /*
     * TODO
     * The logic should be placed somewhere else. It should be fired when new event or message occurs (message broker).
     */
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            this._storageService = storageService;
        }

        [HttpPost]
        public async Task<IActionResult> Update(TimeseriesUpdate update)
        {
            await this._storageService.UpdateAsync(update);
            return this.Ok();
        }
    }
}
