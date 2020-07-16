using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Web.User.Models
{
    public class LocationModel
    {
        public string name { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
        public string Uuid { get; set; }
    }
}
