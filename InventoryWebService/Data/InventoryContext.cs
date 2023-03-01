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

        public DbSet<Inventory> Inventories { get; set; } = default!;
    }
}
