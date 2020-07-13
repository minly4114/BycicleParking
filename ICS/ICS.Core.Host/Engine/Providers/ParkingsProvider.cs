using ICS.Core.Dtos.Outcome;
using ICS.Core.Host.Data;
using ICS.Core.Host.Data.Entities;
using ICS.Core.Host.Engine.IProviders;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.Providers
{
    public class ParkingsProvider : IParkingsProvider
    {
        private readonly PostgresContext _context;
        public ParkingsProvider(PostgresContext context)
        {
            _context = context;
        }

        public List<OutcomeParking> GetOutcomeParkings(Cluster cluster = null)
        {
            var parkings = _context.Parkings.Include(x => x.ParkingKeepAlives).Include(x => x.Cluster).AsQueryable();
            if (cluster != null)
            {
                parkings = parkings.Where(x => x.Cluster == cluster);
            }
            var outcomeParkings = parkings.Select(x => new OutcomeParking()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Uuid = x.Uuid,
                LastKeepAlive = x.ParkingKeepAlives.OrderByDescending(y => y.CreatedAt).FirstOrDefault().CreatedAt,
                LastParkingCondition = x.ParkingKeepAlives.OrderByDescending(y => y.CreatedAt).FirstOrDefault().ParkingCondition
            });
            return outcomeParkings.ToList();
        }

        public IQueryable<Parking> GetParkings()
        {
            return _context.Parkings.AsQueryable<Parking>();
        }

        public async Task InsertAsync(Parking parking)
        {
            await _context.Parkings.AddAsync(parking);
            await _context.SaveChangesAsync();
        }

        public async Task InsertKeepAliveAsync(ParkingKeepAlive keepAlive)
        {
            await _context.ParkingKeepAlives.AddAsync(keepAlive);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Parking parking)
        {
            _context.Parkings.Update(parking);
            await _context.SaveChangesAsync();
        }
    }
}
