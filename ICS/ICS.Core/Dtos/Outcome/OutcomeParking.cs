using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeParking
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public float LocationLat { get; set; }
        [DataMember]
        public float LocationLng { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public DateTime UpdatedAt { get; set; }
        [DataMember]
        public DateTime LastKeepAlive { get; set; }
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public ParkingCondition LastParkingCondition { get; set; }
        public List<OutcomeParkingPlace> ParkingPlace {get;set;}
    }
}
