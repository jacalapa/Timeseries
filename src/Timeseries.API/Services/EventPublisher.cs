using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Timeseries.Common.Model;

namespace Timeseries.API.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync(TimeseriesUpdate update);
    }

    internal class EventPublisher : IEventPublisher
    {
        private readonly string _calculateServiceUpdateStorageUrl;

        public EventPublisher(IOptions<AppSettings> appSettings)
        {
            AppSettings settings = appSettings.Value;
            this._calculateServiceUpdateStorageUrl = $"{settings.CalculationServiceUrl}Storage";
        }

        /*
         * TODO
         * It should be done using some message broker like RabbitMQ instead of calling service by HTTP
         */
        public async Task PublishAsync(TimeseriesUpdate update)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(this._calculateServiceUpdateStorageUrl, content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
