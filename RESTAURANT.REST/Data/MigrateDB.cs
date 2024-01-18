using Microsoft.EntityFrameworkCore;

namespace RESTAURANT.REST.Data
{
    public class MigrateDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<UserContext>());
        }

        private static void SeedData(UserContext? userDbContext)
        {
            Console.WriteLine("Applying Migrations...");
            userDbContext?.Database.Migrate();
        }
    }
}
