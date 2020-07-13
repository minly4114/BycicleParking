using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.Outcome
{
    [DataContact]
    public class OutcomeCredentialCard
    {
        [DataMember]
        public string Rfid { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public CredentialCardCondition Condition { get; set; }
    }
}
