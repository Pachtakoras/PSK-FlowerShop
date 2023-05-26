
using FlowerShop.Models;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Repositories
{
    public interface IOrderRepo
    {
        Task Add(Order order);
        Task<IEnumerable<Order>> GetbyUserId(string id);
        Task<IEnumerable<Order>> GetAll();
        Task Cancel(long id);
        Task Approve(long id);
        Task SetDelivered(long id);
    }
}
