using ICS.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ICS.Web.Data
{
    public class PostgresContext : IdentityDbContext
    {
        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Authorization");
            builder.Ignore<SelectListItem>();
            builder.Ignore<UserRoleModel>();
        }
        public DbSet<ICS.Web.Models.UserRoleModel> UserRoleModel { get; set; }
    }
}
