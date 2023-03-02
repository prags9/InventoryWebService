using InventoryWebService.Models;
using System.Xml.Linq;

namespace Test.Infrastructure
{
    public class MockData
    {        
        public static List<Inventory> GetMockData()
        {
            return new List<Inventory>()
            {
                new Inventory
                    {
                        Name = "Apples",
                       Quantity = 10,
                       CreatedOn = DateTime.Parse("2000-2-12"),
                       LastUpdatedOn = DateTime.Now
                    },
                    new Inventory
                    {
                        Name = "Oranges",
                        Quantity = 10,
                        CreatedOn = DateTime.Now,
                        LastUpdatedOn = DateTime.Now
                    }, new Inventory
                    {
                        Name = "Chiku",
                        Quantity = 10,
                        CreatedOn = DateTime.Now,
                        LastUpdatedOn = DateTime.Now
                    },
                    new Inventory{
                        Name = "Papaya",
                        Quantity = 10,
                        CreatedOn = DateTime.Now,
                        LastUpdatedOn = DateTime.Now
                    }
            };
        }
    }
}
