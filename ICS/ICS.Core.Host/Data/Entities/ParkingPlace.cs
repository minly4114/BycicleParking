using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ICS.Core.Host.Data.Entities
{
    public class ParkingPlace
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Key]
        public Guid Uuid { get; set; }
        public int Level { get; set; }
        public int Serial { get; set; }
        public ServiceGroup ServiceGroup { get; set; }

        public Guid ParkingUuid { get; set; }
        public Parking Parking { get; set; }
        public List<ParkingPlaceKeepAlive> ParkingPlaceKeepAlives { get; set; }
        public List<SessionParking> SessionParkings { get; set; }

        public ParkingPlace()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        public OutcomeParkingPlace ToOutcome()
        {
            ParkingPlaceKeepAlive lastKeepAlive = LastParkingPlaceKeepAlive();
            return new OutcomeParkingPlace()
            {
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                LastKeepAlive = lastKeepAlive != null ? lastKeepAlive.UpdatedAt : UpdatedAt,
                Uuid = Uuid,
                Level = Level,
                Serial = Serial,
                LastPlaceCondition = lastKeepAlive != null ? lastKeepAlive.ParkingCondition : ParkingPlaceCondition.Free,
            };
        }
        public ParkingPlaceKeepAlive LastParkingPlaceKeepAlive()
        {
            return ParkingPlaceKeepAlives!=null? ParkingPlaceKeepAlives.OrderByDescending(x => x.UpdatedAt).FirstOrDefault():null;
        }
    }
}
