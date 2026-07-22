using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CoffeeShop.Controllers
{
    public class CartController : BaseController
    {
        private readonly CoffeeDbContext _context;

        public CartController(CoffeeDbContext context)
        {
            _context = context;
        }

        //---------------------------------
        // Hiển thị Cart
        //---------------------------------

        public IActionResult Index()
        {
            var result = CheckCustomer();

            if (result != null)
                return result;

            var cart = GetCart();

            return View(cart);
        }

        //---------------------------------
        // Add To Cart
        //---------------------------------

        public IActionResult Add(int id)
        {
            var result = CheckCustomer();

            if (result != null)
                return result;

            var menu = _context.Menus.FirstOrDefault(x => x.MenuID == id);

            if (menu == null)
                return NotFound();

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MenuID == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MenuID = menu.MenuID,
                    MenuName = menu.MenuName,
                    Price = menu.Price,
                    Quantity = 1,
                    ImageUrl = menu.ImageUrl
                });
            }
            else
            {
                item.Quantity++;
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        //---------------------------------
        // Remove
        //---------------------------------

        public IActionResult Remove(int id)
        {
            var result = CheckCustomer();

            if (result != null)
                return result;

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MenuID == id);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        //---------------------------------
        // Increase
        //---------------------------------

        public IActionResult Increase(int id)
        {
            var result = CheckCustomer();

            if (result != null)
                return result;

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MenuID == id);

            if (item != null)
            {
                item.Quantity++;
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        //---------------------------------
        // Decrease
        //---------------------------------

        public IActionResult Decrease(int id)
        {
            var result = CheckCustomer();

            if (result != null)
                return result;

            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MenuID == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index));
        }

        //---------------------------------
        // Checkout
        //---------------------------------

        public IActionResult Checkout()
        {
            var result = CheckCustomer();

            if (result != null)
                return result;

            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Message"] = "Cart is empty!";
                return RedirectToAction(nameof(Index));
            }

            int customerId = HttpContext.Session.GetInt32("CustomerID") ?? 0;

            // Tính tổng tiền giỏ hàng
            decimal total = cart.Sum(x => x.Total);

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerID = customerId,

                // Employee mặc định
                EmployeeID = 1,

                Status = "Paid",

                TotalAmount = total
            };


            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    MenuID = item.MenuID,
                    Quantity = item.Quantity
                };

                _context.OrderDetails.Add(detail);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            TempData["Success"] = "Order successfully!";

            return RedirectToAction("MyOrders", "Order");
        }

        //---------------------------------
        // Helper
        //---------------------------------

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(json))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(json)
                   ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);

            HttpContext.Session.SetString("Cart", json);
        }
    }
}