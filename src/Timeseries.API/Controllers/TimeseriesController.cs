using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timeseries.API.Model;
using Timeseries.API.Services;
using Timeseries.Common.Extensions;
using Timeseries.Common.Model;

namespace Timeseries.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeseriesController : ControllerBase
    {
        private readonly ITimeseriesService _timeseriesService;

        public TimeseriesController(ITimeseriesService timeseriesService)
        {
            this._timeseriesService = timeseriesService;
        }

        [HttpPost]
        public async Task<IActionResult> Update(List<DtoDataPoint> model)
        {
            if (model == null || model.Count == 0 || !this.ModelState.IsValid)
            {
                return this.BadRequest("Model is not valid.");
            }

            IEnumerable<TimeseriesUpdate> updates = Convert(model);
            foreach (var update in updates)
            {
                await this._timeseriesService.UpdateAsync(update);
            }

            return this.StatusCode(201);
        }

        private IEnumerable<TimeseriesUpdate> Convert(IEnumerable<DtoDataPoint> dataPoints)
        {
            DateTime now = DateTime.UtcNow;
            var dataPointsByTimeseriesName = dataPoints.GroupBy(x => x.Name);
            foreach (var timeseriesDataPoints in dataPointsByTimeseriesName)
            {
                IReadOnlyList<TimeseriesDataPoint> points = timeseriesDataPoints.Select(Convert).ToArray();
                TimeseriesUpdate update = new TimeseriesUpdate(timeseriesDataPoints.Key, points, now);
                yield return update;
            }
        }

        private static TimeseriesDataPoint Convert(DtoDataPoint point)
        {
            DateTime time = point.T.UnixToDateTime();
            return new TimeseriesDataPoint(time, point.V);
        }
    }
}
