using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.OutcomeClient
{
    [DataContract]
    public class OutcomeClientShortServiceGroup
    {
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
