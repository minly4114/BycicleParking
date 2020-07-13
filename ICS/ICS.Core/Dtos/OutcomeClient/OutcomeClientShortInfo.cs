using ICS.Core.Dtos.Outcome;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.OutcomeClient
{
    [DataContact]
    public class OutcomeClientShortInfo
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PastName { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
    }
}
