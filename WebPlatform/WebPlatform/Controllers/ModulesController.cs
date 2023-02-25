using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using NuGet.Protocol;
using WebPlatform.Models;
using WebPlatform.Models.Enum;
using WebPlatform.Services;

namespace WebPlatform.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ICLogger _logger;
        public ModulesController(ICLogger logger)
        {
            _logger = logger;
            _logger.System("Initialized Modules Controller");
        }
        // GET: ModulesController1
        public ActionResult Index()
        {
            return View("Index",new List<Connection>());
        }
        // Add Port 
        
      
        
        public async Task<IActionResult> AddModule(string port, string appType, List<Connection> conns)
        {
            AppType _appType;
            int Port;
            if (!int.TryParse(port, out Port))
            {
                return BadRequest("Provided Port is Not Supported");
            }
            if (!Enum.TryParse<AppType>(appType.Split("-")[0], true, out _appType))
            {
                return BadRequest("Provided AppType is Not Supported");
            }
            HttpContext.Response.Cookies.Append(_appType.ToString(), Port.ToString());

            conns.Add(new Connection() { AppType = _appType, Port = Port });
            return PartialView("_Modules", conns);
        }


        // GET: ModulesController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModulesController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModulesController1/Create
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

        // GET: ModulesController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModulesController1/Edit/5
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

        // GET: ModulesController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModulesController1/Delete/5
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
