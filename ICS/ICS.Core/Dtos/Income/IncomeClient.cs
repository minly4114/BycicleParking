using System.Runtime.Serialization;

namespace ICS.Core.Dtos.Income
{
    [DataContract]
    public class IncomeClient
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
    }
}
