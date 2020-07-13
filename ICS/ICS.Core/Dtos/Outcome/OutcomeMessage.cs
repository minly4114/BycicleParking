using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeMessage
    {
        [DataMember]
        public OutcomeParticipant Sender { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool IsRead { get; set; }
        [DataMember]
        public bool IsSender { get; set; }
    }
}
