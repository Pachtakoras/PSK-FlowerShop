using FlowerShop.Logging;
using FlowerShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlowerShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ServiceFilter(typeof(LogMethod))]
        public IActionResult Index()
        {
            return View();
        }
        [ServiceFilter(typeof(LogMethod))]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [ServiceFilter(typeof(LogMethod))]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}