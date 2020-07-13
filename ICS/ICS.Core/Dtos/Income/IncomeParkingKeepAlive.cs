using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Income
{
    [DataContract]
    public class IncomeParkingKeepAlive
    {
        [DataMember]
        public Guid ParkingUuid { get; set; }
        [DataMember]
        public ParkingCondition ParkingCondition { get; set; }
    }
}
