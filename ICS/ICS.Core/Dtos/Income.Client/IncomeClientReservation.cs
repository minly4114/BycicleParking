using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Income.Client
{
    [DataContract]
    public class IncomeClientReservation
    {
        [DataMember]
        public string UuidServiceGroup { get; set; }
        [DataMember]
        public string UuidParking { get; set; }
        [DataMember]
        public DateTime StartParking { get; set; }
        [DataMember]
        public DateTime EndParking { get; set; }
        
    }
}
