using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeParkingPlace
    {
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public DateTime UpdatedAt { get; set; }
        [DataMember]
        public DateTime LastKeepAlive { get; set; }
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public int Serial { get; set; }
        [DataMember]
        public ParkingPlaceCondition LastPlaceCondition { get; set; }
    }
}
