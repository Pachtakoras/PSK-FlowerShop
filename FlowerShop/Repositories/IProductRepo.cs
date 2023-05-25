using FlowerShop.Models;

namespace FlowerShop.Repositories
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAll();
        int GetCount();

        Task<Product> GetById(long id);

        Task<Product> GetWithCategory(long id);

        Task<Product> GetByName(string name);

        Task Add(Product product);

        Task Update(Product product);

        Task Delete(Product product);

        Task<IEnumerable<Product>> GetProductsByPageAsync(int pageNumber, int pageSize);
        IQueryable<Product> GetProductsByCategoryId(long categoryId);

    }
}
