using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using TallerIDWM_Backend.Src.Models;
using TallerIDWM_Backend.Src.Interfaces;
using TallerIDWM_Backend.Src.Data;


namespace TallerIDWM_Backend.Src.Repository
{
    public class OrderRepository(DataContext context) : IOrderRepository
    {
        private readonly DataContext _context = context;

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
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
    }
}