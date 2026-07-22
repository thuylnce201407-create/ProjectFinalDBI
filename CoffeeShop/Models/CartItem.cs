namespace CoffeeShop.Models
{
    public class CartItem
    {
        public int MenuID { get; set; }

        public string MenuName { get; set; } = "";

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string ImageUrl { get; set; } = "";

        public decimal Total
        {
            get
            {
                return Price * Quantity;
            }
        }
    }
}