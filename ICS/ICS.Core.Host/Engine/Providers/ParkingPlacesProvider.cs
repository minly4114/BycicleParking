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
    public class ParkingPlacesProvider : IParkingPlacesProvider
    {
        private readonly PostgresContext _context;

        public ParkingPlacesProvider(PostgresContext context)
        {
            _context = context;
        }

        public List<OutcomeParkingPlace> GetOutcomePlaces()
        {
            var places = _context.ParkingPlaces.Include(x => x.ParkingPlaceKeepAlives).AsQueryable();
            var outcomePlaces = places.Select(x => new OutcomeParkingPlace()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Level = x.Level,
                Serial = x.Serial,
                Uuid = x.Uuid,
                LastKeepAlive = x.ParkingPlaceKeepAlives.OrderByDescending(y => y.CreatedAt).FirstOrDefault().CreatedAt,
                LastPlaceCondition = x.ParkingPlaceKeepAlives.OrderByDescending(y => y.CreatedAt).FirstOrDefault().ParkingCondition
            });
            return outcomePlaces.ToList();
        }

        public IQueryable<ParkingPlace> GetParkingPlaces()
        {
            return _context.ParkingPlaces.AsQueryable<ParkingPlace>();
        }

        public async Task InsertAsync(ParkingPlace parkingPlace)
        {
            await _context.ParkingPlaces.AddAsync(parkingPlace);
            await _context.SaveChangesAsync();
        }

        public async Task InsertKeepAliveAsync(ParkingPlaceKeepAlive keepAlive)
        {
            await _context.ParkingPlaceKeepAlives.AddAsync(keepAlive);
            await _context.SaveChangesAsync();
        }

        public async Task<ParkingPlaceKeepAlive> InsertKeepAliveWithReturnAsync(ParkingPlaceKeepAlive keepAlive)
        {
            var place = _context.ParkingPlaceKeepAlives.AddAsync(keepAlive).Result.Entity;
            await _context.SaveChangesAsync();
            return place;
        }


        public async Task UpdateAsync(ParkingPlace parkingPlace)
        {
            _context.ParkingPlaces.Update(parkingPlace);
            await _context.SaveChangesAsync();
        }
    }
}
