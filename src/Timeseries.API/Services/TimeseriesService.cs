using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timeseries.API.Database;
using Timeseries.API.Database.Model;
using Timeseries.Common.Model;

namespace Timeseries.API.Services
{
    public interface ITimeseriesService
    {
        Task UpdateAsync(TimeseriesUpdate update);
    }

    internal class TimeseriesService : ITimeseriesService
    {
        private readonly TimeseriesDbContext _db;
        private readonly IEventPublisher _eventPublisher;

        public TimeseriesService(TimeseriesDbContext db, IEventPublisher eventPublisher)
        {
            _db = db;
            _eventPublisher = eventPublisher;
        }

        public async Task UpdateAsync(TimeseriesUpdate update)
        {
            using (var transaction = await this._db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted))
            {
                DbRevision revision = new DbRevision
                {
                    Time = DateTime.UtcNow
                };

                await this._db.Revisions.AddAsync(revision);

                DbTimeseries timeseries = await this._db.Timeseries
                    .Where(x => x.Name == update.Name)
                    .FirstOrDefaultAsync();

                if (timeseries == null)
                {
                    timeseries = new DbTimeseries
                    {
                        Name = update.Name,
                        DataPoints = update.DataPoints.Select(x => Convert(x, revision)).ToList()
                    };

                    await this._db.Timeseries.AddAsync(timeseries);
                }
                else
                {
                    foreach (var point in update.DataPoints)
                    {
                        DbDataPoint dbDataPoint = Convert(point, revision, timeseries.Id);
                        await this._db.DataPoints.AddAsync(dbDataPoint);
                    }
                }

                await this._db.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            await this._eventPublisher.PublishAsync(update);
        }

        private static DbDataPoint Convert(TimeseriesDataPoint point, DbRevision revision, int? timeseriesId = null)
        {
            DbDataPoint dbPoint = new DbDataPoint
            {
                Time = point.Time,
                Value = point.Value,
                Revision = revision
            };

            if (timeseriesId.HasValue)
            {
                dbPoint.TimeseriesId = timeseriesId.Value;
            }

            return dbPoint;
        }
    }
}
