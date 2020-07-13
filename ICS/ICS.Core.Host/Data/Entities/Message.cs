using ICS.Core.Dtos;
using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Engine.IProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ICS.Core.Host.Data.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public Dialog Dialog{get;set;}
        public Participant Sender { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
        public List<Read> Reads { get; set; }
        public Message()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public OutcomeMessage ToOutcome(Guid currentParticipant, IDbSetProvider<Worker> workersProvider, IDbSetProvider<Client> clientsProvider )
        {
            var outcome = new OutcomeMessage()
            {
                Text=Text,
                CreatedAt=CreatedAt               
            };
            outcome.Sender = Sender.ToOutcome(workersProvider, clientsProvider);
            if(Sender.Uuid!=currentParticipant)
            {
                outcome.IsRead = Reads.FirstOrDefault(x => x.UuidParticipant == currentParticipant).IsRead;
            }
            else
            {
                outcome.IsRead = Reads.Where(x=>x.UuidParticipant!=currentParticipant).FirstOrDefault(x => x.IsRead) != null;
                outcome.IsSender = true;
            }
            return outcome;
        }
    }
}
