using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeParticipant
    {
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PastName { get; set; }
        [DataMember]
        public TypeUser Type { get; set; }
    }
}
