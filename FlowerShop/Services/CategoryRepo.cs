using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Services
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly FlowerContext _context;
        public CategoryRepo(FlowerContext context)
        {
            _context = context;
        }
        public async Task<Category> GetByName(string name)
        {
            return await _context.Categories.Where(c => c.Name == name).FirstOrDefaultAsync();
        }

        public DbSet<Category> GetList()
        {
            return _context.Categories;
        }
    }
}
