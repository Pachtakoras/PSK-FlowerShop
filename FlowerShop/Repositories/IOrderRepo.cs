
using FlowerShop.Models;

namespace FlowerShop.Repositories
{
    public interface IOrderRepo
    {
        Task Add(Order order);
        Task<IEnumerable<Order>> GetbyUserId(string id);
        Task Cancel(long id);
    }
}
