using Ecom_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom_WebAPI.DTO;

namespace Ecom_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        DAL_order _dal = new DAL_order();
        public OrdersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("placeOrder")]
        public IActionResult PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            int orderId = _dal.PlaceOrder(request.Order, request.OrderItems, connectionString);
            if (orderId > 0)
            {
                return Ok(new Response<int> { Success = true, Message = "Order placed successfully", Data = orderId });
            }

            return BadRequest(new Response<int> { Success = false, Message = "Placing order failed" });
        }


        [HttpGet]
        [Route("history/{userId}")]
        public IActionResult GetOrderHistory(int userId)
        {
            string connectionString = _configuration.GetConnectionString("EcomDB");
            var orders = _dal.GetOrdersByUserId(userId, connectionString);
            if (orders != null)
            {
                return Ok(new Response<List<Order>> { Success = true, Message = "Order history retrieved successfully", Data = orders });
            }

            return NotFound(new Response<List<Order>> { Success = false, Message = "No order history found" });
        }



    }
}
