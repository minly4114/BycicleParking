using ICS.Core.Dtos.Income;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeDialog
    {
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public Guid SessionUuid { get; set; }
        [DataMember]
        public DateTime SessionStartParking { get; set; } 
        [DataMember]
        public List<OutcomeParticipant> Participants { get; set; }
        [DataMember]
        public List<OutcomeMessage> Messages { get; set; }
        [DataMember]
        public int NumberUnread { get; set; }
    }
}
