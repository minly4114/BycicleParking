using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Dtos.OutcomeClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class ServiceGroup
    {
        [Key]
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public Guid UuidCreator { get; set; }
        public ServiceGroupCondition Condition { get; set; }
        public List<ClientServiceGroup> Clients { get; set; }
        public List<SessionParking> SessionParkings { get; set; }

        public ServiceGroup()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
            Clients = new List<ClientServiceGroup>();
            Condition = ServiceGroupCondition.Used;
        }

        public OutcomeServiceGroup ToOutcome()
        {
            return new OutcomeServiceGroup()
            {
                Uuid = Uuid.ToString(),
                Name = Name,
                CreatedAt = CreatedAt,
                Sessions = SessionParkings?.ConvertAll(x => x.ToOutcome())
            };
        }
        public OutcomeClientShortServiceGroup ToOutcomeClientShort()
        {
            return new OutcomeClientShortServiceGroup()
            {
                Uuid = Uuid,
                Name = Name
            };
        }
        public OutcomeClientServiceGroup ToClientOutcome(Guid uuidCreator)
        {
            var outcome = new OutcomeClientServiceGroup()
            {
                Uuid = Uuid.ToString(),
                Name = Name,
                CreatedAt = CreatedAt,
                IsCreator = UuidCreator == uuidCreator,
                Members = Clients.ConvertAll(x => new OutcomeClientShortInfo()
                {
                    CardNumber = x.Client.CredentialCardNumber,
                    FirstName = x.Client.FirstName,
                    LastName = x.Client.LastName,
                    PastName = x.Client.PastName
                })
            };
            return outcome;
        }
    }
}
