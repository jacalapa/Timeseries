using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calculation.API.Database;
using Calculation.API.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Calculation.API.Services
{
    public readonly struct CalculationResult
    {
        public static readonly CalculationResult Empty = new CalculationResult(null, null);

        public CalculationResult(double? sum, double? avg)
        {
            this.Sum = sum;
            this.Average = avg;
        }

        public double? Sum { get; }

        public double? Average { get; }
    }

    public interface ICalculationService
    {
        Task<CalculationResult> CalculateAsync(string name, DateTime? from, DateTime? to);
    }

    internal class CalculationService : ICalculationService
    {
        private readonly TimeseriesDbContext _db;

        public CalculationService(TimeseriesDbContext db)
        {
            _db = db;
        }

        public async Task<CalculationResult> CalculateAsync(string name, DateTime? from, DateTime? to)
        {
            DbTimeseries timeseries = await this._db.Timeseries.FirstOrDefaultAsync(x => x.Name == name);
            if (timeseries == null)
            {
                return CalculationResult.Empty;
            }

            IReadOnlyList<DbDataPoint> points = await this.GetPointsAsync(timeseries.Id, from, to);
            if (points.Count == 0)
            {
                return CalculationResult.Empty;
            }

            double sum = points.Sum(x => x.Value);
            double avg = sum / points.Count;
            return new CalculationResult(sum, avg);
        }

        private async Task<IReadOnlyList<DbDataPoint>> GetPointsAsync(int timeseriesId, DateTime? from, DateTime? to)
        {
            IQueryable<DbDataPoint> points = this._db.DataPoints.Where(x => x.TimeseriesId == timeseriesId);

            if (from.HasValue)
            {
                points = points.Where(x => x.Time >= from.Value);
            }

            if (to.HasValue)
            {
                points = points.Where(x => x.Time <= to.Value);
            }

            return await points.ToArrayAsync();
        }
    }
}
