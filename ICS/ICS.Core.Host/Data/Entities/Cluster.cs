using ICS.Core.Dtos.Outcome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ICS.Core.Host.Data.Entities
{
    public class Cluster
    {
        [Key]
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public System.Net.IPAddress IPAddress { get; set; }
        public int Port { get; set; }
        public bool IsConfirmed { get; set; }


        public Worker Supervisor { get; set; }
        public ClusterToken Token { get; set; }
        public List<ClusterKeepAlive> KeepAlives { get; set; }
        public List<Parking> Parkings { get; set; }
        public Cluster()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsConfirmed = false;
            Token = new ClusterToken();
        }
        public OutcomeCluster ToOutcome()
        {
            var lastKeepAlive = KeepAlives.OrderByDescending(y => y.CreatedAt).FirstOrDefault();
            return new OutcomeCluster()
            {
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                IPAddress = IPAddress.ToString(),
                IsConfirmed = IsConfirmed,
                Name = Name,
                Port = Port,
                Uuid = Uuid.ToString(),
                LastKeepAlive = lastKeepAlive != null ? lastKeepAlive.UpdatedAt : UpdatedAt,
                SupervisorUuid = Supervisor?.Uuid.ToString(),
                ParkingUuids = Parkings?.ConvertAll(x=>x.Uuid.ToString())
            };
        }
    }
}
