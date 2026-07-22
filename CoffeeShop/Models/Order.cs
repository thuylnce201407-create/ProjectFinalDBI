using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = "Unpaid";

        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public Customer? Customer { get; set; }

        public decimal TotalAmount { get; set; }

        public Employee? Employee { get; set; }

        // Không để nullable
        public List<OrderDetail> OrderDetails { get; set; } = new();

        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                return OrderDetails.Sum(x =>
                    x.Quantity * (x.Menu?.Price ?? 0));
            }
        }
    }
}