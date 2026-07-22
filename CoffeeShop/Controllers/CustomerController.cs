using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly CoffeeDbContext _context;

        public CustomerController(CoffeeDbContext context)
        {
            _context = context;
        }

        // ======================
        // Index
        // ======================
        public IActionResult Index(string? keyword)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            var customers = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                customers = customers.Where(x =>
                    x.CustomerName.Contains(keyword) ||
                    x.Username.Contains(keyword) ||
                    x.Phone.Contains(keyword));
            }

            ViewBag.Keyword = keyword;

            return View(customers.ToList());
        }

        // ======================
        // Create
        // ======================
        public IActionResult Create()
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // ======================
        // Edit
        // ======================
        public IActionResult Edit(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            var customer = _context.Customers.Find(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            if (ModelState.IsValid)
            {
                _context.Customers.Update(customer);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // ======================
        // Delete
        // ======================
        public IActionResult Delete(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            var customer = _context.Customers.Find(id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            var customer = _context.Customers.Find(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}