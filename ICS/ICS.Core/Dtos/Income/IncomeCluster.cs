using System;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Income
{
    [DataContract]
    public class IncomeCluster
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public int Port { get; set; }
    }
}
