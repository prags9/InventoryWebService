using InventoryWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebService.Data
{
    public class SeedData : ISeedData
    {
        public void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new InventoryContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<InventoryContext>>()))
            {                
                if (context.Inventories.Any())
                {
                    return;   // DB has been seeded
                }
                context.Inventories.AddRange(
                    new Inventory
                    {
                        Name = "Apples",
                        Quantity = 9,
                        CreatedOn = DateTime.Parse("2000-2-12")
                    },
                    new Inventory
                    {
                        Name = "Oranges",
                        Quantity = 290,
                        CreatedOn = DateTime.Parse("2003-2-12")
                    }, new Inventory
                    {
                        Name = "Chiku",
                        Quantity = 90,
                        CreatedOn = DateTime.Parse("2000-2-10")
                    },
                     new Inventory
                     {
                         Name = "Peach",
                         Quantity = 150,
                         CreatedOn = DateTime.Parse("2013-2-09")
                     }, new Inventory
                     {
                         Name = "Watermelon",
                         Quantity = 100,
                         CreatedOn = DateTime.Parse("2009-10-10")
                     }

                );
                context.SaveChanges();
            }
        }
    }
}
