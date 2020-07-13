using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeClusterToken
    {
        [DataMember]
        public Guid Token { get; set; }
        [DataMember]
        public DateTime ExpiredAt { get; set; }
    }
}
