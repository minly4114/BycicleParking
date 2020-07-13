using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ICS.Core.Dtos.Income;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeCluster
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public DateTime UpdatedAt { get; set; }
        [DataMember]
        public DateTime LastKeepAlive { get; set; }
        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public int Port { get; set; }
        [DataMember]
        public bool IsConfirmed { get; set; }
        [DataMember]
        public string SupervisorUuid { get; set; }
        [DataMember]
        public List<string> ParkingUuids { get; set; }

        public IncomeCluster ToIncome()
        {
            return new IncomeCluster()
            {
                IPAddress = IPAddress,
                Name = Name,
                Port = Port,
                Uuid = new Guid(Uuid)
            };
        }
    }
}
