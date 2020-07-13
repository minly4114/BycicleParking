using ICS.Core.Dtos.Outcome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Web.Models
{
    public class ParkingPlaceKeepAliveModel
    {
        public List<OutcomeParkingPlaceKeepAlive> KeepAlives { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string ParkingPlaceUuid { get; set; }
    }
}
