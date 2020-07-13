using System;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class ClusterKeepAlive
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Cluster Cluster { get; set; }
        public ClusterKeepAlive()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
