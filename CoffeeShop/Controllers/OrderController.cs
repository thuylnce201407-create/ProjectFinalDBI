using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Controllers
{
    public class OrderController : BaseController
    {
        private readonly CoffeeDbContext _context;

        public OrderController(CoffeeDbContext context)
        {
            _context = context;
        }

        // ===========================
        // INDEX (Employee)
        // ===========================
        public IActionResult Index(string? keyword)
        {
            var result = CheckEmployee();

            if (result != null)
                return result;

            var orders = _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Menu)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                orders = orders.Where(x =>
                    x.Customer.CustomerName.Contains(keyword) ||
                    x.Employee.FullName.Contains(keyword) ||
                    x.Status.Contains(keyword));
            }

            ViewBag.Keyword = keyword;

            var list = orders.ToList();

            foreach (var item in list)
            {
                item.TotalAmount = item.OrderDetails.Sum(x => x.Quantity * x.Menu.Price);
            }

            return View(list);
        }

        // ===========================
        // DETAILS
        // ===========================
        public IActionResult Details(int id)
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Menu)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
                return NotFound();

            order.TotalAmount = order.OrderDetails.Sum(x => x.Quantity * x.Menu.Price);

            return View(order);
        }

        // ===========================
        // CREATE
        // ===========================
        public IActionResult Create()
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            LoadDropDown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            LoadDropDown();

            return View(order);
        }

        // ===========================
        // EDIT
        // ===========================
        public IActionResult Edit(int id)
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            var order = _context.Orders.Find(id);

            if (order == null)
                return NotFound();

            LoadDropDown();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            if (ModelState.IsValid)
            {
                _context.Orders.Update(order);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            LoadDropDown();

            return View(order);
        }

        // ===========================
        // DELETE
        // ===========================
        public IActionResult Delete(int id)
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            var details = _context.OrderDetails
                .Where(x => x.OrderID == id)
                .ToList();

            _context.OrderDetails.RemoveRange(details);

            var order = _context.Orders.Find(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // INVOICE
        // ===========================
        public IActionResult Invoice(int id)
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Menu)
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
                return NotFound();

            order.TotalAmount = order.OrderDetails.Sum(x => x.Quantity * x.Menu.Price);

            return View(order);
        }

        // ===========================
        // MY ORDERS (Customer)
        // ===========================
        public IActionResult MyOrders()
        {
            var result = CheckCustomer();

            if (result != null)
                return result;

            int customerId = HttpContext.Session.GetInt32("CustomerID") ?? 0;

            var orders = _context.Orders
                .Where(o => o.CustomerID == customerId)
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Menu)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            foreach (var item in orders)
            {
                item.TotalAmount = item.OrderDetails.Sum(x => x.Quantity * x.Menu.Price);
            }

            return View(orders);
        }

        // ===========================
        // LOAD DROPDOWN
        // ===========================
        private void LoadDropDown()
        {
            ViewBag.Customers = new SelectList(
                _context.Customers,
                "CustomerID",
                "CustomerName");

            ViewBag.Employees = new SelectList(
                _context.Employees,
                "EmployeeID",
                "FullName");
        }
    }
}