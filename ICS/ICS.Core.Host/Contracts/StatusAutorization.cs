using ICS.Core.Host.Data.Entities;

namespace ICS.Core.Host.Contracts
{
    public class StatusAutorization
    {
        public int StatusCode { get; set; }
        public Cluster Cluster { get; set; }
    }
}
