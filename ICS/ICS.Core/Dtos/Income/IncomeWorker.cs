using System;
using System.Collections.Generic;
using System.Text;

namespace ICS.Core.Dtos.Income
{
    public class IncomeWorker
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PastName { get; set; }
        public RolePersonnel Role { get; set; }
        public string Uuid { get; set; }
        public HashSet<string> ClusterUuids { get; set; }
    }
}
