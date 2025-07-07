using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        IQueryable<Order> GetOrdersQueryable();
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderByIdAsync(int orderId, string userId);
        Task<List<Order>> GetAllOrdersAsync();
        IQueryable<Order> GetQueryableOrdersByUserId(string userId);
    }
}