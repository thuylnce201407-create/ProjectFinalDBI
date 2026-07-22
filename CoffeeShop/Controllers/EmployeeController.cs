using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly CoffeeDbContext _context;

        public EmployeeController(CoffeeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? keyword)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            var employees = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                employees = employees.Where(x =>
                    x.FullName.Contains(keyword) ||
                    x.Username.Contains(keyword) ||
                    x.Phone.Contains(keyword));
            }

            ViewBag.Keyword = keyword;

            return View(employees.ToList());
        }

        public IActionResult Create()
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"{item.Key}: {error.ErrorMessage}");
                    }
                }

                return View(employee);
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            var employee = _context.Employees.Find(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            if (ModelState.IsValid)
            {
                _context.Employees.Update(employee);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        public IActionResult Delete(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            var employee = _context.Employees.Find(id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            var employee = _context.Employees.Find(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}