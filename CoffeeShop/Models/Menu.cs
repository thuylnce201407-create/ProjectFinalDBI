using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }

        [Required]
        public string MenuName { get; set; } = "";

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryID { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = "Available";

        public bool IsActive { get; set; } = true;

        public Category? Category { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}