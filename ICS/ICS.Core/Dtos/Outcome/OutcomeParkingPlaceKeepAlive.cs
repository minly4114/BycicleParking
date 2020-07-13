using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeParkingPlaceKeepAlive
    {
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public DateTime UpdatedAt { get; set; }
        [DataMember]
        public ParkingPlaceCondition ParkingCondition { get; set; }
        [DataMember]
        public string ParkingPlaceUuid { get; set; }
    }
}
