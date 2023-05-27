using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlowerShop.Services
{
    public class OrderProductRepo : IOrderProductRepo
    {
        private readonly FlowerContext _context;

        public OrderProductRepo(FlowerContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderProduct>> Get(long orderId)
        {
            return await _context.OrderProducts
            .Where(op => op.OrderId == orderId)
            .Include(op => op.Product)
            .ToListAsync();
        }
    }
}
