using ICS.Core.Dtos.Outcome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace ICS.Core.Dtos.Income
{
    [DataContact]
    public class IncomeMessage
    {
        [DataMember]
        public Guid? DialogUuid { get; set; }
        [DataMember, Required]
        public Guid SessionUuid { get; set; }
        [DataMember, Required]
        public IncomeParticipant Sender { get; set; }
        [DataMember, Required]
        public string Text { get; set; }
    }
}
