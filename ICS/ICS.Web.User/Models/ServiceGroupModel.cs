using ICS.Core.Dtos.OutcomeClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ICS.Web.User.Models
{
    [DataContract]
    public class ServiceGroupModel
    {
        [DataMember]
        public OutcomeClientServiceGroup Group { get; set; }
        [DataMember]
        public string Members { get; set; }
        [DataMember]
        public List<string> CardNumberDelete { get; set; }
    }
}
