using System;
using System.Threading.Tasks;
using Calculation.API.Model;
using Calculation.API.Services;
using Microsoft.AspNetCore.Mvc;
using Timeseries.Common.Extensions;

namespace Calculation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationsController : Controller
    {
        private readonly ICalculationService _calculationService;

        public CalculationsController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        [HttpGet]
        public async Task<IActionResult> Calculate(string name, long? from, long? to)
        {
            if (string.IsNullOrEmpty(name))
            {
                return this.BadRequest();
            }

            DateTime? fromTime = from.UnixToDateTime();
            DateTime? toTime = to.UnixToDateTime();
            CalculationResult calculation = await this._calculationService.CalculateAsync(name, fromTime, toTime);
            DtoCalculationResult result = new DtoCalculationResult
            {
                Sum = calculation.Sum,
                Average = calculation.Average
            };

            return this.Json(result);
        }
    }
}
