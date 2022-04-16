using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBankWeb.API.Controllers
{
    public class AccountsAPIController : Controller
    {
        // GET: AccountsAPIController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AccountsAPIController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountsAPIController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountsAPIController/Create
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

        // GET: AccountsAPIController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountsAPIController/Edit/5
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

        // GET: AccountsAPIController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountsAPIController/Delete/5
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
