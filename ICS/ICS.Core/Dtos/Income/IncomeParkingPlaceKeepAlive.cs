using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Income
{
    [DataContract]
    public class IncomeParkingPlaceKeepAlive
    {
        [DataMember]
        public Guid ParkingPlaceUuid { get; set; }

        [DataMember]
        public ParkingPlaceCondition ParkingCondition { get; set; }
    }
}
