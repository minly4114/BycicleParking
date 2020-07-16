using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ICS.Core.Dtos.Income.Client;
using ICS.Core.Dtos.OutcomeClient;
using ICS.Web.User.CoreAdapters.Interfaces;
using ICS.Web.User.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ICS.Web.User.Controllers
{
    public class ServiceGroupController : Controller
    {
        private readonly IClientAdapter _clientAdapter;
        private List<OutcomeClientServiceGroup> serviceGroups;
        public ServiceGroupController(IClientAdapter clientAdapter)
        {
            _clientAdapter = clientAdapter;
        }
        // GET: ServiceGroup
        public ActionResult Index(string id)
        {
            var groups = _clientAdapter.GetServiceGroup(User.FindFirstValue(ClaimTypes.NameIdentifier)).InternalObject;
            if(id!=null&&id.Count()!=0)
            {
                groups.RemoveAll(x => x.Uuid != id);
            }
            serviceGroups = groups;
            return View(groups);
        }

        // GET: ServiceGroup/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ServiceGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncomeClientServiceGroup model)
        {
            try
            {
                model.ClientCardNumbers.RemoveAll(x => x == null);
                model.UuidCreator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = _clientAdapter.AddServiceGroup(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiceGroup/Edit/5
        public ActionResult Edit(string id)
        {
            ServiceGroupModel model = new ServiceGroupModel();
            model.Group = _clientAdapter.GetServiceGroup(User.FindFirstValue(ClaimTypes.NameIdentifier)).InternalObject.FirstOrDefault(x => x.Uuid == id);
            model.Members = JsonConvert.SerializeObject(model.Group.Members);
            return View(model); 
        }

        // POST: ServiceGroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ServiceGroupModel model, string[] Members)
        {
            try
            {
                var memberList = Members.ToList();
                memberList.RemoveAll(x => x == null);
                IncomeClientServiceGroup group = new IncomeClientServiceGroup()
                {
                    Name = model.Group.Name,
                    UuidCreator = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    ClientCardNumbers = memberList
                };
                var result = _clientAdapter.ChangeServiceGroup(id, group);
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult GetMember(string id)
        {
            var member = _clientAdapter.GetClientShort(id).InternalObject;
            return Ok(member);
        }

        // GET: ServiceGroup/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServiceGroup/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                var result = _clientAdapter.DeleteServiceGroup(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(result.Message);
            }
            catch
            {
                return Ok("Невозможно удалить группу");
            }
        }
    }
}