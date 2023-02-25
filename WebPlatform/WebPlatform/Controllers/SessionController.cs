using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using WebPlatform.Models.Enum;
using Microsoft.Extensions.Logging;
using WebPlatform.Services;

namespace WebPlatform.Controllers
{
    public class SessionController : Controller
    {
        private readonly ICLogger _logger;
        public SessionController(ICLogger logger) {

            _logger = logger;
            _logger.System("Initialized Modules Controller");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("[controller]/UpdateSocketPorts")]
        public IActionResult UpdateSocketPorts(string port, string appType)
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
            HttpContext.Session.SetInt32(_appType.ToString(), Port);
            HttpContext.Response.Cookies.Append(_appType.ToString(), Port.ToString());
            return RedirectToAction("Index", "Modules");
        }
    }
}
