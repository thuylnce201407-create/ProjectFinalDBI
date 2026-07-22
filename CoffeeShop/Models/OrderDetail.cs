using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int MenuID { get; set; }

        public int Quantity { get; set; }

        public Order? Order { get; set; }

        public Menu? Menu { get; set; }
    }
}