using ICS.Core.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class ParkingKeepAlive
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ParkingCondition ParkingCondition { get; set; }

        public Parking Parking { get; set; }
        public ParkingKeepAlive()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
