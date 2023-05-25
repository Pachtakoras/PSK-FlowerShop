using FlowerShop.Logging;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShop.Controllers
{
    public class RoleController : Controller
    {
        [ServiceFilter(typeof(LogMethod))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
