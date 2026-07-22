using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly CoffeeDbContext _context;

        public HomeController(CoffeeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId, string? keyword)
        {
            var menus = _context.Menus
                    .Include(x => x.Category)
                    .Where(x => x.Status == "Available")
                    .AsQueryable();

            // Filter Category
            if (categoryId.HasValue)
            {
                menus = menus.Where(x => x.CategoryID == categoryId.Value);
            }

            // Search
            if (!string.IsNullOrEmpty(keyword))
            {
                menus = menus.Where(x =>
                    x.MenuName.Contains(keyword) ||
                    x.Category.CategoryName.Contains(keyword));
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Keyword = keyword;

            return View(menus.ToList());
        }

        public IActionResult Detail(int id)
        {
            var menu = _context.Menus
                               .Include(x => x.Category)
                               .FirstOrDefault(x => x.MenuID == id);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}