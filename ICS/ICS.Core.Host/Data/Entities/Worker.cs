using ICS.Core.Dtos;
using ICS.Core.Dtos.Outcome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICS.Core.Host.Data.Entities
{
    public class Worker
    {
        [Key]
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PastName { get; set; }
        public RolePersonnel Role { get; set; }
        public bool IsConfirmed { get; set; }
        public List<Cluster> ControlledСlusters { get; set; }
        public List<ParkingConfiguration> ParkingConfigurations { get; set; }
        public Worker()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsConfirmed = false;
            Uuid = Guid.NewGuid();
        }
        
        public OutcomeWorker ToOutcome(bool includeClusters)
        {
            var outcomeSupervisor = new OutcomeWorker()
            {
                UpdatedAt = UpdatedAt,
                CreatedAt = CreatedAt,
                Uuid = Uuid,
                FirstName = FirstName,
                LastName = LastName,
                PastName = PastName,
                Role = Role,
                IsConfirmed = IsConfirmed
            };
            if (includeClusters)
            {
                outcomeSupervisor.ControlledСlusters = ControlledСlusters?.ConvertAll(x => x.ToOutcome());
            }
            return outcomeSupervisor;
        }
    }
}
