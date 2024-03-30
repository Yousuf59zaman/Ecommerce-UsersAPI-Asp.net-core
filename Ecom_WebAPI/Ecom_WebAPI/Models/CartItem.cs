namespace Ecom_WebAPI.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; } // Assuming you're tracking which user the cart item belongs to
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
