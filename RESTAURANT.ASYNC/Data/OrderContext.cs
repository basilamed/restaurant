using Microsoft.EntityFrameworkCore;
using RESTAURANT.ASYNC.Data.Models;

namespace RESTAURANT.ASYNC.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options){}
        public DbSet<Order> Orders { get; set; }
    }
}
