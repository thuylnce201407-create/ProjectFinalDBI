using CoffeeShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly CoffeeDbContext _context;

        public CategoryController(CoffeeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = CheckEmployee();
            if (result != null)
                return result;
            var categories = _context.Categories.ToList();

            return View(categories);
        }
    }
}