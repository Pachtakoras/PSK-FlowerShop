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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static NuGet.Packaging.PackagingConstants;

namespace FlowerShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(IOrderRepo orderRepo, ILogger<CartController> logger, UserManager<ApplicationUser> userManager)
        {
            _orderRepo = orderRepo;
            _logger = logger;
            _userManager = userManager;
        }

        //[ServiceFilter(typeof(LogMethod))]
        //public async Task<IActionResult> Index()
        //{
        //    IEnumerable<Order> orders;
        //    if (User.IsInRole("Admin"))
        //    {
        //        orders = await _orderRepo.GetAll();
                
        //    }
        //    else 
        //    {
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        orders = await _orderRepo.GetbyUserId(userId);
        //    }
        //    var orderViewModels = await GetOrderModel(orders);
        //    return View(orderViewModels);
        //}

        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            IEnumerable<Order> orders;
            if (User.IsInRole("Admin"))
            {
                orders = await _orderRepo.GetAll();

            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                orders = await _orderRepo.GetbyUserId(userId);
            }
            orders = GetFilteredOrders(fromDate, toDate, orders);
            var orderViewModels = await GetOrderModel(orders);

            return View(orderViewModels);
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
                    return RedirectToAction("Index", "Orders");
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Approve(long id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _orderRepo.Approve(id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Error"] = "Could not approve order";
                    return RedirectToAction("Index", "Orders");
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Delivered(long id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _orderRepo.SetDelivered(id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Error"] = "Could not set delivered to order";
                    return RedirectToAction("Index", "Orders");
                }
            }
            return RedirectToAction(nameof(Index));
        }
        private IEnumerable<Order> GetFilteredOrders(DateTime? fromDate, DateTime? toDate, IEnumerable<Order> orders) 
        {
            if (fromDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate <= toDate.Value);
            }
            return orders;
        }
        private async Task<List<OrderViewModel>> GetOrderModel(IEnumerable<Order> orders) 
        {
            var orderViewModels = new List<OrderViewModel>();
            if (User.IsInRole("Admin"))
            {
                orderViewModels = new List<OrderViewModel>();
                foreach (var order in orders)
                {
                    var user = await _userManager.FindByIdAsync(order.CustomerId);
                    orderViewModels.Add(new OrderViewModel
                    {
                        Order = order,
                        User = user
                    });
                }
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                foreach (var order in orders)
                {
                    orderViewModels.Add(new OrderViewModel
                    {
                        Order = order,
                        User = user
                    });
                }
            }
            return orderViewModels;
        }
    }
}
