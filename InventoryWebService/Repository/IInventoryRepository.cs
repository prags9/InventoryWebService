using InventoryWebService.Models;

namespace InventoryWebService.Repository
{
    public interface IInventoryRepository
    {
        Task<bool> CreateUpdate(IEnumerable<Inventory> inventories);
        Task<bool> Delete(string itemName);
        Task<IEnumerable<Inventory>> Get(string sortOrder, string returnVal="all");
        Task<IEnumerable<Inventory>> Search(string itemName);
        Task<Inventory> GetByItem(string itemName);
    }
}
