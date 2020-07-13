using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.OutcomeClient
{
    [DataContract]
    public class OutcomeClientParking
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public float LocationLat { get; set; }
        [DataMember]
        public float LocationLng { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public int NumberParkingPlace {get;set;}
        [DataMember]
        public int NumberFreeParkingPlace { get; set; }
    }
}
