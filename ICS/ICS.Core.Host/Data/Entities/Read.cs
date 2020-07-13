using System;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class Read
    {
        [Key]
        public int Id { get; set; }
        public Guid UuidParticipant { get; set; }
        public bool IsRead { get; set; } 
    }
}
