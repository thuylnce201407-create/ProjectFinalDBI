using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Controllers
{
    public class OrderDetailController : BaseController
    {
        private readonly CoffeeDbContext _context;

        public OrderDetailController(CoffeeDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            var details = _context.OrderDetails
                                  .Include(x => x.Order)
                                  .Include(x => x.Menu)
                                  .ToList();

            return View(details);
        }

        // =========================
        // DETAILS
        // =========================
        public IActionResult Details(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            var detail = _context.OrderDetails
                                 .Include(x => x.Order)
                                 .Include(x => x.Menu)
                                 .FirstOrDefault(x => x.OrderDetailID == id);

            if (detail == null)
                return NotFound();

            return View(detail);
        }

        // =========================
        // CREATE
        // =========================
        public IActionResult Create()
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            LoadDropDown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderDetail detail)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            if (ModelState.IsValid)
            {
                _context.OrderDetails.Add(detail);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            LoadDropDown();

            return View(detail);
        }

        // =========================
        // EDIT
        // =========================
        public IActionResult Edit(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            var detail = _context.OrderDetails.Find(id);

            if (detail == null)
                return NotFound();

            LoadDropDown();

            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderDetail detail)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            if (ModelState.IsValid)
            {
                _context.OrderDetails.Update(detail);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            LoadDropDown();

            return View(detail);
        }

        // =========================
        // DELETE
        // =========================
        public IActionResult Delete(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            var detail = _context.OrderDetails
                                 .Include(x => x.Order)
                                 .Include(x => x.Menu)
                                 .FirstOrDefault(x => x.OrderDetailID == id);

            if (detail == null)
                return NotFound();

            return View(detail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = CheckEmployee();
            if (result != null)
                return result;

            var detail = _context.OrderDetails.Find(id);

            if (detail != null)
            {
                _context.OrderDetails.Remove(detail);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // LOAD DROPDOWN
        // =========================
        private void LoadDropDown()
        {
            ViewBag.Orders = new SelectList(
                _context.Orders,
                "OrderID",
                "OrderID");

            ViewBag.Menus = new SelectList(
                _context.Menus,
                "MenuID",
                "MenuName");
        }
    }
}