using InventoryWebService.Data;
using InventoryWebService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryWebService.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryContext _db;

        public InventoryRepository(InventoryContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateUpdate(IEnumerable<Inventory> inventories)
        {
            try
            {
                foreach (var inventory in inventories)
                {
                    var inventoryInDb = await _db.Inventories.FirstOrDefaultAsync(x => x.Name.ToLower() == inventory.Name.ToLower());
                    if(inventoryInDb is null)
                    {
                        //Create
                        _db.Add(inventory);

                    }
                    else
                    {
                        //Update
                        inventoryInDb.Quantity = inventory.Quantity;
                        inventoryInDb.CreatedOn = inventory.CreatedOn;
                        inventoryInDb.LastUpdatedOn = inventory.LastUpdatedOn;
                        _db.Inventories.Update(inventoryInDb);
                    }
                    await _db.SaveChangesAsync();
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(string itemName)
        {
            try
            {
                Inventory inventoryInDb = await _db.Inventories.FirstOrDefaultAsync(x => x.Name.ToLower() == itemName.ToLower());
                if (inventoryInDb == null)
                {
                    return false;
                }
                _db.Remove(inventoryInDb);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Inventory>> Get()
        {
            IEnumerable<Inventory> inventory = new List<Inventory>();
            inventory = await _db.Inventories.OrderBy(x => x.Name.ToLower()).ToListAsync();
            return inventory;
        }

        public Task<Inventory> GetByItem(string itemName)
        {
            var inventory = _db.Inventories.FirstOrDefaultAsync(m => m.Name.ToLower() == itemName.ToLower());
            return inventory;
        }

        public async Task<IEnumerable<Inventory>> Search(string itemName)
        {
            //var inventories = await Get();
            if (itemName is not null)
            {
                return await (_db.Inventories.Where(s => s.Name.Contains(itemName)).ToListAsync());
            }
            return null;
        }
    }
}
