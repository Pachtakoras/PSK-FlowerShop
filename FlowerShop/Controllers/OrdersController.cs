using FlowerShop.DataAccess.Infrastructure;
using FlowerShop.Logging;
using FlowerShop.Models.ViewModels;
using FlowerShop.Models;
using FlowerShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using FlowerShop.Services;
using System.Drawing.Printing;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ILogger _logger;
        public OrdersController(IOrderRepo orderRepo, ILogger<CartController> logger)
        {
            _orderRepo = orderRepo;
            _logger = logger;
        }

        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Order> order = await _orderRepo.GetbyUserId(userId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Cancel(long id)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    await _orderRepo.Cancel(id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Error"] = "Could not cancel order";
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
