using CoffeeShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly CoffeeDbContext _context;

        public AccountController(CoffeeDbContext context)
        {
            _context = context;
        }

        //----------------------------------
        // GET
        //----------------------------------

        public IActionResult Login()
        {
            return View();
        }

        //----------------------------------
        // POST
        //----------------------------------

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // ================= Employee =================

            var employee = _context.Employees
                .FirstOrDefault(x =>
                    x.Username == username &&
                    x.Password == password);

            if (employee != null)
            {
               
                HttpContext.Session.SetInt32("EmployeeID", employee.EmployeeID);
                HttpContext.Session.SetString("Username", employee.Username);
                HttpContext.Session.SetString("FullName", employee.FullName);
                HttpContext.Session.SetString("Role", "Employee");

                return RedirectToAction("Index", "Home");
            }

            // ================= Customer =================

            var customer = _context.Customers
                .FirstOrDefault(x =>
                    x.Username == username &&
                    x.Password == password);

            if (customer != null)
            {
                
                HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                HttpContext.Session.SetString("Username", customer.Username);
                HttpContext.Session.SetString("FullName", customer.CustomerName);
                HttpContext.Session.SetString("Role", "Customer");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";

            return View();
        }



        //----------------------------------
        // Logout
        //----------------------------------

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}