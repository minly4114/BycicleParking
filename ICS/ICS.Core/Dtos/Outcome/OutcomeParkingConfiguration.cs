using ICS.Core.Dtos.Income;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeParkingConfiguration
    {
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public ReservationAllowed ReservationAllowed { get; set; }
        [DataMember]
        public int MaxNumberPlaces { get; set; }
        [DataMember]
        public int MaxNumberDay { get; set; }
        [DataMember]
        public Guid UuidParking { get; set; }
        public IncomeParkingConfiguration ToIncome()
        {
            return new IncomeParkingConfiguration()
            {
                MaxNumberDay = MaxNumberDay,
                MaxNumberPlaces = MaxNumberPlaces,
                ReservationAllowed = ReservationAllowed,
                UuidParking = UuidParking.ToString()
            };
        }
    }
}
