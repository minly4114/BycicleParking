using ICS.Core.Dtos.Outcome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ICS.Core.Dtos.OutcomeClient;

namespace ICS.Core.Host.Data.Entities
{
    public class Parking
    {
        [Key]
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ParkingPlace> ParkingPlaces { get; set; }
        public List<ParkingKeepAlive> ParkingKeepAlives { get; set; }
        public Cluster Cluster { get; set; }
        public List<ParkingConfiguration> ParkingConfigurations { get; set; } 

        public float LocationLat { get; set; }
        public float LocationLng { get; set; }
        public string Address { get; set; }

        public Parking()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ParkingConfigurations = new List<ParkingConfiguration>();
        }
        public OutcomeParking ToOutcome()
        {
            ParkingKeepAlive lastKeepAlive = ParkingKeepAlives!=null? ParkingKeepAlives.OrderByDescending(x => x.UpdatedAt).FirstOrDefault():null;
            return new OutcomeParking()
            {
                Name = Name,
                LocationLat = LocationLat,
                LocationLng = LocationLng,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                ParkingPlace = ParkingPlaces!=null? ParkingPlaces.ConvertAll(x => x.ToOutcome()):null,
                Uuid = Uuid,
                LastKeepAlive = lastKeepAlive != null ? lastKeepAlive.UpdatedAt : UpdatedAt,
                LastParkingCondition = lastKeepAlive != null ? lastKeepAlive.ParkingCondition : Dtos.ParkingCondition.Idle,
            };
        }
        public OutcomeClientParking ToOutcomeClient()
        {
            ParkingKeepAlive lastKeepAlive = ParkingKeepAlives != null ? ParkingKeepAlives.OrderByDescending(x => x.UpdatedAt).FirstOrDefault() : null;
            return new OutcomeClientParking()
            {
                Name = Name,
                LocationLat = LocationLat,
                LocationLng = LocationLng,
                Address = Address,
                Uuid = Uuid,
                NumberParkingPlace = ParkingPlaces!=null?ParkingPlaces.Count:0,
                NumberFreeParkingPlace = GetFreeParkingPlace().Count
            };
        }
        public List<ParkingPlace> GetFreeParkingPlace()
        {
            List<ParkingPlace> freeParkingPlaces;
            freeParkingPlaces = ParkingPlaces != null ? ParkingPlaces.Where(x => x.LastParkingPlaceKeepAlive()?.ParkingCondition == Dtos.ParkingPlaceCondition.Free).ToList():new List<ParkingPlace>();
            return freeParkingPlaces;
        }

        public ParkingConfiguration GetConfiguration()
        {
            return ParkingConfigurations.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
        }
    }
}
