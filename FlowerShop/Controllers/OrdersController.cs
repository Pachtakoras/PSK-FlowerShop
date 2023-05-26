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

        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> order;
            if (User.IsInRole("Admin"))
            {
                order = await _orderRepo.GetAll();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                order = await _orderRepo.GetbyUserId(userId);
            }
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> AdminIndex()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Order> order = await _orderRepo.GetAll();
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
                    return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
