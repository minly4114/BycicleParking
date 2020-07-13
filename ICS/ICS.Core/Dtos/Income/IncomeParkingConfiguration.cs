
using System;

namespace ICS.Core.Dtos.Income
{
    public class IncomeParkingConfiguration
    {
        public ReservationAllowed ReservationAllowed { get; set; }
        public int MaxNumberPlaces { get; set; }
        public int MaxNumberDay { get; set; }
        public string UuidSuperviser { get; set; }
        public string UuidParking { get; set; }
    }
}
