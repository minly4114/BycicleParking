using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Income
{
    [DataContract]
    public class IncomeParkingPlace
    {
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public int Serial { get; set; }
    }
}
