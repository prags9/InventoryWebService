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
            //InMemory Db
            _db = db;
            
               /* _inventory = new List<Inventory>()
                {
                     new Inventory
                    {
                        Name = "Apples",
                       Quantity = 10,
                       CreatedOn = DateTime.Parse("2000-2-12")
                    },
                    new Inventory
                    {
                        Name = "Oranges",
                        Quantity = 120,
                        CreatedOn = DateTime.Parse("2003-2-12")
                    }, new Inventory
                    {
                        Name = "Chiku",
                        Quantity = 190,
                        CreatedOn = DateTime.Parse("2000-2-10")
                    }
                };
                _db.Inventories.AddRange(_inventory);
                _db.SaveChanges();
            */
        }

        public async Task<bool> CreateUpdate(IEnumerable<Inventory> inventories)
        {
            var inventoryInDb = new Inventory();
            try
            {
                if (!inventories.Any())
                {
                    return false;
                }
                foreach (var inventory in inventories)
                {
                    inventoryInDb = await _db.Inventories.FirstOrDefaultAsync(x => x.Name.ToLower() == inventory.Name.ToLower());
                    if(inventoryInDb is null)
                    {
                        //Create
                        _db.Inventories.Add(inventory);

                    }
                    else
                    {
                        //Update
                        if (inventory.RowVersion != null)
                        {
                            _db.Entry(inventoryInDb).Property("RowVersion").OriginalValue = inventory.RowVersion;
                            //inventory.RowVersion = inventory.RowVersion;
                        }
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
                
                throw;
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

        public async Task<IEnumerable<Inventory>> Get(string sortOrder, string returnVal="all")
        {
            IEnumerable<Inventory> inventoryList = new List<Inventory>();
            inventoryList = from inventory in _db.Inventories select inventory;//await _db.Inventories.OrderBy(x => x.Name.ToLower()).ToListAsync();


            switch (sortOrder)
            {
                case "quantity_desc":
                    inventoryList = inventoryList.OrderByDescending(s => s.Quantity);
                    break;
                case "Date":
                    inventoryList = inventoryList.OrderBy(s => s.CreatedOn);
                    break;
                case "date_desc":
                    inventoryList = inventoryList.OrderByDescending(s => s.CreatedOn);
                    break;
                case "quantity":
                    inventoryList = inventoryList.OrderBy(s => s.Quantity);
                    break;
                case "name_desc":
                    inventoryList = inventoryList.OrderByDescending(s => s.Name);
                    break;
                case "name":
                    inventoryList = inventoryList.OrderBy(s => s.Name);
                    break;
                default:
                    inventoryList = inventoryList.OrderBy(s => s.Name);
                    break;
            }
            if (returnVal == "1")
            {
                List<Inventory> list = new List<Inventory>();
                var item = inventoryList.FirstOrDefault();
                list.Add(item);
                inventoryList = list;
            }
           
            return inventoryList;
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
