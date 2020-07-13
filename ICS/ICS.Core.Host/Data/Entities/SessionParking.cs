using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Dtos.OutcomeClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ICS.Core.Host.Data.Entities
{
    public class SessionParking
    {
        [Key]
        public Guid Uuid { get; set; }
        public ServiceGroup ServiceGroup { get; set; }
        public ParkingPlace ParkingPlace { get; set; }
        public List<SessionChange> SessionChanges { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime StartParking { get; set; }
        public DateTime EndParking { get; set; }

        public List<Dialog> Dialogs { get; set; }

        public SessionParking()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
        }
        public OutcomeSessionParking ToOutcome()
        {
            SessionChange sessionChange = SessionChanges != null ? SessionChanges.OrderByDescending(x => x.UpdatedAt).FirstOrDefault() : null;
            var outcome = new OutcomeSessionParking()
            {
                CreatedAt = CreatedAt,
                Uuid = Uuid,
                ParkingPlaceUuid = ParkingPlace != null? ParkingPlace.Uuid.ToString():null
            };
            if (sessionChange != null) outcome.Condition = sessionChange.SessionCondition;
            return outcome;
        }

        public OutcomeClientSession ToOutcomeClient()
        {
            var outcome = new OutcomeClientSession()
            {
                Uuid = Uuid,
                StartParking = StartParking,
                EndParking = EndParking,
                UpdatedAt = UpdatedAt,
                CreatedAt = CreatedAt,
                ServiceGroupName = ServiceGroup?.Name,
            };
            if(ParkingPlace!=null&&ParkingPlace.Parking!=null)
            {
                outcome.ParkingName = ParkingPlace.Parking.Name;
                outcome.ParkingUuid = ParkingPlace.Parking.Uuid;
            }
            if(SessionChanges!=null)
            {
                outcome.CurrentCondition = GetLastSessionChange().SessionCondition;
            }
            if(ServiceGroup!=null)
            {
                outcome.ServiceGroupUuid = ServiceGroup.Uuid;
            }
            return outcome;
        }
        public SessionChange GetLastSessionChange()
        {
            return SessionChanges != null ? SessionChanges.OrderByDescending(x => x.UpdatedAt).FirstOrDefault() : null;
        }
    }
}
