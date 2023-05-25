
using FlowerShop.Models;

namespace FlowerShop.Repositories
{
    public interface IOrderRepo
    {
        Task Add(Order order);
    }
}
