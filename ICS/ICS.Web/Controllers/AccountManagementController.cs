using ICS.Core.Engine.IProviders;
using ICS.Web.Authorization;
using ICS.Web.CoreAdapters.Interfaces;
using ICS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using ICS.Core.Dtos;

namespace ICS.Web.Controllers
{
    [Authorize(Roles = Constants.AdminRole)]
    public class AccountManagementController : Controller
    {
        private readonly ILog4netProvider _log;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWorkerAdapter _supervizerAdapter;

        public AccountManagementController(ILog4netProvider log, IServiceProvider serviceProvider, IWorkerAdapter supervizerAdapter)
        {
            _log = log;
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _supervizerAdapter = supervizerAdapter;
        }

        // GET: AccountMangement
        public ActionResult Index()
        {
            AccountManagement accountManagement = new AccountManagement();
            List<IdentityUser> users = _userManager.Users.ToList();
            List<UserRoleModel> userRoles = new List<UserRoleModel>();
            for (var i = 0; i < users.Count; i++)
            {
                string roles = "";
                _userManager.GetRolesAsync(users[i]).Result.ToList().ForEach(x => roles += x + Environment.NewLine);
                userRoles.Add(new UserRoleModel()
                {
                    User = users[i],
                    Roles = roles
                });
            }
            accountManagement.UserRoles = userRoles;
            return View(accountManagement);
        }

        // GET: AccountMangement/Edit/5
        public ActionResult Edit(string id)
        {
            var userRole = new UserRoleModel();
            userRole.User = _userManager.Users.FirstOrDefault(x => x.Id == id);
            userRole.RoleList = _userManager.GetRolesAsync(userRole.User).Result.ToList();
            List<SelectListItem> roleListItems = new List<SelectListItem>();
            var roles = _roleManager.Roles.ToList();
            roles.ForEach(x => roleListItems.Add(new SelectListItem(x.Name, x.Name)));
            for (int i = 0; i < roleListItems.Count; i++)
            {
                if (userRole.RoleList.FirstOrDefault(x => x.Equals(roleListItems[i].Value)) != null)
                {
                    roleListItems[i].Selected = true;
                }
            }
            userRole.RoleItem = roleListItems;
            userRole.SupervisorData = _supervizerAdapter.GetSupervisor(userRole.User.Id, true).InternalObject?.ToIncome();
            if (userRole.SupervisorData == null) userRole.SupervisorData = new Core.Dtos.Income.IncomeWorker();
            var clusters = _supervizerAdapter.GetClusters().InternalObject.FindAll(x => x.SupervisorUuid == null || x.SupervisorUuid == userRole.SupervisorData.Uuid);
            List<SelectListItem> clusterListItems = new List<SelectListItem>();
            clusters.ForEach(x => clusterListItems.Add(new SelectListItem(x.Name, x.Uuid)));
            for(int i = 0; i < clusterListItems.Count; i++)
            {
                if(userRole.SupervisorData.ClusterUuids?.FirstOrDefault(x=>x.Equals(clusterListItems[i].Value)) != null)
                {
                    clusterListItems[i].Selected = true;
                }
            }
            userRole.ClusterItems = clusterListItems;
            return View(userRole);
        }

        // POST: AccountMangement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UserRoleModel model)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
                user.UserName = model.User.UserName;
                user.Email = model.User.Email;
                user.PhoneNumber = model.User.PhoneNumber;
                user.TwoFactorEnabled = model.User.TwoFactorEnabled;
                _userManager.UpdateAsync(user).Wait();
                var userRole = _userManager.GetRolesAsync(user).Result.ToList();
                if (model.RoleList != null)
                {
                    foreach (var role in model.RoleList)
                    {
                        if (userRole == null || userRole.FirstOrDefault(x => x == role) == null)
                        {
                            var ir = _userManager.AddToRoleAsync(user, role).Result;
                        }
                    }
                }
                if (userRole != null)
                {
                    foreach (var role in userRole)
                    {
                        if (model.RoleList == null || model.RoleList.FirstOrDefault(x => x == role) == null)
                        {
                            var ir = _userManager.RemoveFromRoleAsync(user, role).Result;
                        }
                    }
                }
                model.SupervisorData.Uuid = id;
                model.SupervisorData.Role = RolePersonnel.NoRoles;
                if (model.RoleList.Contains(Constants.AdminRole))
                {
                    model.SupervisorData.Role = RolePersonnel.Administrator;
                }
                if (model.RoleList.Contains(Constants.EngineerRole))
                {
                    model.SupervisorData.Role = model.SupervisorData.Role | RolePersonnel.Engineer;
                }
                if (model.RoleList.Contains(Constants.SupervisorRole))
                {
                    model.SupervisorData.Role = model.SupervisorData.Role | RolePersonnel.Supervisor;
                }
                var result = _supervizerAdapter.RegisterSupervisor(model.SupervisorData);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return Redirect(id);
            }
        }

        // GET: AccountMangement/Delete/5
        public ActionResult Delete(string id)
        {
            var userRole = new UserRoleModel();
            userRole.User = _userManager.Users.FirstOrDefault(x => x.Id == id);
            userRole.RoleList = _userManager.GetRolesAsync(userRole.User).Result.ToList();
            List<SelectListItem> listItems = new List<SelectListItem>();
            var roles = _roleManager.Roles.ToList();
            roles.ForEach(x => listItems.Add(new SelectListItem(x.Name, x.Name)));
            for (int i = 0; i < listItems.Count; i++)
            {
                if (userRole.RoleList.FirstOrDefault(x => x.Equals(listItems[i].Value)) != null)
                {
                    listItems[i].Selected = true;
                }
            }
            userRole.RoleItem = listItems;
            return View(userRole);
        }

        // POST: AccountMangement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
                _userManager.DeleteAsync(user).Wait();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}