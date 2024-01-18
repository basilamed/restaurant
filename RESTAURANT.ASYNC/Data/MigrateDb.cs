using Microsoft.EntityFrameworkCore;

namespace RESTAURANT.ASYNC.Data
{
    public class MigrateDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<OrderContext>());
        }

        private static void SeedData(OrderContext? userDbContext)
        {
            Console.WriteLine("Applying Migrations...");
            userDbContext?.Database.Migrate();
        }
    }
}
