using FlowerShop.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Repositories
{
    public interface ICategoryRepo
    {
        Task<Category> GetByName(string name);
        DbSet<Category> GetList();
    }
}
