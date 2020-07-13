using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ICS.Web.CoreAdapters.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICS.Web.Controllers
{
    public class ParkingPlacesController : Controller
    {
        private readonly IWorkerAdapter _workerAdapter;
        public ParkingPlacesController(IWorkerAdapter workerAdapter)
        {
            _workerAdapter = workerAdapter;
        }
        // GET: ParkingPlaces
        public ActionResult Index(string uuidParking=null)
        {
            var uuidSupervisor = "";
            if (uuidParking == null || uuidParking.Length == 0) uuidSupervisor = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _workerAdapter.GetParkingPlaces(uuidSupervisor, uuidParking);
            var parking = result.InternalObject;
            return View(parking);
        }

        // GET: ParkingPlaces/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ParkingPlaces/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParkingPlaces/Create
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

        // GET: ParkingPlaces/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ParkingPlaces/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ParkingPlaces/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParkingPlaces/Delete/5
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