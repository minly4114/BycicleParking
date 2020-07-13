using System;
using System.Collections.Generic;
using System.Text;

namespace ICS.Core.Dtos.OutcomeClient
{
    public class OutcomeClientSession
    {
        public Guid Uuid { get; set; }
        public Guid ServiceGroupUuid { get; set; }
        public string ServiceGroupName { get; set; }
        public Guid ParkingUuid { get; set; }
        public string ParkingName { get; set; }
        public SessionCondition CurrentCondition { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DateTime StartParking { get; set; }
        public DateTime EndParking { get; set; }
    }
}
