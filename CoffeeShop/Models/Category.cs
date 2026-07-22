using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("Category")]
    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = "";

        public List<Menu>? Menus { get; set; }
    }
}