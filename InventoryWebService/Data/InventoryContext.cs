using InventoryWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebService.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
           : base(options)
        {
        }
        /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             optionsBuilder.UseInMemoryDatabase(databaseName: "InventoryDb");
         }*/
        public DbSet<Inventory> Inventories { get; set; } = default!;
    }
}
