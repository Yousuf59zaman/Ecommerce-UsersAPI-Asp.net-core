using Ecom_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CartController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        DAL_cart _dal = new DAL_cart();

        [HttpPost]
        [Route("add")]
        public IActionResult AddToCart([FromBody] CartItem cartItem)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            bool result = _dal.AddToCart(cartItem, connectionString);
            if (result)
            {
                return Ok(new Response<CartItem> { Success = true, Message = "Added to cart successfully", Data = cartItem });
            }

            return BadRequest(new Response<CartItem> { Success = false, Message = "Adding to cart failed" });
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetCartItems(int userId)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            var cartItems = _dal.GetCartItems(userId, connectionString);
            return Ok(new Response<List<CartItem>> { Success = true, Message = "Cart items retrieved successfully", Data = cartItems });
        }

        [HttpDelete]
        [Route("remove/{cartItemId}")]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            bool result = _dal.RemoveFromCart(cartItemId, connectionString);
            if (result)
            {
                return Ok(new Response<bool> { Success = true, Message = "Removed from cart successfully", Data = true });
            }

            return BadRequest(new Response<bool> { Success = false, Message = "Removing from cart failed" });
        }



    }
}
