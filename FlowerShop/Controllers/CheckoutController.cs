using FlowerShop.DataAccess;
using FlowerShop.DataAccess.Infrastructure;
using FlowerShop.Logging;
using FlowerShop.Models;
using FlowerShop.Repositories;
using FlowerShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly FlowerContext _context;
        private readonly ILogger _logger;
	      private readonly IOrderRepo _orderRepo;
        private readonly IProductRepositoryDecorator _productRepo;
        public CheckoutController(IProductRepositoryDecorator cashingRepo, IOrderRepo orderRepo, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<CheckoutController> logger)
        {
            _productRepo = cashingRepo;
            _UserManager = userManager;
	          _logger = logger;
            _orderRepo = orderRepo;
        }
        
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            List<OrderProduct> OrderProducts = new List<OrderProduct>();
            decimal totalPrice = 0;
            foreach (var item in cart)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    Product prd = await _productRepo.GetById(item.ProductId);
                    OrderProduct orderProduct = new OrderProduct()
                    {
                        Order = null,
                        Product = prd
                    };
                    OrderProducts.Add(orderProduct);
                    totalPrice += item.Price;
                }

            }

            Order order = new Order()
            {
                CustomerId = _UserManager.GetUserId(User),
                DeliveryOption = "",
                ItemsCount = OrderProducts.Count(),
                PaymentMethod = "",
                PriceTotal = totalPrice,
                OrderProducts = OrderProducts
            };
            HttpContext.Session.SetJson("Order", order);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Create(Order order)
        {

            if (ModelState.IsValid)
            {
                Order oldOrder = HttpContext.Session.GetJson<Order>("Order") ?? new Order();

                Order newOrder = new Order
                {
                    CustomerId = oldOrder.CustomerId,
                    DeliveryOption = order.DeliveryOption,
                    PaymentMethod = order.PaymentMethod,
                    OrderProducts = new List<OrderProduct>(),
                    ItemsCount = oldOrder.ItemsCount,
                    PriceTotal = oldOrder.PriceTotal
                };

                var productGroups = oldOrder.OrderProducts
                    .GroupBy(op => op.Product.Id)
                    .Select(group => new { ProductId = group.Key, Quantity = group.Count() });

                foreach (var productGroup in productGroups)
                {
                    var product = await _productRepo.GetById(productGroup.ProductId);
                    var newOrderProduct = new OrderProduct
                    {
                        ProductId = product.Id,
                        Quantity = productGroup.Quantity,
                        OrderId = newOrder.Id,
                    };
                    newOrder.OrderProducts.Add(newOrderProduct);
                }

                await _orderRepo.Add(newOrder);
                TempData["Success"] = "Your order has been confirmed!";

                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = "Could not add the product";
            return View(order);
        }
    }
}
