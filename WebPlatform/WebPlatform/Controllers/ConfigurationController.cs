using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebPlatform.Controllers
{
    public class ConfigurationController : Controller
    {
        // GET: ConfigurationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ConfigurationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ConfigurationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConfigurationController/Create
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

        // GET: ConfigurationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ConfigurationController/Edit/5
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

        // GET: ConfigurationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConfigurationController/Delete/5
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
