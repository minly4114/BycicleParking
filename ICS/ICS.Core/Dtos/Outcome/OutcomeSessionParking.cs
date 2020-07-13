using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeSessionParking
    {
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public string ParkingPlaceUuid { get; set; }
        [DataMember]
        public SessionCondition Condition { get; set; }
    }
}
