using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ICS.Core.Dtos.Income;
using ICS.Web.User.CoreAdapters.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICS.Web.User.Controllers
{
    
    public class UserSettingsController : Controller
    {
        private readonly IClientAdapter _clientAdapter;
        public UserSettingsController(IClientAdapter clientAdapter)
        {
            _clientAdapter = clientAdapter;
        }

        // GET: UserSettings
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserSettings/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserSettings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserSettings/Edit/5
        public ActionResult Edit()
        {
            var result = _clientAdapter.GetClientInfo(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(result.InternalObject?.ToIntcome());
        }

        // POST: UserSettings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IncomeClient incomeClient)
        {
            try
            {
                incomeClient.ClientUuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _clientAdapter.UpdateSettingsClient(incomeClient);

                return RedirectToAction(nameof(Edit));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserSettings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserSettings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}