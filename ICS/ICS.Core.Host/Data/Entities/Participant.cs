using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Engine.IProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ICS.Core.Host.Data.Entities
{
    public class Participant
    {
        [Key]
        public Guid Uuid { get; set; }
        public TypeUser Type { get; set; }
        public List<DialogParticipant> Dialogs { get; set; }

        public OutcomeParticipant ToOutcome(IDbSetProvider<Worker> workersProvider, IDbSetProvider<Client> clientsProvider)
        {
            var outcome = new OutcomeParticipant()
            {
                Uuid = Uuid,
                Type = Type
            };
            if (Type == TypeUser.Client)
            {
                var client = clientsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == Uuid);

                outcome.FirstName = client.FirstName;
                outcome.PastName = client.PastName;
                outcome.LastName = client.LastName;
            }
            else
            {
                var worker = workersProvider.GetQueryable().FirstOrDefault(x => x.Uuid == Uuid);

                outcome.FirstName = worker.FirstName;
                outcome.PastName = worker.PastName;
                outcome.LastName = worker.LastName;
            }
            return outcome;
        }
    }
}
