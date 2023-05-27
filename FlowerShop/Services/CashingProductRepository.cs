using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.InteropServices;

namespace FlowerShop.Services
{
    public class CashingProductRepository : IProductRepositoryDecorator
    {
        private readonly IProductRepo _decorated;

        private readonly IMemoryCache _memoryCache;

        public CashingProductRepository(IProductRepo decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _decorated.GetAll();
        }
        public int GetCount()
        {
            return _decorated.GetCount();
        }

        public async Task<IEnumerable<Product>> GetProductsByPageAsync(int pageNumber, int pageSize)
        {
            return await _decorated.GetProductsByPageAsync(pageNumber, pageSize);
        }
        public async Task<Product> GetByIdNotCashed(long id)
        {
            return await _decorated.GetByIdNotCashed(id);
        }
        public async Task<Product?> GetById(long id)
        {
            string key = $"product-{id}";
            return await _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _decorated.GetById(id);
                });
        }
        public async Task<Product> GetByName(string name)
        {
            return await _decorated.GetByName(name);
        }
        public IQueryable<Product> GetProductsByCategoryId(long categoryId)
        {
            return _decorated.GetProductsByCategoryId(categoryId);
        }
        public async Task Add(Product product)
        {
            await _decorated.Add(product);
        }
        public void Update(Product product)
        {
            ClearCache(product.Id);
            _decorated.Update(product);
        }
        public void ClearCache(long id)
        {
            string key = $"product-{id}";
            _memoryCache.Remove(key);
        }
        public async Task Delete(Product product)
        {
            await _decorated.Delete(product);
        }
        public async Task<Product> GetWithCategory(long id)
        {
            return await _decorated.GetWithCategory(id);
        }
    }
}
