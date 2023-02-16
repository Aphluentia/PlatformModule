using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebPlatform.Models;
using WebPlatform.Services;

namespace WebPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICLogger _logger;

        public HomeController(ICLogger logger)
        {
            _logger = logger;
            _logger.System("Initialized Home Controller");
        }

        public IActionResult Index()
        {
            _logger.System("Accessing Home Controller Index");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.System("Accessing Home Controller Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.Error($"HomeController Error {Activity.Current?.Id} {HttpContext.TraceIdentifier}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}