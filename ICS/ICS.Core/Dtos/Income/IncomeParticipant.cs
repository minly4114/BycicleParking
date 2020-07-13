using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.Income
{
    [DataContract]
    public class IncomeParticipant
    {
        [DataMember]
        public Guid Uuid { get; set; }
        [DataMember]
        public TypeUser Type { get; set; }
    }
}
