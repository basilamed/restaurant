using Microsoft.EntityFrameworkCore;
using RESTAURANT.ASYNC.Broker;
using RESTAURANT.ASYNC.Data;
using RESTAURANT.ASYNC.Data.DTOs;
using RESTAURANT.ASYNC.Data.Models;

namespace RESTAURANT.ASYNC.Services
{
    public class OrderService
    {
        private readonly OrderContext _context;
        private readonly IMessageBroker _messageBroker;

        public OrderService(OrderContext context, IMessageBroker messageBroker)
        {
            _context = context;
            _messageBroker = messageBroker;
        }

        public async Task<Order> CreateOrder(OrderDTO orderDTO)
        {
            var order = new Order
            {
                UserId = orderDTO.UserId,
                OrderItems = orderDTO.OrderItems,
                PhoneNumber = orderDTO.PhoneNumber,
                TotalPrice = orderDTO.TotalPrice
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            _messageBroker.publishMessage(order);

            return order;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);

        }  
        
        public async Task<List<Order>> GetOrderByUserId(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

    }
}
