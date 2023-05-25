
using FlowerShop.DataAccess;
using FlowerShop.Logging;
using FlowerShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Controllers
{
    public class ProductsController : Controller
    {

        private readonly FlowerContext _context;
        private readonly ILogger _logger;
        public ProductsController(FlowerContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Index(string categorySlug = "", int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);
                return View(await _context.Products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
            }

            Category category = await _context.Categories.Where(c => c.Name == categorySlug).FirstOrDefaultAsync();
            if (category == null) {
                return RedirectToAction("Index");
            }

            var productsByCategory = _context.Products.Where(p => p.CategoryId == category.Id);
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsByCategory.Count() / pageSize);
            return View(await productsByCategory.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
        }
    }
}
