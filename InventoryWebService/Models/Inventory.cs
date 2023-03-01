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
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime LastUpdatedOn { get; set; } = DateTime.Now;
    }
}
