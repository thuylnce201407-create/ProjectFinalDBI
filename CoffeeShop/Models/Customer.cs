using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        public string CustomerName { get; set; } = "";

        public string? Phone { get; set; }

        public string? Email { get; set; }

        // ===== Login =====

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        // Navigation

        public List<Order>? Orders { get; set; }
    }
}