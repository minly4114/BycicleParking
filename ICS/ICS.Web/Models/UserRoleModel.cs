using ICS.Core.Dtos.Income;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ICS.Web.Models
{
    public class UserRoleModel
    {
        public IdentityUser User { get; set; }
        public List<string> RoleList { get; set; }
        public string Roles { get; set; }
        public List<SelectListItem> RoleItem { get; set; }
        public IncomeWorker SupervisorData { get; set; }
        public List<SelectListItem> ClusterItems { get; set; }
    }
}
