using FlowerShop.DataAccess;
using FlowerShop.DataAccess.Infrastructure;
using FlowerShop.Models;
using FlowerShop.Models.ViewModels;
using FlowerShop.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FlowerShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepositoryDecorator _productRepo;
        public CartController(IProductRepositoryDecorator productRepo) {
            _productRepo = productRepo;
        }
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel viewModel = new()
            {
                CartItems = cart,
                Total = cart.Sum(x=> x.Quantity * x.Price),
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Add(long id)
        {
            Product product = await _productRepo.GetById(id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(p => p.ProductId == id).FirstOrDefault();

            if(cartItem == null) {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
            TempData["Success"] = "The product has been added!";
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Decrease(long id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cart.Where(p => p.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == id);
            }
            if(cart.Count == 0) {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["Success"] = "The product was decreased!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(long id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cart.Where(p => p.ProductId == id).FirstOrDefault();
            cart.RemoveAll(p => p.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["Success"] = "The product has been Removed!";
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
