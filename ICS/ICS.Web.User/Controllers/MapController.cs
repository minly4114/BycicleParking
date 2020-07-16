using System;
using System.Linq;
using System.Security.Claims;
using ICS.Core.Dtos.Income.Client;
using ICS.Web.User.CoreAdapters.Interfaces;
using ICS.Web.User.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ICS.Web.User.Controllers
{
    public class MapController : Controller
    {
        private readonly IClientAdapter _clientAdapter;
        public MapController(IClientAdapter clientAdapter)
        {
            _clientAdapter = clientAdapter;
        }
        // GET: Map
        public ActionResult Index(string id)
        {
            var serviceGroups = _clientAdapter.GetServiceGroup(User.FindFirstValue(ClaimTypes.NameIdentifier)).InternalObject;
            var parkings = _clientAdapter.GetParkings().InternalObject;
            int number = -1;
            if (id!=null&&id.Count()!=0)
            {
                number = parkings.FindIndex(x => x.Uuid == new Guid(id));
            }
            var stringLocations = new MapModel(JsonConvert.SerializeObject(parkings, Formatting.None), JsonConvert.SerializeObject(serviceGroups, Formatting.None), number);
            return View(stringLocations);
        }
        // Бронирование места
        [HttpPost]
        public ActionResult Reservation(string id, IncomeClientReservation model)
        {
            var status = _clientAdapter.AddReservation(model);
            return Ok(status.Message);
        }

    }
}