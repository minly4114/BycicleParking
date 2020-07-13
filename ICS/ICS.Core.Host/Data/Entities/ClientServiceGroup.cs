using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICS.Core.Host.Data.Entities
{
    public class ClientServiceGroup
    {
        [Key]
        public int Id { get; set; }
        public Guid ClientUuid { get; set; }
        public Client Client { get; set; }

        public Guid ServiceGroupUuid { get; set; }
        public ServiceGroup ServiceGroup { get; set; }
    }
}
