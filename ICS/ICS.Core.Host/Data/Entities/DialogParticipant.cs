using System;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class DialogParticipant
    {
        [Key]
        public int Id { get; set; }

        public Guid DialogUuid { get; set; }
        public Dialog Dialog { get; set; }

        public Guid ParticipantUuid { get; set; }
        public Participant Participant { get; set; }
    }
}
