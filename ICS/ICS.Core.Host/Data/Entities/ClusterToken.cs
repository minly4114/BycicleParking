using System;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class ClusterToken
    {
        [Key]
        public Guid ClusterUuid { get; set; }
        public Guid Value { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public Cluster Cluster { get; set; }
        public ClusterToken()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ExpiredAt = DateTime.UtcNow.AddYears(1);
            Value = Guid.NewGuid();
        }
    }
}
