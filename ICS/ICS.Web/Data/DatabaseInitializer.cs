using ICS.Web.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ICS.Web.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            RoleInitialize(serviceProvider);
            var section = configuration.GetSection("UserAdmin");
            var userName = section.GetSection("UserLogin").Value;
            var testUserPw = section.GetSection("UserPW").Value;
            var adminID = await EnsureUser(serviceProvider, testUserPw, userName);
            await EnsureRole(serviceProvider, adminID, Constants.AdminRole);
        }
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = await userManager.FindByEmailAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    Email = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        private static void RoleInitialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }
            if (!roleManager.RoleExistsAsync(Constants.AdminRole).Result)
            {
                var IR = roleManager.CreateAsync(new IdentityRole(Constants.AdminRole)).Result;
            }
            if (!roleManager.RoleExistsAsync(Constants.EngineerRole).Result)
            {
                var IR = roleManager.CreateAsync(new IdentityRole(Constants.EngineerRole)).Result;
            }
            if (!roleManager.RoleExistsAsync(Constants.SupervisorRole).Result)
            {
                var IR = roleManager.CreateAsync(new IdentityRole(Constants.SupervisorRole)).Result;
            }
        }
    }
}
