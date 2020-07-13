using ICS.Core.Engine.IProviders;
using ICS.Core.Host.Data;
using ICS.Core.Host.Data.Entities;
using ICS.Core.Host.Engine.IProviders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Engine.Providers
{
    public class SessionsProvider:ISessionsProvider
    {
        private readonly IDbSetProvider<SessionParking> _sessionProvider;
        public SessionsProvider(PostgresContext context, IDbSetProvider<SessionParking> sessionProvider)
        {
            _sessionProvider = sessionProvider.Build(context.SessionParkings, context);
        }

        public List<SessionParking> GetSessionReservation()
        {
            var session = _sessionProvider.GetQueryable().Include(x => x.SessionChanges).ToList();
            var list = session.Where(x => x.GetLastSessionChange().SessionCondition == Dtos.SessionCondition.Reservation).ToList();
            return list;
        }

        Task ISessionsProvider.CancelReservation(SessionParking session)
        {
            session.SessionChanges.Add(new SessionChange()
            {
                SessionCondition = Dtos.SessionCondition.ReservationCanceledServer
            });
            return _sessionProvider.UpdateAsync(session);
        }
    }
}
