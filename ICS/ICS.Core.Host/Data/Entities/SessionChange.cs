using ICS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Data.Entities
{
    public class SessionChange
    {
        [Key]
        public int Id { get; set; }
        public Guid SessionParkingUuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public SessionCondition SessionCondition { get; set; }

        public SessionParking SessionParking { get; set; }
        public ParkingPlaceKeepAlive ParkingPlaceKeepAlive { get; set; }
        public SessionChange()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
