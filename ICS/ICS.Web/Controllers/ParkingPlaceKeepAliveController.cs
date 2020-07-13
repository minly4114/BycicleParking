using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ICS.Web.CoreAdapters.Interfaces;
using ICS.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICS.Web.Controllers
{
    public class ParkingPlaceKeepAliveController : Controller
    {
        private readonly IWorkerAdapter _workerAdapter;
        public ParkingPlaceKeepAliveController(IWorkerAdapter workerAdapter)
        {
            _workerAdapter = workerAdapter;
        }
        // GET: ParkingPlaceKeepAlive
        public ActionResult Index(string parkingPlaceUuid, ParkingPlaceKeepAliveModel parkingPlaceKeepAliveModel=null)
        {
            if (parkingPlaceKeepAliveModel != null) parkingPlaceUuid = parkingPlaceKeepAliveModel.ParkingPlaceUuid;
            var result = _workerAdapter.GetParkingPlaceKeepAlives(parkingPlaceUuid);
            ParkingPlaceKeepAliveModel keepAliveModel = new ParkingPlaceKeepAliveModel();
            keepAliveModel.ParkingPlaceUuid = parkingPlaceUuid;
            if (parkingPlaceKeepAliveModel?.DateStart != DateTime.MinValue && parkingPlaceKeepAliveModel?.DateEnd != DateTime.MinValue)
            {
                keepAliveModel.KeepAlives = result.InternalObject.Where(x => x.CreatedAt > parkingPlaceKeepAliveModel.DateStart && x.CreatedAt < parkingPlaceKeepAliveModel.DateEnd).ToList();
            }
            else
            {
                keepAliveModel.KeepAlives = result.InternalObject;
            }            
            return View(keepAliveModel);
        }

        // GET: ParkingPlaceKeepAlive/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ParkingPlaceKeepAlive/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParkingPlaceKeepAlive/Create
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

        // GET: ParkingPlaceKeepAlive/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ParkingPlaceKeepAlive/Edit/5
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

        // GET: ParkingPlaceKeepAlive/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParkingPlaceKeepAlive/Delete/5
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