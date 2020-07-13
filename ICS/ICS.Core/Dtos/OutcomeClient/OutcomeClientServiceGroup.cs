using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.OutcomeClient
{
    [DataContract]
    public class OutcomeClientServiceGroup
    {
        [DataMember]
        public string Uuid { get; set; }
        [DataMember]
        public DateTime CreatedAt { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<OutcomeClientShortInfo> Members { get; set; }
        [DataMember]
        public bool IsCreator { get; set; }
    }
}
