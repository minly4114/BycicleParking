using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICS.Web.CoreAdapters.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using ICS.Web.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Org.BouncyCastle.Math.EC.Rfc7748;
using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Outcome;
using ICS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ICS.Core.Dtos;

namespace ICS.Web.Controllers
{
    [Authorize(Roles = Constants.SupervisorRole)]
    public class ParkingsController : Controller
    {
        private readonly IWorkerAdapter _workerAdapter;
        public ParkingsController(IWorkerAdapter workerAdapter)
        {
            _workerAdapter = workerAdapter;
        }
        // GET: Parking
        public ActionResult Index(string uuidCluster=null)
        {
            var uuidSupervisor = "";
            if(uuidCluster==null||uuidCluster.Length==0)uuidSupervisor = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _workerAdapter.GetParkings(uuidSupervisor, uuidCluster);
            var parking = result.InternalObject;
            return View(parking);
        }

        // GET: Parking/Details/5
        public ActionResult Details(string id)
        {
            var result = _workerAdapter.GetParkings(null, null, id);
            var parking = result.InternalObject.FirstOrDefault();
            return View(parking);
        }

        // GET: Parking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parking/Create
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

        // GET: Parking/Edit/5
        public ActionResult Edit(string id)
        {
            var configurationModel = new ConfigurationParkingModel();
            configurationModel.ParkingConfiguration = _workerAdapter.GetParkingConfiguration(id).InternalObject.ToIncome();
            List<SelectListItem> reservationAllowed = new List<SelectListItem>();
            reservationAllowed.Add(new SelectListItem()
            {
                Value = ReservationAllowed.Allowed.ToString(),
                Text = "Разрешено",
                Selected = configurationModel.ParkingConfiguration.ReservationAllowed == ReservationAllowed.Allowed
            });
            reservationAllowed.Add(new SelectListItem()
            {
                Value = ReservationAllowed.Prohibited.ToString(),
                Text = "Запрещено",
                Selected = configurationModel.ParkingConfiguration.ReservationAllowed == ReservationAllowed.Prohibited
            });
            configurationModel.ReservationAllowed = reservationAllowed;
            return View(configurationModel);
        }

        // POST: Parking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id,ConfigurationParkingModel configuration)
        {
            try
            {
                var userUuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                configuration.ParkingConfiguration.UuidSuperviser = userUuid;
                configuration.ParkingConfiguration.ReservationAllowed = (ReservationAllowed)Enum.Parse(typeof(ReservationAllowed), configuration.ReservationAllowedString);
                var result = _workerAdapter.UpdateParkingConfiguration(configuration.ParkingConfiguration);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Parking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Parking/Delete/5
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