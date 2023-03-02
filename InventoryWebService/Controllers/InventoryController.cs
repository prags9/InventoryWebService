using InventoryWebService.Models;
using InventoryWebService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InventoryWebService.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["QuantitySortParm"] = sortOrder == "quantity" ? "quantity_desc" : "quantity";
            ViewData["CreatedOnSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";           
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";           


            var inventories = await _inventoryRepository.Get(sortOrder);
            IEnumerable<Inventory> list = inventories.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                list = await _inventoryRepository.Search(searchString);
            }

            return inventories != null ? View(list) : Problem("Entity set is null");
        }

        [HttpGet("api/Inventory")]
        public async Task<IEnumerable<Inventory>> Get()
        {
            IEnumerable<Inventory> inventories = await _inventoryRepository.Get(null);
            return inventories;
           
        }       
        public async Task<IActionResult> Details(string itemName)
        {
            if (String.IsNullOrEmpty(itemName))
            {
                return NotFound();
            }

            var inventory = await _inventoryRepository.GetByItem(itemName);
                
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        [HttpGet("api/Inventory/{itemName}")]
        public async Task<object> GetById(string itemName)
        {
            if (String.IsNullOrEmpty(itemName))
            {
                return NotFound();
            }

            var inventory = await _inventoryRepository.GetByItem(itemName);

            if (inventory == null)
            {
                return NotFound();
            }
            
            return inventory;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IEnumerable<Inventory> inventories)
        {
            var isSuccess= await _inventoryRepository.CreateUpdate(inventories);
            if (isSuccess)
            {
                return View(inventories);
            }
            return View(nameof(Index));
        }

        [HttpPost("api/Inventory")]
        public async Task<object> Add([FromBody]IEnumerable<Inventory> inventories)
        {
            var isSuccess = await _inventoryRepository.CreateUpdate(inventories);
            if (isSuccess)
            {
                return inventories;
            }
            return null;
        }

        public async Task<IActionResult> Edit(string itemName)
        {
                if (string.IsNullOrEmpty(itemName))
                {
                    return NotFound();
                }

                var inventory = await _inventoryRepository.GetByItem(itemName);
                if (inventory == null)
                {
                    return NotFound(); 
                }
                return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Inventory inventoryFromForm)
        {
            List<Inventory> inventoryList = new List<Inventory>();
            try
            {                
                inventoryList.Add(inventoryFromForm);
                var inventory = await _inventoryRepository.CreateUpdate(inventoryList);
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Inventory)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty,
                        "Unable to save changes. The inventory was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Inventory)databaseEntry.ToObject();

                    if (databaseValues.Name != clientValues.Name)
                    {
                        ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                    }
                    if (databaseValues.Quantity != clientValues.Quantity)
                    {
                        ModelState.AddModelError("Quantity", $"Current value: {databaseValues.Quantity}");
                    }


                    ModelState.AddModelError(string.Empty, "The record you attempted to edit  was modified by another user after you got the original value. The edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                    databaseValues.RowVersion = (byte[])databaseValues.RowVersion;
                    ModelState.Remove("RowVersion");
                }              
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");                
            }
            return View(inventoryFromForm);
        }

        [HttpPut("api/Inventory/{itemName}")]
        public async Task<object> Update(string itemName,[FromBody] Inventory inventoryFromApi)
        {
            try
            {
                if (itemName is null)
                {
                    return NotFound();
                }
                List<Inventory> inventoryList = new List<Inventory>();
                inventoryList.Add(inventoryFromApi);

                var inventory = await _inventoryRepository.CreateUpdate(inventoryList);
                return inventoryList;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Inventory)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    ModelState.AddModelError(inventoryFromApi.Name,
                        "Unable to save changes. The inventory was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Inventory)databaseEntry.ToObject();

                    if (databaseValues.Name != clientValues.Name)
                    {
                        ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                    }
                    if (databaseValues.Quantity != clientValues.Quantity)
                    {
                        ModelState.AddModelError("Quantity", $"Current value: {databaseValues.Quantity}");
                    }


                    ModelState.AddModelError(inventoryFromApi.Name, "The record you attempted to edit  was modified by another user after you got the original value.");
                    databaseValues.RowVersion = (byte[])databaseValues.RowVersion;
                    ModelState.Remove("RowVersion");
                }
                ModelState.TryGetValue(inventoryFromApi.Name, out ModelStateEntry x);
                 return BadRequest(x);

            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                ModelState.TryGetValue(inventoryFromApi.Name, out ModelStateEntry x);
                return BadRequest(x);
            }
        }

        public async Task<IActionResult> Delete(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return NotFound();
            }

            var invventory = await _inventoryRepository.GetByItem(itemName);
            if (invventory == null)
            {
                return NotFound();
            }

            return View(invventory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Inventory inventory)
        {
            if (inventory == null)
            {
                return Problem("Some internal error occured.");
            }
            if (inventory != null)
            {
               await _inventoryRepository.Delete(inventory.Name);
            }
            
            return RedirectToAction(nameof(Index), new List<Inventory>() { inventory });
        }

        [HttpDelete("api/Inventory/{itemName}")]
        public async Task<object> DeleteInventory(string itemName)
        {
            if (itemName == null)
            {
                return NotFound();
            }
            if(await _inventoryRepository.Delete(itemName))
            {
                return String.Format($"Successfully deleted the inventory");
            }

            return Problem("Some internal error occurred");
        }

        [HttpGet("api/Inventory/values")]
        public async Task<IEnumerable<Inventory>> Get(string sortBy, string returnVal="all")
        {
            IEnumerable<Inventory> inventories = await _inventoryRepository.Get(sortBy, returnVal);
            return inventories;

        }
    }
}
