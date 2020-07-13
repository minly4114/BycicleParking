using ICS.Core.Dtos.Outcome;
using ICS.Core.Host.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.IProviders
{
    public interface IParkingsProvider
    {
        IQueryable<Parking> GetParkings();
        Task InsertAsync(Parking parking);
        Task UpdateAsync(Parking parking);
        public Task InsertKeepAliveAsync(ParkingKeepAlive keepAlive);
        List<OutcomeParking> GetOutcomeParkings(Cluster cluster = null);
    }
}
