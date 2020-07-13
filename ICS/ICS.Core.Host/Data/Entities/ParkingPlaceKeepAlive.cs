using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class ParkingPlaceKeepAlive
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ParkingPlaceCondition ParkingCondition { get; set; }
        public ParkingPlace ParkingPlace { get; set; }
        public ParkingPlaceKeepAlive()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public OutcomeParkingPlaceKeepAlive ToOutcome()
        {
            return new OutcomeParkingPlaceKeepAlive()
            {
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                ParkingCondition = ParkingCondition,
                ParkingPlaceUuid = ParkingPlace?.Uuid.ToString()
            };
        }
    }
}
