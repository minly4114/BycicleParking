using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ICS.Core.Dtos.Outcome;
using ICS.Web.CoreAdapters.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using ICS.Core.Dtos.Income;
using Microsoft.AspNetCore.Authorization;
using ICS.Web.Authorization;
using System.Security.Claims;

namespace ICS.Web.Controllers
{
    [Authorize(Roles = Constants.SupervisorRole)]
    public class ClustersController : Controller
    {
        private readonly IWorkerAdapter _workerAdapter;
        public ClustersController(IWorkerAdapter workerAdapter)
        {
            _workerAdapter = workerAdapter;
        }
        // GET: Cluster
        public ActionResult Index(string uuidCluster = null)
        {
            var uuidSupervisor = "";
            if (uuidCluster == null || uuidCluster.Length == 0) uuidSupervisor = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _workerAdapter.GetClusters(uuidSupervisor,uuidCluster);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return View(result.InternalObject);
            }
            //TODO
            return View(null);
        }

        // GET: Cluster/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cluster/Edit/5
        public ActionResult Edit(string id)
        {
            var uuidSupervisor = "";
            if (id == null || id.Length == 0) uuidSupervisor = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _workerAdapter.GetClusters(uuidSupervisor, id).InternalObject.FirstOrDefault(x => x.Uuid == id).ToIncome();
            if (result == null) result = new IncomeCluster();
            return View(result);
        }

        // POST: Cluster/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IncomeCluster model)
        {
            try
            {
                // TODO: Add update logic here
                var result = _workerAdapter.UpdateCluster(model);
                if (result == HttpStatusCode.OK)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Redirect(id.ToString());
                }
            }
            catch
            {
                return View();
            }
        }

        // POST: Cluster/Confirm/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm([FromQuery]string uuid, [FromQuery]bool confirm, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var result =_workerAdapter.ConfirmCluster(confirm, uuid);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}