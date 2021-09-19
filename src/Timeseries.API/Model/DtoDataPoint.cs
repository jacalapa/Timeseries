using System.ComponentModel.DataAnnotations;

namespace Timeseries.API.Model
{
    public class DtoDataPoint
    {
        [Required]
        public string Name { get; set; }

        public long T { get; set; }

        public double V { get; set; }
    }
}
