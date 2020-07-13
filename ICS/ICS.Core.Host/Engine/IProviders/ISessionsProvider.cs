using ICS.Core.Host.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.IProviders
{
    public interface ISessionsProvider
    {
        List<SessionParking> GetSessionReservation();
        Task CancelReservation(SessionParking session);
    }
}
