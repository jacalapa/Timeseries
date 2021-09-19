using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timeseries.API.Database.Model
{
    [Table("revision")]
    public class DbRevision
    {
        [Key]
        public int Id { get; set; }

        public DateTime Time { get; set; }
    }
}
