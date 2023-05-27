using FlowerShop.Models;

namespace FlowerShop.Repositories
{
    public interface IOrderProductRepo
    {
        Task<IEnumerable<OrderProduct>> Get(long orderId);
    }
}
