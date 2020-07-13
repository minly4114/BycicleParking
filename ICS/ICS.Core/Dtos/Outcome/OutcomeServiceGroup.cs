using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeServiceGroup
    {
        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public List<OutcomeSessionParking> Sessions {get;set;}
    }
}
