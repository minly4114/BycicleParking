using ICS.Core.Dtos.Income;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICS.Core.Dtos.Outcome
{
    public class OutcomeWorker
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid Uuid { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PastName { get; set; }
        public RolePersonnel Role { get; set; }
        public bool IsConfirmed { get; set; }
        public List<OutcomeCluster> ControlledСlusters { get; set; }

        public IncomeWorker ToIncome()
        {
            return new IncomeWorker()
            {
                FirstName = FirstName,
                LastName = LastName,
                PastName = PastName,
                Role = Role,
                Uuid = Uuid.ToString(),
                ClusterUuids = ControlledСlusters?.ConvertAll(x => x.Uuid.ToString()).ToHashSet()
            };
        }
    }
}
