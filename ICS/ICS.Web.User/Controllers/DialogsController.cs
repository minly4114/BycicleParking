using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ICS.Core.Dtos;
using ICS.Core.Dtos.Income;
using ICS.Web.User.CoreAdapters.Interfaces;
using ICS.Web.User.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace ICS.Web.User.Controllers
{
    public class DialogsController : Controller
    {

        private readonly IClientAdapter _clientAdapter;
        public DialogsController(IClientAdapter clientAdapter)
        {
            _clientAdapter = clientAdapter;
        }
        // GET: DialogController
        public ActionResult Index()
        {
            var dialogs = _clientAdapter.GetDialogs(User.FindFirstValue(ClaimTypes.NameIdentifier)).InternalObject;
            return View(dialogs);
        }

        // GET: DialogController/Messages/5
        [HttpGet]
        public ActionResult Messages(string id)
        {
            var dialogs = _clientAdapter.GetDialogs(User.FindFirstValue(ClaimTypes.NameIdentifier)).InternalObject;
            var dialog = dialogs.FirstOrDefault(x => x.Uuid.ToString() == id);
            return Ok(JsonConvert.SerializeObject(dialog.Messages) );
        }

        [HttpPost]
        public ActionResult SendMessage(IncomeMessage message)
        {
            try
            {
                message.Sender = new IncomeParticipant()
                {
                    Uuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Type = TypeUser.Client
                };
                var result = _clientAdapter.SendMessage(message);
                return Ok(result.Message);
            }
            catch
            {
                return Ok("Неудалось отправить сообщение!");
            }
        }
        [HttpPost]
        public ActionResult SendDialogIsRead(string id)
        {
            try
            {
                var result = _clientAdapter.SendStatusIsRead(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
                return Ok(result.Message);
            }
            catch
            {
                return Ok("Неудалось отправить статус о прочтении диалога!");
            }
        }

        // GET: DialogController/Create
        public ActionResult Create()
        {
            MessageModel model = new MessageModel();
            var sessions = _clientAdapter.GetSession(User.FindFirstValue(ClaimTypes.NameIdentifier)).InternalObject;
            List<SelectorModel> list = sessions.ConvertAll(x => new SelectorModel() { Name = $"{x.ParkingName} {x.StartParking.ToShortDateString()}", Value = x.Uuid.ToString() });
            model.Sessions = JsonConvert.SerializeObject(list);
            return View(model);
        }

        // POST: DialogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncomeMessage message)
        {
            try
            {
                message.Sender = new IncomeParticipant()
                {
                    Uuid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Type = TypeUser.Client
                };
                var result = _clientAdapter.SendMessage(message);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(message);
            }
        }

        // GET: DialogController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DialogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DialogController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DialogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
