using ICS.Core.Dtos.Outcome;
using ICS.Core.Engine.IProviders;
using ICS.Core.Host.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class Dialog
    {
        [Key]
        public Guid Uuid { get; set; }
        public SessionParking Session { get; set; }
        public List<Message> Messages { get; set; }
        public List<DialogParticipant> Participants { get; set; }
        public DateTime CreatedAt { get; set; }

        public Dialog()
        {
            CreatedAt = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
        }

        public OutcomeDialog ToOutcome(Guid currentParticipant, IDbSetProvider<Worker> workersProvider, IDbSetProvider<Client> clientsProvider)
        {
            var outcome = new OutcomeDialog()
            {
                Uuid = Uuid,
                SessionUuid = Session.Uuid,
                SessionStartParking = Session.StartParking,
                Messages = Messages.ConvertAll(x => x.ToOutcome(currentParticipant, workersProvider, clientsProvider)),
                Participants = Participants.ConvertAll(x => x.Participant.ToOutcome(workersProvider, clientsProvider)),
                NumberUnread = 0
            };
            
            foreach(var message in outcome.Messages)
            {
                if(!message.IsSender&&!message.IsRead)
                {
                    outcome.NumberUnread++;
                }
            }    
            return outcome;
        }
    }
}
