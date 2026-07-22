using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult? CheckLogin()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return null;
        }

        protected IActionResult? CheckEmployee()
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            if (HttpContext.Session.GetString("Role") != "Employee")
            {
                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        protected IActionResult? CheckCustomer()
        {
            var result = CheckLogin();

            if (result != null)
                return result;

            if (HttpContext.Session.GetString("Role") != "Customer")
            {
                return RedirectToAction("Index", "Home");
            }

            return null;
        }
    }
}