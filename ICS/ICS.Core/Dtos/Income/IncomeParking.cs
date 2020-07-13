using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Income
{
    [DataContract]
    public class IncomeParking
    {
        [DataMember, Required]
        public Guid Uuid { get; set; }
        [DataMember, Required(AllowEmptyStrings=false)]
        public string Name { get; set; }
        [DataMember, Required]
        public float LocationLat { get; set; }
        [DataMember, Required]
        public float LocationLng { get; set; }
        [DataMember, Required]
        public string Address { get; set; }
        [DataMember]
        public List<IncomeParkingPlace> ParkingPlaces { get; set; }
    }
}
