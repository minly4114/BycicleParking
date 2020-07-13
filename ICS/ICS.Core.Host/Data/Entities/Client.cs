using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Dtos.OutcomeClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ICS.Core.Host.Data.Entities
{
    public class Client
    {
        [Key]
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PastName { get; set; }
        public string Phone { get; set; }
        [ForeignKey("CredentialCard")]
        public string CredentialCardNumber { get; set; }
        public CredentialCard CredentialCard { get; set; }
        public List<ClientServiceGroup> ServiceGroups { get; set; }
        public Client()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ServiceGroups = new List<ClientServiceGroup>();
            Uuid = Guid.NewGuid();
        }

        public OutcomeClient ToOutcome(Guid uuidParking, SessionCondition[] includeConditions )
        {
            ServiceGroups.ForEach(x => x.ServiceGroup.SessionParkings.RemoveAll(y => y.ParkingPlace.ParkingUuid != uuidParking||!includeConditions.Contains(y.GetLastSessionChange().SessionCondition)));
            return new OutcomeClient()
            {
                ClientUuid = Uuid.ToString(),
                FirstName = FirstName,
                LastName = LastName,
                PastName = PastName,
                Phone = Phone,
                ServiceGroups = ServiceGroups.ConvertAll(x => x.ServiceGroup.ToOutcome()),
                CardNumber = CredentialCard?.CardNumber
            };
        }
        public OutcomeClient ToOutcome()
        {
            return new OutcomeClient()
            {
                ClientUuid = Uuid.ToString(),
                FirstName = FirstName,
                LastName = LastName,
                PastName = PastName,
                Phone = Phone,
                ServiceGroups = ServiceGroups.ConvertAll(x=>x.ServiceGroup.ToOutcome()),
                CardNumber = CredentialCard?.CardNumber
            };
        }
        public OutcomeClientShortInfo ToOutcomeShort()
        {
            return new OutcomeClientShortInfo()
            {
                FirstName = FirstName,
                LastName = LastName,
                PastName = PastName,
                CardNumber = CredentialCardNumber
            };
        }
    }
}
