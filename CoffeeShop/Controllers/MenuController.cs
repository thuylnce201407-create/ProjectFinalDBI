using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Controllers
{
    public class MenuController : BaseController
    {
        private readonly CoffeeDbContext _context;

        public MenuController(CoffeeDbContext context)
        {
            _context = context;
        }

        //Danh sách
        public IActionResult Index()
        {
            var result = CheckLogin();
            if (result != null)
                return result;

            var menus = _context.Menus
                                .Include(x => x.Category)
                                .Where(x => x.IsActive)
                                .ToList();

            return View(menus);
        }

        //Form thêm
        public IActionResult Create()
        {
            var result = CheckLogin();
            if (result != null)
                return result;
            ViewBag.Categories = new SelectList(
                _context.Categories,
                "CategoryID",
                "CategoryName");

            return View();
        }

        //Lưu dữ liệu
        [HttpPost]
        public IActionResult Create(Menu menu)
        {
            var result = CheckLogin();
            if (result != null)
                return result;
            if (ModelState.IsValid)
            {
                _context.Menus.Add(menu);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(
                _context.Categories,
                "CategoryID",
                "CategoryName");

            return View(menu);
        }

        // =================== EDIT ===================

        // Hiển thị form Edit
        public IActionResult Edit(int id)
        {
            var result = CheckLogin();
            if (result != null)
                return result;
            var menu = _context.Menus.Find(id);

            if (menu == null)
                return NotFound();

            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Categories,
                "CategoryID",
                "CategoryName",
                menu.CategoryID);

            return View(menu);
        }

        // Lưu Edit
        [HttpPost]
        public IActionResult Edit(Menu menu)
        {
            var result = CheckLogin();
            if (result != null)
                return result;
            if (ModelState.IsValid)
            {
                _context.Menus.Update(menu);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Categories,
                "CategoryID",
                "CategoryName",
                menu.CategoryID);

            return View(menu);
        }

        // =================== DELETE ===================

        // Hiển thị xác nhận
        public IActionResult Delete(int id)
        {
            var result = CheckLogin();
            if (result != null)
                return result;
            var menu = _context.Menus
                               .Include(x => x.Category)
                               .FirstOrDefault(x => x.MenuID == id);

            if (menu == null)
                return NotFound();

            return View(menu);
        }

        // Xóa dữ liệu
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = CheckLogin();
            if (result != null)
                return result;
            var menu = _context.Menus.Find(id);

            if (menu != null)
            {
                _context.Menus.Remove(menu);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}