using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Calculation.API.Database;
using Calculation.API.Database.Model;
using Microsoft.EntityFrameworkCore;
using Timeseries.Common.Model;

namespace Calculation.API.Services
{
    public interface IStorageService
    {
        Task UpdateAsync(TimeseriesUpdate update);
    }

    internal class StorageService : IStorageService
    {
        private readonly TimeseriesDbContext _db;

        public StorageService(TimeseriesDbContext db)
        {
            _db = db;
        }

        public async Task UpdateAsync(TimeseriesUpdate update)
        {
            using (var transaction = await this._db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                DbTimeseries dbTimeseries = await this._db.Timeseries
                    .AsNoTracking()
                    .Where(x => x.Name == update.Name)
                    .FirstOrDefaultAsync();

                if (dbTimeseries == null)
                {
                    dbTimeseries = new DbTimeseries
                        { Name = update.Name, DataPoints = update.DataPoints.Select(x => Convert(x)).ToList() };
                    await this._db.Timeseries.AddAsync(dbTimeseries);
                }
                else
                {
                    IReadOnlyList<DateTime> times = update.DataPoints.Select(x => x.Time).ToArray();

                    Dictionary<DateTime, DbDataPoint> pointsByTime = await this._db.DataPoints
                        .Where(x => x.TimeseriesId == dbTimeseries.Id && times.Contains(x.Time))
                        .ToDictionaryAsync(x => x.Time);

                    foreach (var dataPoint in update.DataPoints)
                    {
                        if (pointsByTime.TryGetValue(dataPoint.Time, out DbDataPoint existingDataPoint))
                        {
                            existingDataPoint.Value = dataPoint.Value;
                            continue;
                        }

                        DbDataPoint newDataPoint = Convert(dataPoint, dbTimeseries.Id);
                        await this._db.DataPoints.AddAsync(newDataPoint);
                    }
                }

                await this._db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        private static DbDataPoint Convert(TimeseriesDataPoint point, int? timeseriesId = null)
        {
            DbDataPoint dbPoint = new DbDataPoint
            {
                Time = point.Time,
                Value = point.Value
            };

            if (timeseriesId.HasValue)
            {
                dbPoint.TimeseriesId = timeseriesId.Value;
            }

            return dbPoint;
        }
    }
}
