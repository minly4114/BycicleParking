using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using Npgsql.TypeHandlers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class ParkingConfiguration
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReservationAllowed ReservationAllowed { get; set; }
        public int MaxNumberPlaces { get; set; }
        public int MaxNumberDay { get; set; }
        public Parking Parking { get; set; }
        public Worker Modifying { get; set; }
        public ParkingConfiguration()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public OutcomeParkingConfiguration ToOutcome()
        {
            return new OutcomeParkingConfiguration()
            {
                CreatedAt = CreatedAt,
                MaxNumberDay = MaxNumberDay,
                MaxNumberPlaces = MaxNumberPlaces,
                ReservationAllowed = ReservationAllowed,
                UuidParking = Parking.Uuid
            };
        }
    }
}
