using ICS.Core.Dtos.Outcome;
using ICS.Core.Host.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.IProviders
{
    public interface IParkingPlacesProvider
    {
        IQueryable<ParkingPlace> GetParkingPlaces();
        Task InsertAsync(ParkingPlace parkingPlace);
        Task UpdateAsync(ParkingPlace parkingPlace);
        Task InsertKeepAliveAsync(ParkingPlaceKeepAlive keepAlive);
        Task<ParkingPlaceKeepAlive> InsertKeepAliveWithReturnAsync(ParkingPlaceKeepAlive keepAlive);
        List<OutcomeParkingPlace> GetOutcomePlaces();
    }
}
