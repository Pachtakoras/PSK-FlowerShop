
using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;
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
        private readonly IProductRepositoryDecorator _productRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductRepositoryDecorator productRepo, ICategoryRepo categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_productRepo.GetCount() / pageSize);
            return View(await _productRepo.GetProductsByPageAsync(p, pageSize));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetList(),"Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetList(), "Id", "Name", product.CategoryId);

         
            if(ModelState.IsValid)
            {
                var name = await _productRepo.GetByName(product.Name);
                if(name != null)
                {
                    ModelState.AddModelError("", "The product already exists");
                
                }
                await _productRepo.Add(product);
                TempData["Success"] = "The product has been added";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Could not add the product";
            return View(product);
        }

        public async Task<IActionResult> Edit(long id)
        {
            Product product = await _productRepo.GetWithCategory(id);
            ViewBag.Categories = new SelectList(_categoryRepo.GetList(), "Id", "Name", product.CategoryId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Product product)
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetList(), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                _productRepo.Update(product);
                TempData["Success"] = "The product has been updated";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Could not update the product";
            return View(product);
        }

        public async Task<IActionResult> Delete(long id)
        {
            Product product = await _productRepo.GetById(id);
            await _productRepo.Delete(product);
            TempData["Success"] = "Product has been deleted";
            return RedirectToAction("Index");
        }
    }
}
