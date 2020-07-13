using ICS.Core.Dtos.Income;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Web.Models
{
    public class ConfigurationParkingModel
    {
        public IncomeParkingConfiguration ParkingConfiguration { get; set; }
        public List<SelectListItem> ReservationAllowed { get; set; }
        public string ReservationAllowedString { get; set; }
    }
}
