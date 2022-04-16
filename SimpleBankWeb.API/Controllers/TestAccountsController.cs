using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleBankWeb.API.Controllers
{
    public class TestAccountsController : Controller
    {
        // GET: TestAccountsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TestAccountsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestAccountsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestAccountsController/Create
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

        // GET: TestAccountsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TestAccountsController/Edit/5
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

        // GET: TestAccountsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestAccountsController/Delete/5
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
