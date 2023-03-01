using InventoryWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebService.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new InventoryContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<InventoryContext>>()))
            {
                // Look for any movies.
                if (context.Inventories.Any())
                {
                    return;   // DB has been seeded
                }
                context.Inventories.AddRange(
                    new Inventory
                    {
                        Name = "Apples",
                       Quantity = 10,
                       CreatedOn = DateTime.Parse("2000-2-12")
                    },
                    new Inventory
                    {
                        Name = "Oranges",
                        Quantity = 10,
                        CreatedOn = DateTime.Parse("2003-2-12")
                    }, new Inventory
                    {
                        Name = "Chiku",
                        Quantity = 10,
                        CreatedOn = DateTime.Parse("2000-2-10")
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
