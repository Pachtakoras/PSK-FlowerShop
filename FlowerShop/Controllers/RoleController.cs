using Microsoft.AspNetCore.Mvc;

namespace FlowerShop.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
