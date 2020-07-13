using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICS.Web.Models
{
    public class AccountManagement
    {
        [Key]
        public int Id { get; set; }
        public List<UserRoleModel> UserRoles { get; set; }
        public int ColCount { get; set; }
    }
}
