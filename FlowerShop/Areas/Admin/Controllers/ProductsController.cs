
using FlowerShop.DataAccess;
using FlowerShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly FlowerContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(FlowerContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);
            return View(await _context.Products.OrderByDescending(x => x.Id)
                .Include(p => p.Category)
                .Skip((p - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories,"Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

         
            if(ModelState.IsValid)
            {
                var name = await _context.Products.FirstOrDefaultAsync(p => p.Name == product.Name);
                if(name != null)
                {
                    ModelState.AddModelError("", "The product already exists");
                
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product has been added";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Could not add the product";
            return View(product);
        }

        public async Task<IActionResult> Edit(long id)
        {
            Product product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            _context.Entry(product).State = EntityState.Detached;
            _context.Entry(product.Category).State = EntityState.Detached;
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product has been updated";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Could not update the product";
            return View(product);
        }

        public async Task<IActionResult> Delete(long id)
        {
            Product product = await _context.Products.FindAsync(id);
            _context.Remove(product);
            _context.SaveChanges();
            TempData["Success"] = "Product has been deleted";
            return RedirectToAction("Index");
        }
    }
}
