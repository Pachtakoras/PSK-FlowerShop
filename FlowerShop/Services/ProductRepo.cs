
using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace FlowerShop.Services
{
    public class ProductRepo : IProductRepo
    {
        private readonly FlowerContext _context;

        public ProductRepo(FlowerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }
        public int GetCount()
        {
            return  _context.Products.Count();
        }

        public async Task<IEnumerable<Product>> GetProductsByPageAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                    .OrderByDescending(p => p.Id)
                    .Include(p => p.Category)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }
        public async Task<Product> GetById(long id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<Product> GetByName(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }
        public IQueryable<Product> GetProductsByCategoryId(long categoryId)
        {
            return _context.Products.Where(p => p.CategoryId == categoryId);
        }
        public async Task Add(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Product product)
        {
            _context.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<Product> GetWithCategory(long id)
        {
           return  await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
