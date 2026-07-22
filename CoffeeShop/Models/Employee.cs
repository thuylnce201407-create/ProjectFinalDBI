using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        public string FullName { get; set; } = "";

        public string Position { get; set; } = "";

        public decimal Salary { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        // Thêm mới
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public List<Order>? Orders { get; set; }
    }
}