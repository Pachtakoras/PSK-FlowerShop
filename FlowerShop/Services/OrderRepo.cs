using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;

namespace FlowerShop.Services
{
    public class OrderRepo: IOrderRepo
    {
        private readonly FlowerContext _context;
        public OrderRepo(FlowerContext context)
        {
            _context = context;
        }
        public async Task Add(Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}
