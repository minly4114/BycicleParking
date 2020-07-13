using ICS.Core.Dtos.Income;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Outcome
{
    [DataContract]
    public class OutcomeClient
    {
        [DataMember]
        public string ClientUuid { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string PastName { get; set; }
        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string CardNumber { get; set; }

        [DataMember]
        public List<OutcomeServiceGroup> ServiceGroups { get; set; }

        public IncomeClient ToIntcome()
        {
            return new IncomeClient()
            {
                ClientUuid = ClientUuid,
                FirstName = FirstName,
                LastName = LastName,
                PastName = PastName,
                Phone = Phone,
                CardNumber = CardNumber
            };
        }
    }
}
