using FlowerShop.DataAccess;
using FlowerShop.Models;
using FlowerShop.Repositories;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<Order>> GetbyUserId(string id) 
        {
            return await _context.Orders.Where(o => o.CustomerId == id).ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task Cancel(long id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                order.Status = Order.orderStatus.Cancelled.ToString();
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Approve(long id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                order.Status = Order.orderStatus.Approved.ToString();
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SetDelivered(long id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                order.Status = Order.orderStatus.Delivered.ToString();
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
