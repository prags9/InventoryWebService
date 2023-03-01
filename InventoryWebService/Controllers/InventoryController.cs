﻿using InventoryWebService.Models;
using InventoryWebService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebService.Controllers
{    
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        /*public async Task<IActionResult> Index()
        {
            IEnumerable<Inventory> inventories = await _inventoryRepository.Get();
            return inventories != null ? View(inventories.ToList()) : Problem("Entity set is null");
            
        }*/
        public async Task<IActionResult> Index(string searchString)
        {

            var inventories = await _inventoryRepository.Get();
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
            IEnumerable<Inventory> inventories = await _inventoryRepository.Get();
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
          // var inventoryInDb = await _inventoryRepository.GetByItem(itemName);
            List<Inventory> inventoryList = new List<Inventory>();
            inventoryList.Add(inventoryFromForm);

            var inventory = await _inventoryRepository.CreateUpdate(inventoryList);           
            return View(nameof(Index), inventoryList);
        }

        [HttpPut("api/Inventory/{itemName}")]
        public async Task<object> Update(string itemName,[FromBody] Inventory inventoryFromApi)
        {
            if(itemName is null)
            {
                return NotFound();
            }
            List<Inventory> inventoryList = new List<Inventory>();
            inventoryList.Add(inventoryFromApi);

            var inventory = await _inventoryRepository.CreateUpdate(inventoryList);
            return inventoryList;
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

        /* public async Task<IActionResult> Index(string movieGenre, string searchString)
         {
             if (_context.Movie == null)
             {
                 return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
             }

             // Use LINQ to get list of genres.
             IQueryable<string> genreQuery = from m in _context.Movie
             orderby m.Genre
                                             select m.Genre;
             var movies = from m in _context.Movie
                          select m;

             if (!string.IsNullOrEmpty(searchString))
             {
                 movies = movies.Where(s => s.Title!.Contains(searchString));
             }

             if (!string.IsNullOrEmpty(movieGenre))
             {
                 movies = movies.Where(x => x.Genre == movieGenre);
             }

             var movieGenreVM = new MovieGenreViewModel
             {
                 Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                 Movies = await movies.ToListAsync()
             };

             return View(movieGenreVM);
         }*/
    }
}