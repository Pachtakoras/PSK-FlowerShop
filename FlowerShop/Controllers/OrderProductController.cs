using FlowerShop.DataAccess;
using FlowerShop.Logging;
using FlowerShop.Models;
using FlowerShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Controllers
{
    public class OrderProductController : Controller
    {
        private readonly IOrderProductRepo _orderRepo;
        public OrderProductController(IOrderProductRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> ViewOrderProducts(long orderId)
        {
            var view = await _orderRepo.Get(orderId);
            return View(view);
        }
    }
}
