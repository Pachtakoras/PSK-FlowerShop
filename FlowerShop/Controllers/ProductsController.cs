
using FlowerShop.DataAccess;
using FlowerShop.Logging;
using FlowerShop.Models;
using FlowerShop.Repositories;
using FlowerShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepo _productRepo;
        private readonly ILogger _logger;
        private readonly ICategoryRepo _categoryRepo;
        public ProductsController(IProductRepo productRepo, ICategoryRepo categoryRepo,  ILogger<ProductsController> logger)
        {
            _productRepo = productRepo;
            _logger = logger;
            _categoryRepo = categoryRepo;
        }
        [ServiceFilter(typeof(LogMethod))]
        public async Task<IActionResult> Index(string categorySlug = "", int p = 1)
        {
            int pageSize = 6;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.CategorySlug = categorySlug;

            if (categorySlug == "")
            {
                ViewBag.TotalPages = (int)Math.Ceiling((decimal)_productRepo.GetCount() / pageSize);
                //return View(await _context.Products.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
                return View(await _productRepo.GetProductsByPageAsync(p, pageSize));
            }

            Category category = await _categoryRepo.GetByName(categorySlug);
            if (category == null) {
                return RedirectToAction("Index");
            }

            //var productsByCategory = _context.Products.Where(p => p.CategoryId == category.Id);
            var productsByCategory = _productRepo.GetProductsByCategoryId(category.Id);
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)productsByCategory.Count() / pageSize);
            return View(await productsByCategory.OrderByDescending(p => p.Id).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync());
        }
    }
}
