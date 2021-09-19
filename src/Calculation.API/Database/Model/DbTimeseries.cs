using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculation.API.Database.Model
{
    [Table("timeseries")]
    internal class DbTimeseries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdateTime { get; set; }

        public virtual List<DbDataPoint> DataPoints { get; set; }
    }
}
