using System.ComponentModel.DataAnnotations;

namespace InventoryWebService.Models
{
    public class Inventory
    {
        [Key]
        public string Name { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
    }
}
