using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Data;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Models;


namespace TallerIDWM_Backend.Src.Repository
{
    public class OrderRepository(DataContext context) : IOrderRepository
    {
        private readonly DataContext _context = context;

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public IQueryable<Order> GetQueryableOrdersByUserId(string userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Address)
                .Include(o => o.Items);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public IQueryable<Order> GetOrdersQueryable()
        {
            return _context.Orders.Include(u => u.Address).Include(o => o.Items).AsQueryable();
        }
    }
}