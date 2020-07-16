using System.Security.Claims;
using ICS.Web.User.CoreAdapters.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICS.Web.User.Controllers
{
    public class HistorySessionController : Controller
    {
        private readonly IClientAdapter _clientAdapter;
        public HistorySessionController(IClientAdapter clientAdapter)
        {
            _clientAdapter = clientAdapter;
        }
        // GET: HistorySessionController
        public ActionResult Index(string id)
        {
            var sessions = _clientAdapter.GetSession(User.FindFirstValue(ClaimTypes.NameIdentifier)).InternalObject;
            if (id != null && id.Length != 0)
            {
                sessions.RemoveAll(x => x.Uuid.ToString() != id);
            }

            return View(sessions);
        }

        // GET: HistorySessionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HistorySessionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HistorySessionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: HistorySessionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HistorySessionController/Edit/5
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

        // POST: HistorySessionController/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var result = _clientAdapter.CancelSession(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(result.Message);
            }
            catch
            {
                return Ok("Не удалось отменить сессию");
            }
        }
    }
}
